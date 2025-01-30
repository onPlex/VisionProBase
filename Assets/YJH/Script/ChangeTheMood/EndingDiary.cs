using UnityEngine;
using UnityEngine.SceneManagement;
namespace YJH.ChangeTheMood
{
    public class EndingDiary : MonoBehaviour
    {
        [SerializeField]
        Result result;
        [SerializeField]
        GameObject HappyEndObj;

        [SerializeField]
        GameObject UnHappyEndObj;


        private void OnEnable()
        {
            CheckHappyEnd();
        }

        private void CheckHappyEnd()
        {
            if (result.isHappyEnd)
            {
                HappyEndObj.SetActive(true);
            }
            else
            {
                UnHappyEndObj.SetActive(true);

            }
        }

        public void OnClickRestartCurrentScene()
        {

            // 현재 활성화된 씬 정보를 가져와서, 그 씬의 인덱스(혹은 이름)로 다시 로드
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        public void OnClickQuitApp()
        {

            Application.Quit();
        }
    }
}