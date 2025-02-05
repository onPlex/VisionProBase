using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void LoadScene(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            // 현재 씬의 인덱스를 가져오고 다음 씬을 로드
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings; // 씬 인덱스가 초과되지 않도록 순환
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // 입력받은 이름의 씬을 로드
            SceneManager.LoadScene(name);
            
        }
    }
}
