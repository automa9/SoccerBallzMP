using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : MonoBehaviour
{
    public string sceneName;

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
