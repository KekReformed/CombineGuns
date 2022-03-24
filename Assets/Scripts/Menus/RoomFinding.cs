using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomFinding : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject WaitingStatusMenu;
    [SerializeField] private TextMeshProUGUI WaitingStatusText;
    [SerializeField] private GameObject StartButton;

    private bool isConnecting = false;

    private const string GameVersion = "0.1";
    private const int MaxPlayersInRoom = 4;

    private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

    public void FindOpponent()
    {
        isConnecting = true;

        WaitingStatusMenu.SetActive(true);

        WaitingStatusText.text = "Searching...";

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");

        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        WaitingStatusMenu.SetActive(false);

        Debug.Log($"Disconnected due to {cause}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Games dead making new room");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersInRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully");

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerCount != MaxPlayersInRoom)
        {
            WaitingStatusText.text = "Waiting for game to start...";
            Debug.Log("Client waiting for game to start");
        }
        else
        {
            WaitingStatusText.text = "Game starting";
            Debug.Log("Match is ready to begin");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersInRoom)
        {
            StartGame();
        }

        StartButton.SetActive(true);
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        Debug.Log("Closing room, Match is starting");
        WaitingStatusText.text = "Opponent found";

        PhotonNetwork.LoadLevel("Level 1");
    }
}
