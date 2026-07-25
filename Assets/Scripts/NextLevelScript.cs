using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelScript : MonoBehaviour
{
    public void GoNextLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
