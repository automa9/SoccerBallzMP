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

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerName[0] = PhotonNetwork.NickName;
            photonView.RPC("Set_OtherPlayerName", RpcTarget.OthersBuffered, 0, PhotonNetwork.NickName);
            PhotonNetwork.Instantiate(player.name, playerPos[0], Quaternion.identity);
            Debug.Log("You are the Master");
            //Transform canvas = player.transform.Find("Canvas");
            //Transform showName = canvas.Find("Name");
            //showName.GetComponent<TextMeshProUGUI>().text = playerName[0];
        }
        else
        {
            playerName[1]= PhotonNetwork.NickName;
            photonView.RPC("Set_OtherPlayerName", RpcTarget.OthersBuffered, 1, PhotonNetwork.NickName);
            Quaternion rotationB = Quaternion.Euler(0f, 180f, 0f);
            PhotonNetwork.Instantiate(player.name, playerPos[1], rotationB);
            Debug.Log("Youre client");
            //Transform canvas = player.transform.Find("Canvas");
            //Transform showName = canvas.Find("Name");
            //showName.GetComponent<TextMeshProUGUI>().text = playerName[1];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    void Set_OtherPlayerName(int index, string name)
    {
        playerName[index] = name;
    }
}
