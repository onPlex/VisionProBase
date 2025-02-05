using UnityEngine;
using TMPro;

namespace YJH.MajorHunting
{
    /// <summary>
    /// 각 버튼(오브젝트)에 부착되는 스크립트.
    /// 클릭 시 MainShootingGame에 "missionName"을 전달하여
    /// 정답/오답 여부를 판정받고, 해당 효과 함수를 호출한다.
    /// </summary>
    public class ShootingTarget : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private TMP_Text missionText; // 버튼에 표시될 텍스트
        [Space]
        [SerializeField]
        private string missionName;
        // 이 버튼이 정답으로 연결되는 "미션 이름" 
        // 예: "Robot Attack", "Bio Hazard" 등

        [SerializeField]
        private MainShootingGame mainShootingGame;
        // Inspector에서 MainShootingGame을 연결 (씬 내 객체)

        [Header("AnswerEffect")]
        [SerializeField]
        GameObject NomalBoard;
        [SerializeField]
        GameObject GreenBoard;
        [SerializeField]
        GameObject RedBoard;

        /// <summary>
        /// 버튼이 클릭되면 이 함수가 실행된다고 가정 (OnClick 등 이벤트 연결)
        /// </summary>
        public void OnTargetClicked()
        {
            if (mainShootingGame != null)
            {
                mainShootingGame.OnClickMissionButton(missionName, this);
            }
        }

        /// <summary>
        /// 정답일 때 재생할 효과 (사운드, 파티클 등)
        /// </summary>
        public void PlayCorrectEffect()
        {
            Debug.Log($"[ShootingTarget: {name}] 정답 효과 재생!");
            // TODO: 사운드, 파티클, 애니메이션, UI 표시 등
            NomalBoard.SetActive(false);
            GreenBoard.SetActive(true);
        }

        /// <summary>
        /// 오답일 때 재생할 효과
        /// </summary>
        public void PlayWrongEffect()
        {
            Debug.Log($"[ShootingTarget: {name}] 오답 효과 재생!");
            // TODO: 사운드, 파티클, 애니메이션, UI 표시 등
            NomalBoard.SetActive(false);
            RedBoard.SetActive(true);
        }

         public void ResetEffect()
        {
            // 초기 상태(노멀 보드 활성)
            if (NomalBoard != null)  NomalBoard.SetActive(true);
            if (GreenBoard != null)  GreenBoard.SetActive(false);
            if (RedBoard != null)    RedBoard.SetActive(false);
        }

        public void SetMissionName(string name)
        {
            missionName = name;
            if (missionText != null)
                missionText.text = name;
        }
    }
}
