using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJSON;
using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    private SceneController scene;
    public SocketIOComponent socket;
    public GameObject player;
    public GameObject playerMinigame;

    private static string roomName;
    private static int roomCapacity;
    public static string playerName;
    private static string roomID;
    private static string minigameSelected;
    private static string difficultySelected;
    public static bool isOwner;
    public static UserJSON storedUserJSON;

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
        if (socket )
        DontDestroyOnLoad(socket);
        DontDestroyOnLoad(player);
    }

    // Start is called before the first frame update
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
        socket.On("minigame start", OnMinigameStart);
        socket.On("next player", OnNextPlayer);
        socket.On("update message", OnUpdateMessage);
        socket.On("score change", OnScoreChange);
        socket.On("one player left", OnOnePlayerLeft);
        socket.On("end game", OnEndGame);
        socket.On("player left minigame", OnPlayerLeftMinigame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void createGame()
    {

    }
    public void JoinRoom()
    {
        StartCoroutine(ConnectToRoom());
    }

    public void StartChallenge()
    {
        scene.ToWorld1StrandedMultiplayer();
    }

    #region Commands
    IEnumerator ConnectToRoom()
    {
        scene.ToMultiplayerRoom();

        roomName = PlayerPrefs.GetString("roomName", "room1");
        roomCapacity = PlayerPrefs.GetInt("roomCapacity", 4);
        playerName = PlayerPrefs.GetString("username", "fazli");
        roomID = PlayerPrefs.GetString("roomID", "create");
        minigameSelected = PlayerPrefs.GetString("Minigame Selected", "World 2 Stranded");
        difficultySelected = PlayerPrefs.GetString("Minigame Difficulty", "Easy");

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
        List<SpawnPoint> playerSpawnPoints = GameObject.Find("RoomManager").GetComponent<PlayerSpawner>().playerSpawnPoints;
        storedUserJSON = new UserJSON(playerName, roomID, isOwner);
        PlayerJSON playerJSON = new PlayerJSON(playerName, roomID, isOwner, roomName, roomCapacity, minigameSelected, difficultySelected, playerSpawnPoints);
        string data = JsonUtility.ToJson(playerJSON);
        Debug.Log(data);
        socket.Emit("player connect", new JSONObject(data));
        yield return new WaitForSeconds(0.1f);
        socket.Emit("play", new JSONObject(data));
    }

    public void CommandMove(Vector3 vec3)
    {
        string data = JsonUtility.ToJson(new PositionJSON(vec3));
        socket.Emit("player move", new JSONObject(data));
    }

    public void CommandGetRooms()
    {
        socket.Emit("get rooms");
    }

    public void CommandLeaveChallenge()
    {
        scene.ToMultiplayerLobby();
        socket.Emit("disconnect");
    }

    public void CommandMinigameStart(List<int> questionBarrelLocations)
    {
        print("minigame start command");
        StartCoroutine(MinigameStart(questionBarrelLocations));
    }

    IEnumerator MinigameStart(List<int> questionBarrelLocations)
    {
        Debug.Log("Emit minigame connect");
        string data = JsonUtility.ToJson(storedUserJSON);
        print("emit minigame connect");
        socket.Emit("minigame connect", new JSONObject(data));
        yield return new WaitForSeconds(0.1f);
        string questionBarrelData = JsonUtility.ToJson(new QuestionBarrelJSON(questionBarrelLocations));
        socket.Emit("minigame start", new JSONObject(questionBarrelData));
    }

    public void CommandEndTurn()
    {
        print("end turn");
        string data = JsonUtility.ToJson(storedUserJSON);
        socket.Emit("end turn", new JSONObject(data));
    }

    public void CommandRollDice(int diceNumber)
    {
        print("roll dice");
        string data = JsonUtility.ToJson(new OneIntVariableJSON(diceNumber));
        socket.Emit("roll dice", new JSONObject(data));
    }

    public void CommandAnsweringQuestion()
    {
        print("answering question");
        socket.Emit("answer question");
    }

    public void CommandQnResult(int scoreChange)
    {
        print("qn result");
        string data = JsonUtility.ToJson(new OneIntVariableJSON(scoreChange));
        socket.Emit("qn result", new JSONObject(data));
    }

    public void CommandEndGame()
    {
        print("end game");
        socket.Emit("end game");
    }

    #endregion

    #region Listening

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
    void OnMinigameStart(SocketIOEvent socketIOEvent)
    {
        print("minigame start");
        string data = socketIOEvent.data.ToString();
        QuestionBarrelJSON questionBarrelJSON = QuestionBarrelJSON.CreateFromJSON(data);
        StartCoroutine(LoadMinigame(questionBarrelJSON.questionBarrelLocations));
    }

    IEnumerator LoadMinigame(List<int> questionBarrelLocations)
    {
        scene.ToWorld1StrandedMultiplayer();

        while (GameObject.Find("StrandedMultiplayerGameManager") == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StrandedMultiplayerGameManager.instance.SpawnQuestionBarrelsMultiplayer(questionBarrelLocations);
        string data = JsonUtility.ToJson(storedUserJSON);
        print("emit minigame connect");
        socket.Emit("minigame connect", new JSONObject(data));
    }
    
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
        GameObject p = Instantiate(player, position, Quaternion.identity);
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
        GameObject p = Instantiate(playerMinigame, StrandedMultiplayerGameManager.instance.GetStartWayPoint().transform.position, Quaternion.identity) as GameObject;
        FollowThePath pm = p.GetComponent<FollowThePath>();
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

    IEnumerator AddPlayerToGameStatus(string playerName)
    {
        StrandedMultiplayerGameStatus.instance.AddPlayer(playerName);
        yield return new WaitForSeconds(0.1f);
        StrandedMultiplayerGameStatus.instance.DisplayOtherPlayerScore(playerName);
    }

    void OnUpdateMessage(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        MessageJSON currentMessage = MessageJSON.CreateFromJSON(data);
        StartCoroutine(UpdateMessage(currentMessage.message));
    }

    IEnumerator UpdateMessage(string message)
    {
        ChallengeManager.instance.SendToMessageLog(message);
        yield return new WaitForSeconds(0.1f);
    }

    void OnScoreChange(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        ScoreChangeJSON scoreChange = ScoreChangeJSON.CreateFromJSON(data);
        StartCoroutine(ScoreChange(scoreChange));
    }

    IEnumerator ScoreChange(ScoreChangeJSON scoreChange)
    {
        StrandedMultiplayerGameStatus.instance.OtherPlayerScoreChange(scoreChange.playerName, scoreChange.scoreChange);
        yield return new WaitForSeconds(0.1f);
    }

    void OnEndGame(SocketIOEvent socketIOEvent)
    {
        print("end game");
        StrandedMultiplayerGameManager.instance.GameComplete();

    }

    void OnPlayerLeftMinigame(SocketIOEvent socketIOEvent)
    {
        print("Player left Minigame");
        string data = socketIOEvent.data.ToString();
        UserJSON UserLeftJSON = UserJSON.CreateFromJSON(data);
        StrandedMultiplayerGameStatus.instance.PlayerLeft(UserLeftJSON.name);
    }

    void OnPlay(SocketIOEvent socketIOEvent)
    {
        print("You joined");
        string data = socketIOEvent.data.ToString();
        UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
        GameObject p = Instantiate(player, position, Quaternion.identity) as GameObject;
        PlayerMovement pm = p.GetComponent<PlayerMovement>();
        Transform t = p.transform.Find("Player Name Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = currentUserJSON.name;
        pm.isLocalPlayer = true;
        pm.isOwner = currentUserJSON.isOwner;
        pm.multiplayer = true;
        p.name = currentUserJSON.name;
    }

    void OnPlayerMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        // if it is the current player, return
        if (userJSON.name == storedUserJSON.name)
        {
            Debug.LogError("inside player move same name");
            return;
        }
        Debug.LogError("inside player escape same name");
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            Debug.LogError("inside player move diff name");
            p.transform.position = position;
        }
    }

    void OnOtherPlayerDisconnected(SocketIOEvent socketIOEvent)
    {
        print("user disconnected");
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        print(userJSON.name + " disconnected");
        Destroy(GameObject.Find(userJSON.name));
    }

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
            FollowThePath pm = p.GetComponent<FollowThePath>();
            pm.isOwner = true;
            if (pm.isLocalPlayer)
                isOwner = true;
        }
            
    }

    void OnOnePlayerLeft(SocketIOEvent socketIOEvent)
    {
        print("one player left");
        StartCoroutine(OnePlayerLeft());
    }

    IEnumerator OnePlayerLeft()
    {
        scene.ToMultiplayerLobby();
        socket.Emit("disconnect");
        while (GameObject.Find("LobbyManager") == null)
            yield return new WaitForSeconds(0.1f);
        LobbyManager.instance.GetComponent<LobbyManager>().NotEnoughPlayers();
    }

    void OnGetRooms(SocketIOEvent socketIOEvent)
    {
        print("Rooms retrieved");
        string data = socketIOEvent.data.ToString();
        Debug.Log(data);
        JObject roomJSONData = (JObject)JsonConvert.DeserializeObject(data);

        LobbyManager.instance.GetComponent<LobbyManager>().AddRooms(roomJSONData);
        //TODO data manipulation
    }

    void OnRoomIsFull(SocketIOEvent socketIOEvent)
    {
        print("room is full");
        StartCoroutine(LoadMultiplayerLobby());
    }

    IEnumerator LoadMultiplayerLobby()
    {
        scene.ToMultiplayerLobby();
        while (GameObject.Find("LobbyManager") == null)
            yield return new WaitForSeconds(0.1f);
        LobbyManager.instance.GetComponent<LobbyManager>().RoomIsFull();
    }

    #endregion

    #region JSONMessageClasses

    [Serializable]
    public class PlayerJSON
    {
        public string name;
        public string roomID;
        public bool isOwner;
        public string roomName;
        public int capacity;
        public string minigameSelected;
        public string difficultySelected;
        public List<PointJSON> playerSpawnPoints;

        public PlayerJSON(string _name, string _roomID, bool _isOwner, string _roomName, 
            int _capacity, string _minigameSelected, string _difficultySelected, List<SpawnPoint> _playerSpawnPoints)
        {
            playerSpawnPoints = new List<PointJSON>();
            name = _name;
            roomID = _roomID;
            isOwner = _isOwner;
            roomName = _roomName;
            capacity = _capacity;
            minigameSelected = _minigameSelected;
            difficultySelected = _difficultySelected;

            foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoints) 
            {
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                Debug.Log(pointJSON.position[0]);
                playerSpawnPoints.Add(pointJSON);
            }
        }
    }

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

        public static RoomJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<RoomJSON>(data);
        }
    }

    [Serializable]
    public class PointJSON
    {
        public float[] position;
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

    [Serializable]
    public class PositionJSON
    {
        public float[] position;

        public PositionJSON(Vector3 _position)
        {
            position = new float[] { _position.x, _position.y, _position.z };
        }
    }

    [Serializable]
    public class UserJSON // notify that another player joins the game 
    {
        public string name;
        public string roomID;
        public bool isOwner;
        public float[] position;

        public UserJSON(string _name, string _roomID, bool _isOwner)
        {
            name = _name;
            roomID = _roomID;
            isOwner = _isOwner;
            position = new float[] { 0, 0, 0 };
        }
        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }

    [Serializable]
    public class QuestionBarrelJSON
    {
        public List<int> questionBarrelLocations;

        public QuestionBarrelJSON(List<int> _questionBarrelLocations)
        {
            questionBarrelLocations = new List<int>();
            for (int i = 0;i<_questionBarrelLocations.Count;i++)
            {
                questionBarrelLocations.Add(_questionBarrelLocations[i]);
            }
        }
        
        public static QuestionBarrelJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<QuestionBarrelJSON>(data);
        }
    }

    [Serializable]
    public class MessageJSON
    {
        public string message;

        public static MessageJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<MessageJSON>(data);
        }
    }

    [Serializable]
    public class OneIntVariableJSON
    {
        public int anyIntVariable;

        public OneIntVariableJSON (int _anyIntVariable)
        {
            anyIntVariable = _anyIntVariable;
        }
    }

    [Serializable]
    public class ScoreChangeJSON
    {
        public string playerName;
        public int scoreChange;

        public static ScoreChangeJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<ScoreChangeJSON>(data);
        }
    }

    #endregion
}
