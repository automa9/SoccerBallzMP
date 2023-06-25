using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public TextMeshProUGUI[] playerName;

    public GameObject room1SettingPanel;
    public GameObject room2SettingPanel;
    public GameObject room3SettingPanel;
    public GameObject waitingPanel;
    public GameObject teamSelectionPanel;

    public Button teamA;
    public Button teamB;
    public Button startButton;

    RoomOptions options;
    public int maxNumberOfPlayers = 2;
    public int minNumberOfPlayers = 1;

    //public string sceneMP;

    private string level1RoomID = "Level1Room";
    private string level2RoomID = "Level2Room";
    private string level3RoomID = "Level3Room";

    private string teamSelected;
    private string roomName = string.Empty;

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

/*    public void JoinRoom1()
    {
        roomName = level1RoomID;
        JoinRoom();
    }

    public void JoinRoom2()
    {
        roomName = level2RoomID;
        JoinRoom();
    }

    public void JoinRoom3()
    {
        roomName = level3RoomID;
        JoinRoom();
    }
*/
    public void JoinRoom(string room)
    {
        room1SettingPanel.SetActive(false);
        room2SettingPanel.SetActive(false);
        room3SettingPanel.SetActive(false);
        teamSelectionPanel.SetActive(true);
        waitingPanel.SetActive(false);

        roomName = room;
    }

    // team selection function
    public void ToggleButton(Button activeButton, Button inactiveButton)
    {
        activeButton.interactable = false;
        inactiveButton.interactable = true;
    }

    public void OnButton1Click()
    {
        ToggleButton(teamA, teamB);
        teamSelected = "Lightning Strikes";
    }

    public void OnButton2Click()
    {
        ToggleButton(teamB, teamA);
        teamSelected = "Mighty Warriors";
    }

    public void SelectTeam()
   {
        room1SettingPanel.SetActive(false);
        room2SettingPanel.SetActive(false);
        room3SettingPanel.SetActive(false);
        waitingPanel.SetActive(true);
        teamSelectionPanel.SetActive(false);

        PlayerPrefs.SetString("Team", teamSelected);
        string namePlayer = PlayerPrefs.GetString("username");
        PhotonNetwork.NickName = namePlayer;
        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    public void LoadScene()
    {
        // Load the scene based on the room ID
        if (roomName == level1RoomID)
        {
            PhotonNetwork.LoadLevel("MultiLevel");
        }
        else if (roomName == level2RoomID)
        {
            PhotonNetwork.LoadLevel("Level2");
        }
        else if (roomName == level3RoomID)
        {
            PhotonNetwork.LoadLevel("Level3SceneName");
        }
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
            if (!teamA.interactable)
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
        else
        {
            if (!teamA.interactable)
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
    void Send_PlayersName(int index, string name)
    {
        playerName[index].text = name;
    }
}
