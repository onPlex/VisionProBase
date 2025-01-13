using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        SceneManager.LoadSceneAsync(SceneName);
    }
}
