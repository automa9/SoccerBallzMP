using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public TextMeshProUGUI[] playerName;

    public TMP_InputField roomName;
    //public Text roomName;

    public GameObject roomSettingPanel;
    public GameObject waitingPanel;

    public Button startButton;

    RoomOptions options;
    public int maxNumberOfPlayers = 2;
    public int minNumberOfPlayers = 1;

    public string sceneMP;

    void Start()
    {
        options = new RoomOptions
        {
            MaxPlayers = (byte)maxNumberOfPlayers,
            IsOpen = true,
            IsVisible = true
        };
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == maxNumberOfPlayers)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(roomName.text))
        {
            roomSettingPanel.SetActive(false);
            waitingPanel.SetActive(true);

            string namePlayer = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = namePlayer;
            PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
        }
    }

    public void LoadScene()
    {
        PhotonNetwork.LoadLevel(sceneMP);
    }

    public override void OnConnectedToMaster() // callback function for when first connection is made
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (PhotonNetwork.IsMasterClient)
        {
            playerName[0].text = PhotonNetwork.NickName;
            photonView.RPC("Send_PlayersName", RpcTarget.OthersBuffered, 0, PhotonNetwork.NickName);
        }
        else
        {
            playerName[1].text = PhotonNetwork.NickName;
            photonView.RPC("Send_PlayersName", RpcTarget.OthersBuffered, 1, PhotonNetwork.NickName);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        throw new System.NotImplementedException();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        throw new System.NotImplementedException();
    }

    [PunRPC]
    void Send_PlayersName(int index,string name)
    {
        playerName[index].text = name;
    }
}
