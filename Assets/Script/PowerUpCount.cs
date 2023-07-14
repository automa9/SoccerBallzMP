using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpCounter : MonoBehaviour
{
    public TextMeshProUGUI countText;

    private MeshTrail meshTrail;

    private void Start()
    {
        meshTrail = FindObjectOfType<MeshTrail>();
    }

    // Update is called once per frame
    void Update()
    {
        int powerUpCount = meshTrail.GetPowerUpCount();
        countText.text = powerUpCount.ToString();
    }
}
