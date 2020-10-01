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
    public Canvas canvas;
    public SocketIOComponent socket;
    public InputField playerNameInput;
    public InputField roomNameInput;
    public InputField roomCapacityInput;
    public GameObject player;

    private static string currentRoomName;
    
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
    }

    // Start is called before the first frame update
    void Start()
    {
        //subscribe to all the various websocket events
        socket.On("other player connected", OnOtherPlayerConnected);
        socket.On("play", OnPlay);
        socket.On("player move", OnPlayerMove);
        socket.On("other player disconnected", OnOtherPlayerDisconnected);
        socket.On("owner disconnected", OnOwnerDisconnected);
        socket.On("updateRooms", OnUpdateRooms);
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
        string roomName = roomNameInput.text;
        currentRoomName = roomName;
        int roomCapacity = int.Parse(roomCapacityInput.text);
        string playerName = playerNameInput.text;
        bool isOwner;

        if (roomCapacity == -1)
        {
            isOwner = false;
        }
        else
        {
            isOwner = true;
        }

        List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
        PlayerJSON playerJSON = new PlayerJSON(playerName, isOwner, roomName, roomCapacity, playerSpawnPoints);
        string data = JsonUtility.ToJson(playerJSON);

        if (roomCapacity == -1)
        {
            yield return new WaitForSeconds(0.5f);
            socket.Emit("player connect", new JSONObject(data));
            yield return new WaitForSeconds(1f);
        }

        socket.Emit("play", new JSONObject(data));
        canvas.gameObject.SetActive(false);
    }

    public void CommandMove(Vector3 vec3)
    {
        string data = JsonUtility.ToJson(new PositionJSON(vec3));
        socket.Emit("player move", new JSONObject(data));
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
        if (userJSON.name == playerNameInput.text)
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

    void OnUpdateRooms(SocketIOEvent socketIOEvent)
    {
        print("OnUpdateRooms updated");
        string data = socketIOEvent.data.ToString();
        //TODO data manipulation
    }

    #endregion

    #region JSONMessageClasses

    [Serializable]

    /*public class RoomJSON
    {
        public string roomName;
        public int capacity;
        public PlayerJSON playerJSON;

        public RoomJSON(string _roomName, int _capacity, string _name, bool _isOwner, List<SpawnPoint> _playerSpawnPoints)
        {
            roomName = _roomName;
            capacity = _capacity;
            playerJSON = new PlayerJSON(_name, _isOwner, _playerSpawnPoints);
        }
    }*/
    public class PlayerJSON
    {
        public string name;
        public bool isOwner;
        public string roomName;
        public int capacity;
        public List<PointJSON> playerSpawnPoints;

        public PlayerJSON(string _name, bool _isOwner, string _roomName, int _capacity, List<SpawnPoint> _playerSpawnPoints)
        {
            playerSpawnPoints = new List<PointJSON>();
            name = _name;
            isOwner = _isOwner;
            roomName = _roomName;
            capacity = _capacity;

            foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoints) 
            {
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                playerSpawnPoints.Add(pointJSON);
            }
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
        public string roomName;
        public bool isOwner;
        public float[] position;

        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }


    #endregion
}
