using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jun
{
    public class SceneController : MonoBehaviour
    {
        // [SerializeField] private string _sceneName;

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}