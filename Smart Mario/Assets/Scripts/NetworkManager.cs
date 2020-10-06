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
        socket.On("play", OnPlay);
        socket.On("player move", OnPlayerMove);
        socket.On("other player disconnected", OnOtherPlayerDisconnected);
        socket.On("owner disconnected", OnOwnerDisconnected);
        socket.On("get rooms", OnGetRooms);
        socket.On("room is full", OnRoomIsFull);
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
        StartCoroutine(ConnectToServer());
    }

    #region Commands
    IEnumerator ConnectToServer()
    {
        scene.ToMultiplayerRoom();

        string roomName = PlayerPrefs.GetString("roomName", "room1");
        int roomCapacity = PlayerPrefs.GetInt("roomCapacity", 4);
        string playerName = PlayerPrefs.GetString("username", "fazli");
        string roomID = PlayerPrefs.GetString("roomID", "create");
        string minigameSelected = PlayerPrefs.GetString("Minigame Selected", "World 2 Stranded");
        string difficultySelected = PlayerPrefs.GetString("Difficulty Selected", "Easy");

        bool isOwner;

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

    public void CommandLeaveRoom()
    {
        scene.ToMultiplayerLobby();
        socket.Emit("disconnect");
    }

    #endregion

    #region Listening

    void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
    {
        print("Someone else joined");
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        GameObject o = GameObject.Find(userJSON.name) as GameObject;
        if (o != null)
        {
            return;
        }
        GameObject p = Instantiate(player, position, Quaternion.identity) as GameObject;
        PlayerMovement pm = p.GetComponent<PlayerMovement>();
        Transform t = p.transform.Find("Player Name Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = userJSON.name;
        pm.isLocalPlayer = false;
        pm.isOwner = userJSON.isOwner;
        pm.multiplayer = true;
        p.name = userJSON.name;
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
        if (userJSON.name == PlayerPrefs.GetString("username", "fazli"))
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
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
        PlayerMovement pm = p.GetComponent<PlayerMovement>();
        pm.isOwner = true;
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

        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }


    #endregion
}
