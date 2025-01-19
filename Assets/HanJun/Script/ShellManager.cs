using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

namespace Jun
{
    public enum CareerType
    {
        Realistic,    // 현실형
        Investigative, // 탐구형
        Artistic,     // 예술형
        Social,       // 사회형
        Enterprising, // 기업형
        Conventional  // 관습형
    }

    public class ShellManager : MonoBehaviour
    {
        public GameObject dolphin;
        public TMP_Text _countText;
        public GameObject shellCountUI;

        public GameObject[] objects; // 6개의 오브젝트 (고정)

        public TextMeshPro[] texts;
        public TextMeshPro[] desces;
        public ShellBehavior[] _shellObjects;

        private int currentStartIndex = 0; // 현재 텍스트 시작 인덱스
        private const int TEXT_COUNT_PER_CYCLE = 6; // 한 번에 보여줄 텍스트 데이터 개수

        private int[] _buttonExecutionCounts; // 버튼별 실행 횟수 저장
        private int _mostClickedButton = -1;  // 가장 많이 눌린 버튼 번호

        private int _SelectShellNumber = 0;

        private CareerType careerType;
        public CareerType CareerType { get => careerType; set => careerType = value; }

        void Start()
        {
            if (objects.Length != TEXT_COUNT_PER_CYCLE)
            {
                Debug.LogError("오브젝트 수는 반드시 6개여야 합니다.");
                return;
            }

            if (titleTextData.Length != TEXT_COUNT_PER_CYCLE * TEXT_COUNT_PER_CYCLE)
            {
                Debug.LogError("텍스트 데이터는 반드시 36개여야 합니다.");
                return;
            }

            _buttonExecutionCounts = new int[6];

            UpdateTextData(); // 초기 텍스트 설정
        }


        /// <summary>
        /// 조개 선택시 이벤트
        /// </summary>
        /// <param name="index"></param> <summary>
        public void SelectShellEvent(int index)
        {
            ResetShellButtonEvent();

            _shellObjects[index].SetAnimationEvent(false, () =>
            {
                MoveToBasket(index);
                UpdateMostClickedButton(index);// 가장 많이 눌린 버튼 업데이트
            });
        }

        private void UpdateMostClickedButton(int index)
        {
            _buttonExecutionCounts[index]++; // 실행 횟수 증가

            // 가장 많이 눌린 버튼의 번호를 업데이트
            int maxCount = _buttonExecutionCounts.Max();
            _mostClickedButton = System.Array.IndexOf(_buttonExecutionCounts, maxCount);

            // _mostClickedButton 값을 CareerType 열거형으로 매핑
            if (_mostClickedButton >= 0 && _mostClickedButton < System.Enum.GetValues(typeof(CareerType)).Length)
            {
                CareerType = (CareerType)_mostClickedButton;
            }
            else
            {
                Debug.LogWarning("유효하지 않은 CareerType 매핑");
            }

            Debug.Log($"가장 많이 눌린 버튼: {_mostClickedButton} (누름 횟수: {_buttonExecutionCounts[_mostClickedButton]})");
        }

        /// <summary>
        /// 조개 아래 UI버튼 이벤트
        /// </summary>
        /// <param name="index"></param>
        bool _isToggleDescButtonEvent = false;
        public void SelectButtonEvent(int index)
        {
            if (index < 0 || index >= _shellObjects.Length)
            {
                Debug.LogWarning("유효하지 않은 인덱스입니다.");
                return;
            }

            _isToggleDescButtonEvent = !_isToggleDescButtonEvent;

            if (_isToggleDescButtonEvent)
            {
                ResetShellButtonEvent();
                _shellObjects[index].SetAnimationEvent(true, () => CompleteEvent(index));
            }
            else
            {
                ResetShellButtonEvent();
            }
        }

        /// <summary>
        /// 바구니로 조개 이동 연출 구현
        /// </summary>
        /// <param name="index"></param>
        private void MoveToBasket(int index)
        {
            _shellObjects[index].EnabeldTitleObject(false);
            _shellObjects[index].GetComponent<ShellThrow>().ThrowToTarget(() =>
            {
                // 조개 선택시 바구니로 이동 후 바구니 카운트 업
                _shellObjects[index].gameObject.SetActive(false);
                _SelectShellNumber++;
                _countText.text = $"{_SelectShellNumber}/6";
            }, 1f);


            DelayFunc(2f, () =>
            {
                /// Reset Shell Object
                foreach (var objs in _shellObjects)
                {
                    objs.gameObject.SetActive(true);
                    objs.EnabeldTitleObject(true);
                }
                _isToggleDescButtonEvent = false;
                _shellObjects[index].GetComponent<ShellThrow>().ReturnToOriginal();

                // Debug.Log(_SelectShellNumber);
                if (_SelectShellNumber >= 6)
                {
                    Debug.Log("End Contents and Show Dolphine");
                    AllResetEvent();
                    shellCountUI.SetActive(false);
                    dolphin.SetActive(true);
                    dolphin.GetComponent<Dolphin>().PlayAnimation();
                    return;
                }
                else SelectObject(_SelectShellNumber);
            });
        }

        private void ResetShellButtonEvent()
        {
            foreach (var desc in _shellObjects)
            {
                desc.SetAnimationEvent(false);
                desc.EnabeldDescObject(false);
                desc.EnabledCollider(false);
            }
        }

        private void AllResetEvent()
        {
            foreach (var item in _shellObjects)
                item.AllResetEvent();
        }

