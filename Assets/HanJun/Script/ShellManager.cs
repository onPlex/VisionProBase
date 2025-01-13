using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

namespace Jun
{
    public class ShellManager : MonoBehaviour
    {
        public GameObject _resultCanvas;
        public TMP_Text _countText;
        public GameObject[] objects; // 6개의 오브젝트 (고정)
        public TextMeshPro[] texts;
        public ShellBehavior[] _shellObjects;

        private int currentStartIndex = 0; // 현재 텍스트 시작 인덱스
        private const int TEXT_COUNT_PER_CYCLE = 6; // 한 번에 보여줄 텍스트 데이터 개수

        private int _SelectShellNumber = 0;

        private string[] titleTextData = {
        "만들기", "탐구", "아이디어", "공감", "발표", "정리",
        "기계", "운동", "예술", "봉사", "리더십", "기록",
        "요리", "호기심", "표현", "도움", "경쟁", "계획",
        "고치기", "관찰", "창작", "단체활동", "토론", "규칙",
        "고치기", "관찰", "자유", "협동", "목표", "규칙",
        "키우기", "분석", "상상", "대화", "관심", "안정"};

        private string[] descTextData = { };

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

            UpdateTextData(); // 초기 텍스트 설정
        }

        // int tempNumber = 0;
        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.Q))
        //     {
        //         SelectObject(tempNumber);
        //         tempNumber++;
        //         if (tempNumber >= 6) tempNumber = 0;
        //     }
        // }

        /// <summary>
        /// 조개 선택시 이벤트
        /// </summary>
        /// <param name="index"></param> <summary>
        public void SelectShellEvent(int index)
        {
            ResetShellButtonEvent();

            _shellObjects[index].SetAnimationEvent(false, () => MoveToBasket(index));
        }

        /// <summary>
        /// 조개 아래 UI버튼 이벤트
        /// </summary>
        /// <param name="index"></param>
        bool _isToggleDescButtonEvent = false;
        public void SelectButtonEvent(int index)
        {
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
            }, 0.1f);


            DelayFunc(0.1f, () =>
            {
                /// Reset Shell Object
                foreach (var objs in _shellObjects)
                {
                    objs.gameObject.SetActive(true);
                    objs.EnabeldTitleObject(true);
                }
                _isToggleDescButtonEvent = false;
                _shellObjects[index].GetComponent<ShellThrow>().ReturnToOriginal();

                Debug.Log(_SelectShellNumber);
                if (_SelectShellNumber >= 6)
                {
                    Debug.Log("End Contents and Show ResultPopup");
                    ResetShellButtonEvent();
                    _resultCanvas.SetActive(true);
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
                if (textComponent == null)
                {
                    Debug.LogError($"오브젝트 {i}에 TextMeshPro가 없습니다.");
                    continue;
                }

                // 새로운 텍스트 할당
                int textIndex = (currentStartIndex + i) % titleTextData.Length;
                textComponent.text = titleTextData[textIndex];
            }
        }
    }
}