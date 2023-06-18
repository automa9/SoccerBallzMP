using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerData : MonoBehaviour
{
    public APISystem api;
    public TMP_InputField username;

    void Start()
    {

    }

    public void submitData(string sceneName)
    {
        if(string.IsNullOrEmpty(username.text))
        {
            Debug.Log("Enter your name");
        }
        else
        {
            PlayerPrefs.SetString("username", username.text);
            string name = username.text;
            FindObjectOfType<APISystem>().Register(name, name, name, name); //change the value if your alias, fname, lname and id are different
            SceneManager.LoadScene(sceneName);
        }
    }
}
