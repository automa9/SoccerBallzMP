using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class DashboardRank : MonoBehaviour
{
    public APISystem api;
    public Transform content;
    public ScrollRect scrollRect;
    public GameObject prefab;
    public GameObject playerRankPos;

    #region Monobehaviour Callback

    void Start()
    {
        if (prefab)
        {
            ListTopPlayer();
        }
    }

    #endregion

    #region Public Method

    public void ListTopPlayer()
    {
        StartCoroutine(IEList());
    }

    #endregion

    #region IEnumerator Method

    private IEnumerator IEList()
    {
        List<GameObject> tempHold = new List<GameObject>();

        api.GetLeaderboard();

        yield return new WaitUntil(() => api.containerB.status == "1");

        for (int i = 0; i < api.containerB.message.data.Count; i++)
        {
            tempHold.Add(Instantiate(prefab, content));
            tempHold[i].SetActive(false);
            tempHold[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            tempHold[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = api.containerB.message.data[i].alias;

            api.GetPlayer(api.containerB.message.data[i].alias);

            yield return new WaitUntil(() => api.containerA.status == "1");

            tempHold[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = api.containerA.message.score[0].value;

            api.containerA.status = "0";
        }

        for (int i = 0; i < api.containerB.message.data.Count; i++)
        {
            int highestScore = 0;
            for(int j = i; j < api.containerB.message.data.Count; j++)
            {
                int score = int.Parse(tempHold[j].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text);

                if (score > highestScore)
                {
                    highestScore = score;
                    string holdName;
                    string holdScore;
                    holdName = tempHold[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
                    tempHold[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tempHold[j].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
                    tempHold[j].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = holdName;
                    holdScore = tempHold[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
                    tempHold[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = tempHold[j].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
                    tempHold[j].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = holdScore;
                }
            }
            tempHold[i].SetActive(true);

            if(tempHold[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text == PlayerPrefs.GetString("username"))// && i > 2)
            {
                playerRankPos.SetActive(false);
                playerRankPos.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                playerRankPos.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tempHold[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
                playerRankPos.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = tempHold[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
                playerRankPos.SetActive(true);
            }
        }

    }

    #endregion
}
