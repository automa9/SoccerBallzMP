using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public TextMeshProUGUI[] playerNameA;
    public TextMeshProUGUI[] playerNameB;

    public GameObject room1SettingPanel;
    public GameObject room2SettingPanel;
    public GameObject room3SettingPanel;
    public GameObject waitingPanel;
    public GameObject teamSelectionPanel;

    public Button[] teamAButtons;
    public Button[] teamBButtons;
    public Button startButton;

    RoomOptions options;
    public int maxNumberOfPlayers = 6;
    public int minNumberOfPlayers = 2;

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

    public void JoinRoom(string room)
    {
        room1SettingPanel.SetActive(false);
        room2SettingPanel.SetActive(false);
        room3SettingPanel.SetActive(false);
        teamSelectionPanel.SetActive(true);
        waitingPanel.SetActive(false);

        roomName = room;
    }

    // Team A button click event handlers
    public void OnButton1ClickA()
    {
        ToggleButton(teamAButtons, 0);
        teamSelected = "Lightning Strikes";
    }

    public void OnButton2ClickA()
    {
        ToggleButton(teamAButtons, 1);
        teamSelected = "Lightning Strikes";
    }

    public void OnButton3ClickA()
    {
        ToggleButton(teamAButtons, 2);
        teamSelected = "Lightning Strikes";
    }

    // Team B button click event handlers
    public void OnButton1ClickB()
    {
        ToggleButton(teamBButtons, 0);
        teamSelected = "Mighty Warriors";
    }

    public void OnButton2ClickB()
    {
        ToggleButton(teamBButtons, 1);
        teamSelected = "Mighty Warriors";
    }

    public void OnButton3ClickB()
    {
        ToggleButton(teamBButtons, 2);
        teamSelected = "Mighty Warriors";
    }

    // Toggle button selection for the specified team
    private void ToggleButton(Button[] buttons, int selectedIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = (i == selectedIndex);
        }
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
            UpdatePlayerNameUI(playerNameA, 0);
            photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, 0, PhotonNetwork.NickName);
        }
        else
        {
            UpdatePlayerNameUI(playerNameB, 0);
            photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, 1, PhotonNetwork.NickName);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Handle player leaving the room
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // Handle player property updates
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // Handle master client switch
    }

    [PunRPC]
    void SendPlayerName(int index, string name)
    {
        if (index == 0)
        {
            UpdatePlayerNameUI(playerNameA, 1, name);
        }
        else
        {
            UpdatePlayerNameUI(playerNameB, 1, name);
        }
    }

    private void UpdatePlayerNameUI(TextMeshProUGUI[] playerNameUI, int index, string playerName = "")
    {
        if (index >= 0 && index < playerNameUI.Length)
        {
            playerNameUI[index].text = playerName;
        }
    }
}
