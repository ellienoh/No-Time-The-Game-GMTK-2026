using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelScript : MonoBehaviour
{
    private void Update()
    {
        
    }

    public void GoNextLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
