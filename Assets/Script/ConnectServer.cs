using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectServer : MonoBehaviourPunCallbacks
{
    public string lobbySceneName;
    public GameObject loadingPanel; //Loading Picture panel
    public GameObject loadingBar; // Reference to the loading bar or sprite
    private LoadingBarController loadingBarController;

    // Start is called before the first frame update
    void Start()
    {
        loadingBar.SetActive(false); // Initially hide the loading bar or sprite
        loadingBarController = loadingBar.GetComponent<LoadingBarController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConnectMultiplayer()
    {
        loadingBar.SetActive(true); // Show the loading bar or sprite when connecting
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        loadingBarController.UpdateLoadingBar(0.5f); // Update the loading bar progress
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {   
        loadingPanel.SetActive(true);
        loadingBarController.UpdateLoadingBar(1f); // Update the loading bar progress
        SceneManager.LoadScene(lobbySceneName);
    }
}