        /// <summary>
        /// 조개 UI선택 시 애니메이션 재생 후 실행되는 이벤트
        /// 조개 콜리더 켜지고 설명 팝업 실행
        /// </summary>
        /// <param name="index"></param>
        private void CompleteEvent(int index)
        {
            _shellObjects[index].EnabledCollider(true);
            _shellObjects[index].EnabeldDescObject(true);
        }

        private Coroutine delayCoroutine = null;
        private void DelayFunc(float time, UnityAction onComplete)
        {
            if (delayCoroutine == null) StartCoroutine(DelayFuncCoroutine(time, onComplete));
            else StopCoroutine(delayCoroutine);
        }

        private IEnumerator DelayFuncCoroutine(float time, UnityAction onComplete)
        {
            yield return new WaitForSeconds(time);
            onComplete?.Invoke();
        }

        // 오브젝트 선택 시 호출
        public void SelectObject(int objectIndex)
        {
            if (objectIndex < 0 || objectIndex >= objects.Length)
            {
                Debug.LogError("잘못된 오브젝트 인덱스입니다.");
                return;
            }



            // 텍스트 갱신 후 인덱스 업데이트
            currentStartIndex = (currentStartIndex + TEXT_COUNT_PER_CYCLE) % titleTextData.Length;
            UpdateTextData();
        }

        // 6개의 텍스트 데이터를 갱신
        private void UpdateTextData()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                // 텍스트를 설정할 오브젝트의 TextMeshPro 컴포넌트 찾기
                TextMeshPro textComponent = texts[i]; //objects[i].GetComponentInChildren<TextMeshPro>();
                TextMeshPro descComponent = desces[i];
                if (textComponent == null)
                {
                    Debug.LogError($"오브젝트 {i}에 TextMeshPro가 없습니다.");
                    continue;
                }

                // 새로운 텍스트 할당
                int textIndex = (currentStartIndex + i) % titleTextData.Length;
                textComponent.text = titleTextData[textIndex];
                descComponent.text = descTextData[textIndex];
            }
        }
        private string[] titleTextData = {
        "만들기", "탐구", "아이디어", "공감", "발표", "정리",
        "기계", "운동", "예술", "봉사", "리더십", "기록",
        "요리", "호기심", "표현", "도움", "경쟁", "계획",
        "고치기", "관찰", "창작", "단체활동", "토론", "규칙",
        "고치기", "관찰", "자유", "협동", "목표", "규칙",
        "키우기", "분석", "상상", "대화", "관심", "안정"};

        private string[] descTextData ={
    "나는 손으로 조립하거나 만드는 것을 좋아해!",
    "나는 새로운 물건이나 현상을 다양한 방법으로 알아보는 것을 좋아해!",
    "나는 항상 새로운 아이디어를 떠올리는 것을 좋아해!",
    "나는 친구의 고민이나 어려움이 내 일처럼 느껴져!",
    "나는 여러 사람 앞에서 발표하는 것을 좋아해!",
    "나는 사물함이나 책상 정돈을 좋아해!",
    "나는 컴퓨터, 기차 등이 어떻게 작동하는지 알고 싶어!",
    "나는 몸을 활발하게 움직이는 활동을 좋아해!",
    "나는 문학, 미술, 연극, 영화 같은 예술 작품 감상을 좋아해!",
    "나는 어려운 처지에 있는 사람들을 돕고 싶어!",
    "나는 사람들을 이끄는 역할을 좋아해!",
    "나는 사소한 일들도 메모하는 것을 좋아해!",
    "나는 맛있는 음식 만드는 것을 좋아해!",
    "나는 미스테리나 수수께끼를 보면 스스로 풀어보고 싶어!",
    "나는 생각과 느낌을 말이나 몸짓으로 나타내는 것을 좋아해!",
    "나는 친구에게 힘든 일이 생겼을 때 꼭 도와주고 싶어!",
    "나는 경쟁에서 이기고 싶은 마음이 강해!",
    "나는 무엇을 할지 미리 정하고 차근차근 실천하는 것을 좋아해!",
    "고장난 게임기나 장난감을 보면 스스로 고쳐보고 싶어!",
    "나는 무엇이든 자세히 살펴보는 것을 좋아해!",
    "나는 예술 활동을 통해 새로운 것을 만드는 과정을 좋아해!",
    "나는 다른 사람들과 즐겁게 어울리는 것을 좋아해!",
    "나는 다른 사람에게 내 의견을 잘 이야기할 수 있어!",
    "나는 약속이나 질서를 잘 지키려고 노력해!",
    "고장난 게임기나 장난감을 보면 스스로 고쳐보고 싶어!",
    "나는 무엇이든 자세히 살펴보는 것을 좋아해!",
    "나는 정해진 방법보다 내 마음에 드는 방법을 찾고 싶어!",
    "나는 문제를 혼자보다 함께 해결하는 것을 좋아해!",
    "나는 내가 원하는 것을 이루기 위해 항상 노력해!",
    "나는 약속이나 질서를 잘 지키려고 노력해!",
    "나는 식물이나 동물을 보살피기를 좋아해!",
    "나는 어떤 문제를 틀렸을 때, 틀린 이유를 분명히 알고 싶어!",
    "나는 재미있고 독특한 생각이 머릿속에 자주 펼쳐져!",
    "나는 사람들과 이야기를 주고 받으며 소통하는 것을 좋아해!",
    "나는 주변 사람에게 주목받는 것을 좋아해!",
    "나는 새로운 변화보다 익숙한 환경을 좋아해!"
};

    }
}
