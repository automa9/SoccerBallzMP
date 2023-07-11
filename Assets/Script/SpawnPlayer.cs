using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    public string[] playerName;
    public GameObject player;
    public Vector3[] playerPos; 

    private int playerCount;
    private int teamPosition;

    private void Awake(){
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 15;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        teamPosition = PlayerPrefs.GetInt("previousIndex");

        PlayerPrefs.DeleteKey("previousIndex");

        Debug.Log("Number of player: " + playerCount);
        Debug.Log("Position Selected: " + teamPosition);

        for(int i = 0; i< playerPos.Length; i++)
        {
            if(teamPosition == i)
            {
                playerName[i] = PhotonNetwork.NickName;
                photonView.RPC("Set_OtherPlayerName", RpcTarget.OthersBuffered, i, PhotonNetwork.NickName);

                Quaternion rotationB;
                string color;
                Color nameColor;

                if (i > 2)
                {
                   rotationB = Quaternion.Euler(0f, 180f, 0f);
                    nameColor = Color.red;
                    color = "Red";
                }
                else
                {
                   rotationB =  Quaternion.identity;
                    nameColor = Color.blue;
                    color = "Blue";
                }

                GameObject spawnPlayer = PhotonNetwork.Instantiate(player.name, playerPos[i], rotationB);

                TextMeshProUGUI playername = spawnPlayer.GetComponentInChildren<TextMeshProUGUI>();
                playername.text = playerName[i];
                playername.color = nameColor;

                photonView.RPC("UpdatePlayerName", RpcTarget.OthersBuffered, playerName[i], color);

                break;
            }
        }
    }


    [PunRPC]
    void Set_OtherPlayerName(int index, string name)
    {
        playerName[index] = name;
    }

    [PunRPC]
    void UpdatePlayerName(string name, string color)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            TextMeshProUGUI playerName = player.GetComponentInChildren<TextMeshProUGUI>();
            if(playerName.text == "Name")
            {
                playerName.text = name;
                if(color == "Blue")
                {
                    playerName.color = Color.blue;
                }
                else
                {
                    playerName.color = Color.red;
                }
                
                break;
            }
        }
    }
}
