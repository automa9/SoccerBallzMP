using UnityEngine;
using UnityEngine.UI;

public class LoadingBarController : MonoBehaviour
{
    public Image loadingBarImage;

    void Start()
    {
        loadingBarImage.fillAmount = 0f; 
    }

    public void UpdateLoadingBar(float progress)
    {
        loadingBarImage.fillAmount = progress;
    }
}
