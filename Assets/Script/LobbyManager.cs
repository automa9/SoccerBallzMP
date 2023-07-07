using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LobbyManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public TextMeshProUGUI[] playerName;
    public Button[] selectButton;

    public GameObject room1SettingPanel;
    public GameObject room2SettingPanel;
    public GameObject room3SettingPanel;
    public GameObject waitingPanel;

    public Button startButton;

    RoomOptions options;
    public int maxNumberOfPlayers = 6;
    public int minNumberOfPlayers = 2;

    private string level1RoomID = "Level1Room";
    private string level2RoomID = "Level2Room";
    private string level3RoomID = "Level3Room";

    private string roomName = string.Empty;
    private string namePlayer;

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
        if (PhotonNetwork.IsMasterClient && CheckPlayersTeamSelected())
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
        waitingPanel.SetActive(true);
        roomName = room;

        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    public bool CheckPlayersTeamSelected()
    {
        int count = 0;
        // Use to check if all players have selected the team
        for(int i = 0; i < playerName.Length; i++)
        {
            if(playerName[i].text != "Waiting for player...")
            {
                count++;
            }
        }

        if(count >= minNumberOfPlayers)
        {
            return true;
        }

        return false;
    }

    public void onClickToSelectTeam(int buttonIndex)
    {
        namePlayer = PlayerPrefs.GetString("username");
        PhotonNetwork.NickName = namePlayer;

        if (PhotonNetwork.IsMasterClient)
        {
            UpdatePlayerName(buttonIndex, namePlayer + " [Master]");
            photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, buttonIndex, namePlayer + " [Master]");
        }
        else
        {
            UpdatePlayerName(buttonIndex, namePlayer);
            photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, buttonIndex, namePlayer);
        }

        // Previous Selected Place
        if (PlayerPrefs.HasKey("previousIndex"))
        {
            int previousIndex = PlayerPrefs.GetInt("previousIndex");

            UpdatePlayerName(previousIndex, "Waiting for player...");
            photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, previousIndex, "Waiting for player...");
        }

        // Team Select Updated
        if(buttonIndex == 0 || buttonIndex == 1 || buttonIndex == 2)
        {
            PlayerPrefs.SetString("teamSelected", "Lightning Strikes");
        }
        else
        {
            PlayerPrefs.SetString("teamSelected", "Mighty Warriors");
        }

        PlayerPrefs.SetInt("previousIndex", buttonIndex);
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

    public override void OnConnectedToMaster() 
    {
        // callback function for when first connection is made
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
       
    }

    public void ClickToLeffRoom()
    {
        PhotonNetwork.LeaveRoom();


        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < playerName.Length; i++)
            {
                if (playerName[i].text.Contains("[Master]"))
                {
                    UpdatePlayerName(i, "Waiting for player...");
                    photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, i, "Waiting for player...");
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < playerName.Length; i++)
            {
                if (playerName[i].text == namePlayer)
                {
                    UpdatePlayerName(i, "Waiting for player...");
                    photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, i, "Waiting for player...");
                    break;
                }
            }
        }

        room1SettingPanel.SetActive(true);
        room2SettingPanel.SetActive(true);
        room3SettingPanel.SetActive(true);
        waitingPanel.SetActive(false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for(int i = 0; i < playerName.Length; i++)
        {
            if(playerName[i].text == otherPlayer.NickName)
            {
                SendPlayerName(i, "Waiting for player..."); 
                break;
            }
        }  
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // Handle player property updates
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // Handle master client switch
        for (int i = 0; i < playerName.Length; i++)
        {
            if (playerName[i].text != newMasterClient.NickName && playerName[i].text.Contains("[Master]"))
            {
                SendPlayerName(i, "Waiting for player...");              
            }
            else if(newMasterClient.NickName == playerName[i].text)
            {
                string name = playerName[i].text + " [Master]";
                SendPlayerName(i, name);
               
            }
        }
    }

    void UpdatePlayerName(int index, string name)
    {
        playerName[index].text = name;

        if (name == "Waiting for player...")
        {
            selectButton[index].interactable = true;
        }
        else
        {
            selectButton[index].interactable = false;
        }

    }


    [PunRPC]
    void SendPlayerName(int index, string name)
    {
        playerName[index].text = name;

        if (name == "Waiting for player...")
        {
            selectButton[index].interactable = true;
        }
        else
        {
            selectButton[index].interactable = false;
        }    
    }

}
