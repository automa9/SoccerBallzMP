using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpCount : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI countText;

    int powerUpCount;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        powerUpCount = FindObjectOfType<MeshTrail>().getPowerUpCount();
        countText.text = powerUpCount.ToString();
    }
}
