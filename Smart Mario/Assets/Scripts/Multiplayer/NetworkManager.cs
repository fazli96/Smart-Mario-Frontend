using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages all events related to multiplayer 
/// including listeners for events from the server and events to be emitted to the server
/// It manages the communication between the client and the server on the client side
/// </summary>
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    private SceneController scene;
    public SocketIOComponent socket;
    public GameObject[] players = new GameObject[2];
    public GameObject[] playersMinigame = new GameObject[2];

    private static string roomName;
    private static int roomCapacity;
    private static string roomPassword;
    public static string playerName;
    public static int customChar;
    private static string roomID;
    private static string minigameSelected;
    private static string difficultySelected;
    private static int levelSelected;
    public static bool isOwner;
    public static UserJSON storedUserJSON;

    /// <summary>
    /// This method is called to get the instance of scene controller and to enable gameObjects to persist
    /// when new scene is loaded
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        if (socket)
        DontDestroyOnLoad(socket);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// This method is called to destroy the persistent GameObjects when main menu screen is loaded
    /// meaning when player navigates out of multiplayer mode
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject);
            Destroy(GameObject.Find("SocketIO"));
            Debug.Log("I am inside the if statement");
        }
    }

    /// <summary>
    /// This method is called before the the first frame update to subscribe to all websocket events 
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
        //subscribe to all the various websocket events
        socket.On("other player connected", OnOtherPlayerConnected);
        socket.On("other player connected minigame", OnOtherPlayerConnectedMinigame);
        socket.On("play", OnPlay);
        socket.On("player move", OnPlayerMove);
        socket.On("other player disconnected", OnOtherPlayerDisconnected);
        socket.On("owner disconnected", OnOwnerDisconnected);
        socket.On("get rooms", OnGetRooms);
        socket.On("room is full", OnRoomIsFull);
        socket.On("wrong room password", OnWrongRoomPassword);
        socket.On("minigame start", OnMinigameStart);
        socket.On("next player", OnNextPlayer);
        socket.On("update message", OnUpdateMessage);
        socket.On("score change", OnScoreChange);
        socket.On("one player left", OnOnePlayerLeft);
        socket.On("end game", OnEndGame);
        socket.On("player left minigame", OnPlayerLeftMinigame);
        socket.On("minigame2 enter", OnMinigame2Enter);
        socket.On("minigame2 start", OnMinigame2Start);
        socket.On("end game2", OnEndGame2);
    }
    
    /// <summary>
    /// This method is called when a player has joined a challenge room
    /// It navigates the player to the challenge room and spawn the local player and other players already in the room
    /// </summary>
    public void JoinRoom()
    {
        StartCoroutine(ConnectToRoom());
    }

    /// <summary>
    /// This method is called when the owner of a challenge room has started the challenge
    /// It navigates the players in the room to the minigame selected
    /// </summary>
    public void StartChallenge()
    {
        switch (PlayerPrefs.GetString("Minigame Selected", "World 1 Stranded"))
        {
            case "World 1 Stranded":
                scene.ToWorld1StrandedMultiplayer();
                break;
            case "World 1 Matching Cards":
                scene.ToWorld1MatchingMultiplayer();
                break;
            case "World 2 Stranded":
                scene.ToWorld2StrandedMultiplayer();
                break;
            case "World 2 Matching Cards":
                scene.ToWorld2MatchingMultiplayer();
                break;
            default:
                break;
        }
    }

    #region Commands
    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to subscribe to websocket events available only to the current room
    /// </summary>
    /// <returns></returns>
    IEnumerator ConnectToRoom()
    {
        scene.ToMultiplayerRoom();

        roomName = PlayerPrefs.GetString("roomName", "room1");
        roomCapacity = PlayerPrefs.GetInt("roomCapacity", 4);
        roomPassword = PlayerPrefs.GetString("roomPassword", "");
        playerName = PlayerPrefs.GetString("username", "fazli");
        customChar = int.Parse(PlayerPrefs.GetString("customChar", "0"));
        roomID = PlayerPrefs.GetString("roomID", "create");
        minigameSelected = PlayerPrefs.GetString("Minigame Selected", "World 2 Stranded");
        difficultySelected = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        levelSelected = PlayerPrefs.GetInt("MinigameLevel", 1);

        if (roomID.Equals("create"))
        {
            isOwner = true;
        }
        else
        {
            isOwner = false;
        }

        //wait for scene to finish loading
        while (GameObject.Find("RoomManager") == null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // initialize player data to send to the server
        List<SpawnPoint> playerSpawnPoints = GameObject.Find("RoomManager").GetComponent<PlayerSpawner>().playerSpawnPoints;
        storedUserJSON = new UserJSON(playerName, customChar, roomID, isOwner);
        PlayerJSON playerJSON = new PlayerJSON(playerName, customChar, roomID, isOwner, roomName, roomCapacity, roomPassword,
            minigameSelected, difficultySelected, levelSelected, playerSpawnPoints);
        string data = JsonUtility.ToJson(playerJSON);
        Debug.Log(data);
        // send websocket event to server to check for existence of players already in the challenge room
        socket.Emit("player connect", new JSONObject(data));
        yield return new WaitForSeconds(0.1f);
        // send websocket event to server to indicate that the player has joined the challenge room
        socket.Emit("play", new JSONObject(data));
    }

    /// <summary>
    /// This method is called when the local player moves. 
    /// It sends the player's position to the server to broadcast to the other players
    /// </summary>
    /// <param name="vec3"></param>
    public void CommandMove(Vector3 vec3)
    {
        string data = JsonUtility.ToJson(new PositionJSON(vec3));
        // send websocket event to server of the local player current position
        socket.Emit("player move", new JSONObject(data));
    }

    /// <summary>
    /// This method is called to get the rooms available from the server
    /// </summary>
    public void CommandGetRooms()
    {
        // send websocket event to server to get all rooms currently available
        socket.Emit("get rooms");
    }

    /// <summary>
    /// This method is called when the player leaves the game
    /// It alerts other players via the server that the player has left the game.
    /// </summary>
    public void CommandLeaveChallenge()
    {
        scene.ToMultiplayerLobby();
        // send websocket event to server to indicate that the player has left the challenge (disconnected)
        socket.Emit("disconnect");
    }


    public void CommandMinigame2Enter()
    {
        Debug.Log("Emit minigame2 connect");
        string data = JsonUtility.ToJson(storedUserJSON);
        print("emit minigame connect");
        // send websocket event to server the locations of question barrels initialized by owner
        socket.Emit("minigame connect", new JSONObject(data));
        socket.Emit("minigame2 enter");
    }

    public void CommandMinigame2Start()
    {
        Debug.Log("Emit minigame2 start");
        //
        socket.Emit("minigame2 start");
    }

    /// <summary>
    /// This method is called when the owner of the challenge room has finished initializing the minigame selected.
    /// Question barrels initialized are send to other players via the server so that the question barrels locations
    /// are the same for all players
    /// This method is also to alert other players that the challenge has started before 
    /// transporting the players from the challenge room to the minigame selected
    /// </summary>
    /// <param name="questionBarrelLocations"></param>
    public void CommandMinigameStart(List<int> questionBarrelLocations)
    {
        print("minigame start command");
        StartCoroutine(MinigameStart(questionBarrelLocations));

    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to emit websocket events to other players
    /// once the challenge has started by the owner
    /// </summary>
    /// <returns></returns>
    IEnumerator MinigameStart(List<int> questionBarrelLocations)
    {
        Debug.Log("Emit minigame connect");
        string data = JsonUtility.ToJson(storedUserJSON);
        print("emit minigame connect");
        // send websocket event to server the locations of question barrels initialized by owner
        socket.Emit("minigame connect", new JSONObject(data));
        yield return new WaitForSeconds(0.1f);
        string questionBarrelData = JsonUtility.ToJson(new QuestionBarrelJSON(questionBarrelLocations));
        // send websocket event to server to indicate that the challenge has started
        socket.Emit("minigame start", new JSONObject(questionBarrelData));
    }

    /// <summary>
    /// This method is called when the player finished his/her turn.
    /// It alerts other players via the server that the player has ended his/her turn
    /// </summary>
    public void CommandEndTurn()
    {
        print("end turn");
        string data = JsonUtility.ToJson(storedUserJSON);
        // send websocket event to server to indicate that the player has ended his/her turn
        socket.Emit("end turn", new JSONObject(data));
    }

    /// <summary>
    /// This method is called when the player rolled the dice.
    /// It alerts other players via the server that the player has rolled the dice
    /// </summary>
    public void CommandRollDice(int diceNumber)
    {
        print("roll dice");
        string data = JsonUtility.ToJson(new OneIntVariableJSON(diceNumber));
        // send websocket event to server to indicate that the player has rolled the dice
        socket.Emit("roll dice", new JSONObject(data));
    }

    /// <summary>
    /// This method is called when the player is answering a question.
    /// It alerts other players via the server that the player is answering a question
    /// </summary>
    public void CommandAnsweringQuestion()
    {
        print("answering question");
        // send websocket event to server to indicate that the player is answering a question
        socket.Emit("answer question");
    }

    /// <summary>
    /// This method is called when the player has answered the question.
    /// It alerts other players via the server that the player has answered the question
    /// </summary>
    public void CommandQnResult(int scoreChange)
    {
        print("qn result");
        string data = JsonUtility.ToJson(new OneIntVariableJSON(scoreChange));
        // send websocket event to server to indicate that the player has answered the question
        socket.Emit("qn result", new JSONObject(data));
    }

    /// <summary>
    /// This method is called when the player has reached the ending, hence has completed the level.
    /// It alerts other players via the server that the player has completed the level
    /// </summary>
    public void CommandEndGame()
    {
        print("end game");
        // send websocket event to server to indicate that the player has reached the ending hence has completed the level
        socket.Emit("end game");
    }
    public void CommandEndGame2()
    {
        print("end game");
        // send websocket event to server to indicate that the player has reached the ending hence has completed the level
        socket.Emit("end game2");
    }

    public void CommandMatchedCard(int pairsLeft)
    {
        print("Matched a card");
        string data = JsonUtility.ToJson(new OneIntVariableJSON(pairsLeft));
        socket.Emit("matched card", new JSONObject(data));
    }

    #endregion

    #region Listening

    /// <summary>
    /// This is a listener that listens for websocket event where a non-local player has ended his/her turn
    /// if the next player is the local player, set the current turn of the local player to true.
    /// </summary>
    /// <param name="socketIOEvent when a player ends his/her turn"></param>
    void OnNextPlayer(SocketIOEvent socketIOEvent)
    {
        print("next player");
        string data = socketIOEvent.data.ToString();
        UserJSON nextPlayerJSON = UserJSON.CreateFromJSON(data);
        if (storedUserJSON.name.Equals(nextPlayerJSON.name))
        {
            StrandedMultiplayerGameManager.currentTurn = true;
            StrandedMultiplayerGameManager.instance.ShowDice();
        }
    }
    void OnMinigame2Enter(SocketIOEvent socketIOEvent)
    {
        print("minigame2 enter");
        if (PlayerPrefs.GetString("Minigame Selected", "World 1 Matching Cards")=="World 1 Matching Cards")
            scene.ToWorld1MatchingMultiplayer();
        else
            scene.ToWorld2MatchingMultiplayer();
    }
    void OnMinigame2Start(SocketIOEvent socketIOEvent)
    {
        print("minigame2 started");
        //scene.ToWorld1MatchingMultiplayer();
        MatchingMultiplayerGameManager.instance.changeStartState();

    }

    /// <summary>
    /// This is a listener that listens for websocket event a non-local player who is the owner
    /// of the challenge room has started the challenge
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnMinigameStart(SocketIOEvent socketIOEvent)
    {
        print("minigame start");
        string data = socketIOEvent.data.ToString();
        QuestionBarrelJSON questionBarrelJSON = QuestionBarrelJSON.CreateFromJSON(data);
        StartCoroutine(LoadMinigame(questionBarrelJSON.questionBarrelLocations));
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to initialize question Barrels for players who are not the owner of the challenge room
    /// </summary>
    /// <param name="questionBarrelLocations"></param>
    /// <returns></returns>
    IEnumerator LoadMinigame(List<int> questionBarrelLocations)
    {
        if (PlayerPrefs.GetString("Minigame Selected", "World 1 Stranded")== "World 1 Stranded")
            scene.ToWorld1StrandedMultiplayer();
        else
            scene.ToWorld2StrandedMultiplayer();

        while (GameObject.Find("StrandedMultiplayerGameManager") == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StrandedMultiplayerGameManager.instance.SpawnQuestionBarrelsMultiplayer(questionBarrelLocations);
        string data = JsonUtility.ToJson(storedUserJSON);
        print("emit minigame connect");
        socket.Emit("minigame connect", new JSONObject(data));
    }
    
    /// <summary>
    /// This is a listener that listens for websocket event where there exist a player 
    /// currently in the same challenge room the player is joining
    /// The player is instantiated in the challenge room as a player gameObject
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
    {
        print("Someone else joined");
        string data = socketIOEvent.data.ToString();
        UserJSON otherUserJSON = UserJSON.CreateFromJSON(data);
        roomID = otherUserJSON.roomID;
        Vector3 position = new Vector3(otherUserJSON.position[0], otherUserJSON.position[1], otherUserJSON.position[2]);
        GameObject o = GameObject.Find(otherUserJSON.name) as GameObject;
        if (o != null)
        {
            return;
        }
        GameObject p = Instantiate(players[customChar], position, Quaternion.identity);
        PlayerMovement pm = p.GetComponent<PlayerMovement>();
        Transform t = p.transform.Find("Player Name Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = otherUserJSON.name;
        pm.isLocalPlayer = false;
        pm.isOwner = otherUserJSON.isOwner;
        pm.multiplayer = true;
        p.name = otherUserJSON.name;
    }

    /// <summary>
    /// This is a listener that listens for websocket event where there exist a player 
    /// currently in the same challenge the player is in
    /// The player is instantiated in the minigame as a player gameObject
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnOtherPlayerConnectedMinigame(SocketIOEvent socketIOEvent)
    {
        
        string data = socketIOEvent.data.ToString();
        UserJSON otherUserJSON = UserJSON.CreateFromJSON(data);
        print(otherUserJSON.name + " joined");
        GameObject o = GameObject.Find(otherUserJSON.name) as GameObject;
        if (o != null)
        {
            return;
        }
        GameObject p = Instantiate(playersMinigame[customChar], StrandedMultiplayerGameManager.instance.GetStartWayPoint().transform.position, Quaternion.identity) as GameObject;
        PlayerPathMovement pm = p.GetComponent<PlayerPathMovement>();
        Transform t = p.transform.Find("Player Name Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = otherUserJSON.name;
        pm.isLocalPlayer = false;
        pm.multiplayer = true;
        pm.isOwner = otherUserJSON.isOwner;
        p.name = otherUserJSON.name;
        StartCoroutine(AddPlayerToGameStatus(otherUserJSON.name));
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to add other players connected to the same challenge and
    /// display the initial scores of these players
    /// </summary>
    /// <param name="playerName"></param>
    /// <returns></returns>
    IEnumerator AddPlayerToGameStatus(string playerName)
    {
        StrandedMultiplayerGameStatus.instance.AddPlayer(playerName);
        yield return new WaitForSeconds(0.1f);
        StrandedMultiplayerGameStatus.instance.DisplayOtherPlayerScore(playerName);
    }

    /// <summary>
    /// This is a listener that listens for websocket event where there is a new message in the message log
    /// The new message will be appended to the message log and displayed on the screen
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnUpdateMessage(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        MessageJSON currentMessage = MessageJSON.CreateFromJSON(data);
        if (currentMessage.message.Equals(storedUserJSON.name + "'s turn"))
            StartCoroutine(UpdateMessage("Your turn"));
        else
            StartCoroutine(UpdateMessage(currentMessage.message));
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to update messages in the message log and
    /// display the messages on the screen
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    IEnumerator UpdateMessage(string message)
    {
        yield return new WaitForSeconds(0.1f);
        ChallengeManager.instance.SendToMessageLog(message);
    }

    /// <summary>
    /// This is a listener that listens for websocket event where the score of non-local player has changed
    /// The scores of the player will be updated acccordingly based on the scoreChange retrieved
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnScoreChange(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        ScoreChangeJSON scoreChange = ScoreChangeJSON.CreateFromJSON(data);
        StartCoroutine(ScoreChange(scoreChange));
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to update the affected player score using the scoreChange
    /// </summary>
    /// <param name="scoreChange"></param>
    /// <returns></returns>
    IEnumerator ScoreChange(ScoreChangeJSON scoreChange)
    {
        StrandedMultiplayerGameStatus.instance.OtherPlayerScoreChange(scoreChange.playerName, scoreChange.scoreChange);
        yield return new WaitForSeconds(0.1f);
    }

    /// <summary>
    /// This is a listener that listens for websocket event where a player has completed the level
    /// The game results panel will appear and the game will end regardless of whether the other players has completed the level
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnEndGame(SocketIOEvent socketIOEvent)
    {
        print("end game");
        StrandedMultiplayerGameManager.instance.GameComplete();

    }

    void OnEndGame2(SocketIOEvent socketIOEvent)
    {
        print("end game");
        MatchingMultiplayerGameStatus.instance.WinLevel();
    }

    /// <summary>
    /// This is a listener that listens for websocket event where the non-local player has left the minigame
    /// Game status data such as player score that is related to the player will be destroyed or disposed of
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnPlayerLeftMinigame(SocketIOEvent socketIOEvent)
    {
        print("Player left Minigame");
        string data = socketIOEvent.data.ToString();
        UserJSON UserLeftJSON = UserJSON.CreateFromJSON(data);
        StrandedMultiplayerGameStatus.instance.PlayerLeft(UserLeftJSON.name);
    }

    /// <summary>
    /// This is a listener that listens for websocket event where the local player has joined a challenge room
    /// The local player will be instantiated as a player gameObject in the challenge room
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnPlay(SocketIOEvent socketIOEvent)
    {
        print("You joined");
        string data = socketIOEvent.data.ToString();
        UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
        GameObject p = Instantiate(players[customChar], position, Quaternion.identity) as GameObject;
        PlayerMovement pm = p.GetComponent<PlayerMovement>();
        Transform t = p.transform.Find("Player Name Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = currentUserJSON.name;
        pm.isLocalPlayer = true;
        pm.isOwner = currentUserJSON.isOwner;
        pm.multiplayer = true;
        p.name = currentUserJSON.name;
        RoomManager.instance.ShowRoomDetails(currentUserJSON.roomID);
    }

    /// <summary>
    /// This is a listener that listens for websocket event where a non-local player moves from one position to another new position.
    /// The player gameObject which refers to the player will move towards the position retrieved from the event
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnPlayerMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        // if it is the current player, return
        if (userJSON.name == storedUserJSON.name)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.position = position;
        }
    }

    /// <summary>
    /// This is a listener that listens for websocket event where a non-local player is disconnected.
    /// The player gameObject that refers to the player that is disconnected will be destroyed from the scene
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnOtherPlayerDisconnected(SocketIOEvent socketIOEvent)
    {
        print("user disconnected");
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        print(userJSON.name + " disconnected");
        Destroy(GameObject.Find(userJSON.name));
    }

    /// <summary>
    /// This is a listener that listens for websocket event where 
    /// the non-local player who is the owner of the challenge room is disconnected.
    /// A random player will be selected as the new owner
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnOwnerDisconnected(SocketIOEvent socketIOEvent)
    {
        print("owner disconnected");
        string data = socketIOEvent.data.ToString();
        UserJSON newOwnerJSON = UserJSON.CreateFromJSON(data);
        GameObject p = GameObject.Find(newOwnerJSON.name);
        if (GameObject.Find("StrandedMultiplayerGameManager") == null)
        {
            PlayerMovement pm = p.GetComponent<PlayerMovement>();
            pm.isOwner = true;
            if (pm.isLocalPlayer)
                isOwner = true;
        }
        else
        {
            PlayerPathMovement pm = p.GetComponent<PlayerPathMovement>();
            pm.isOwner = true;
            if (pm.isLocalPlayer)
                isOwner = true;
        }
            
    }

    /// <summary>
    /// This is a listener that listens for websocket event where there is only one player left in the minigame
    /// The player left will be brought back to the multiplayer lobby where an error message will be displayed
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnOnePlayerLeft(SocketIOEvent socketIOEvent)
    {
        print("one player left");
        StartCoroutine(OnePlayerLeft());
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to load the multiplayer scene and display error message
    /// only when the multiplayer lobby scene has fully loaded
    /// </summary>
    /// <returns></returns>
    IEnumerator OnePlayerLeft()
    {
        scene.ToMultiplayerLobby();
        socket.Emit("disconnect");
        while (GameObject.Find("LobbyManager") == null)
            yield return new WaitForSeconds(0.1f);
        LobbyManager.instance.GetComponent<LobbyManager>().NotEnoughPlayers();
    }

    /// <summary>
    /// This is a listener that listens for websocket event where 
    /// the rooms available has been retrieved from the server.
    /// Rooms retrieved will be displayed via a dropdown menu
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnGetRooms(SocketIOEvent socketIOEvent)
    {
        print("Rooms retrieved");
        string data = socketIOEvent.data.ToString();
        Debug.Log(data);
        JObject roomJSONData = (JObject)JsonConvert.DeserializeObject(data);

        LobbyManager.instance.GetComponent<LobbyManager>().AddRooms(roomJSONData);
        //TODO data manipulation
    }

    /// <summary>
    /// This is a listener that listens for websocket event where the room that the local player wants to join is full
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnRoomIsFull(SocketIOEvent socketIOEvent)
    {
        print("room is full");
        StartCoroutine(LoadMultiplayerLobby(0));
    }

    /// <summary>
    /// This is a listener that listens for websocket event where the local player 
    /// has entered the wrong password for the room he/she wants to join
    /// </summary>
    /// <param name="socketIOEvent"></param>
    void OnWrongRoomPassword(SocketIOEvent socketIOEvent)
    {
        print("wrong room password");
        StartCoroutine(LoadMultiplayerLobby(1));
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to load the multiplayer scene and display error message
    /// only when the multiplayer lobby scene has fully loaded
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMultiplayerLobby(int type)
    {
        scene.ToMultiplayerLobby();
        while (GameObject.Find("LobbyManager") == null)
            yield return new WaitForSeconds(0.1f);
        if (type == 0)
            LobbyManager.instance.GetComponent<LobbyManager>().RoomIsFull();
        else
            LobbyManager.instance.GetComponent<LobbyManager>().WrongRoomPassword();
    }
    

    #endregion

    #region JSONMessageClasses
    /// <summary>
    /// This class is to store all relevant player data when creating or joining a challenge room
    /// </summary>
    [Serializable]
    public class PlayerJSON
    {
        public string name;
        public int customChar;
        public string roomID;
        public bool isOwner;
        public string roomName;
        public int capacity;
        public string roomPassword;
        public string minigameSelected;
        public string difficultySelected;
        public int levelSelected;
        public List<PointJSON> playerSpawnPoints;

        /// <summary>
        /// This is a constructor of PlayerJSON
        /// </summary>
        /// <param name="_name of student"></param>
        /// <param name="_customChar character selected"></param>
        /// <param name="_roomID"></param>
        /// <param name="_isOwner of challenge room"></param>
        /// <param name="_roomName"></param>
        /// <param name="_capacity of room"></param>
        /// <param name="_minigameSelected"></param>
        /// <param name="_difficultySelected"></param>
        /// <param name="_levelSelected"></param>
        /// <param name="_playerSpawnPoints locations where player may be spawned at"></param>
        public PlayerJSON(string _name, int _customChar, string _roomID, bool _isOwner, string _roomName, 
            int _capacity, string _roomPassword, string _minigameSelected, string _difficultySelected, int _levelSelected, List<SpawnPoint> _playerSpawnPoints)
        {
            playerSpawnPoints = new List<PointJSON>();
            name = _name;
            customChar = _customChar;
            roomID = _roomID;
            isOwner = _isOwner;
            roomName = _roomName;
            capacity = _capacity;
            roomPassword = _roomPassword;
            minigameSelected = _minigameSelected;
            difficultySelected = _difficultySelected;
            levelSelected = _levelSelected;


            foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoints) 
            {
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                Debug.Log(pointJSON.position[0]);
                playerSpawnPoints.Add(pointJSON);
            }
        }
    }

    /// <summary>
    /// This class is to store the challenge room details
    /// </summary>
    [Serializable]
    public class RoomJSON
    {
        public string roomID;
        public string roomName;
        public string roomOwner;
        public int noOfClients;
        public int roomCapacity;
        public string minigameSelected;
        public string difficultySelected;
        public int levelSelected;

        /// <summary>
        /// This method is to convert jsonString to RoomJSON object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RoomJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<RoomJSON>(data);
        }
    }

    /// <summary>
    /// This class is to store the position of the spawn point
    /// </summary>
    [Serializable]
    public class PointJSON
    {
        public float[] position;

        /// <summary>
        /// This is a constructor of PointJSON
        /// </summary>
        /// <param name="spawnPoint"></param>
        public PointJSON(SpawnPoint spawnPoint)
        {
            position = new float[]
            {
                spawnPoint.transform.position.x,
                spawnPoint.transform.position.y,
                spawnPoint.transform.position.z
            };
        }
    }

    /// <summary>
    /// This class is to store the position of the player
    /// </summary>
    [Serializable]
    public class PositionJSON
    {
        public float[] position;

        /// <summary>
        /// This is a constructor of PositionJSON
        /// </summary>
        /// <param name="_position of player"></param>
        public PositionJSON(Vector3 _position)
        {
            position = new float[] { _position.x, _position.y, _position.z };
        }
    }

    /// <summary>
    /// This class is to store the player data
    /// </summary>
    [Serializable]
    public class UserJSON
    {
        public string name;
        public int customChar;
        public string roomID;
        public bool isOwner;
        public float[] position;

        /// <summary>
        /// This is a constructor of UserJSON
        /// </summary>
        /// <param name="_name of player"></param>
        /// <param name="_customChar character selected"></param>
        /// <param name="_roomID"></param>
        /// <param name="_isOwner of challenge room"></param>
        public UserJSON(string _name, int _customChar, string _roomID, bool _isOwner)
        {
            name = _name;
            customChar = _customChar;
            roomID = _roomID;
            isOwner = _isOwner;
            position = new float[] { 0, 0, 0 };
        }
        /// <summary>
        /// This method is to convert jsonString to UserJSON object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }

    /// <summary>
    /// This class is to store the location of question barrels using a list of waypoint index
    /// </summary>
    [Serializable]
    public class QuestionBarrelJSON
    {
        public List<int> questionBarrelLocations;

        /// <summary>
        /// This is a constructor of QuestionBarrelJSON
        /// </summary>
        /// <param name="_questionBarrelLocations list of waypoint index"></param>
        public QuestionBarrelJSON(List<int> _questionBarrelLocations)
        {
            questionBarrelLocations = new List<int>();
            for (int i = 0;i<_questionBarrelLocations.Count;i++)
            {
                questionBarrelLocations.Add(_questionBarrelLocations[i]);
            }
        }

        /// <summary>
        /// This method is to convert jsonString to QuestionBarrelJSON object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static QuestionBarrelJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<QuestionBarrelJSON>(data);
        }
    }

    /// <summary>
    /// This class is to store the messages of player interactions for the message log
    /// </summary>
    [Serializable]
    public class MessageJSON
    {
        public string message;

        /// <summary>
        /// This method is to convert jsonString to MessageJSON object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MessageJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<MessageJSON>(data);
        }
    }

    /// <summary>
    /// This class is to store any object that has only one integer variable.
    /// Useful for converting objects to jsonString or vice versa for ease of data transfer between client and server
    /// </summary>
    [Serializable]
    public class OneIntVariableJSON
    {
        public int anyIntVariable;

        /// <summary>
        /// This is a constructor of OneIntVariableJSON
        /// </summary>
        /// <param name="_anyIntVariable"></param>
        public OneIntVariableJSON (int _anyIntVariable)
        {
            anyIntVariable = _anyIntVariable;
        }
    }

    /// <summary>
    /// This class is to store the change in score of a player which is acquired when the player has answered a question
    /// </summary>
    [Serializable]
    public class ScoreChangeJSON
    {
        public string playerName;
        public int scoreChange;

        /// <summary>
        /// This method is to convert jsonString to ScoreChangeJSON object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ScoreChangeJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<ScoreChangeJSON>(data);
        }
    }

    #endregion
}
