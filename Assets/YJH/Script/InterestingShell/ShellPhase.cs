using UnityEngine;

public class ShellPhase : MonoBehaviour
{
  [Header("Which Phase is this? (0 ~ 5)")]
    [SerializeField] private int phaseIndex;

    [Header("ShellInfo References (6 items)")]
    [SerializeField] private ShellInfo[] shellInfos = new ShellInfo[6];

    // 36개의 WordTextData
    private string[] WordTextData = {
        "만들기", "탐구", "아이디어", "공감", "발표", "정리",
        "기계",   "운동", "예술",   "봉사", "리더십", "기록",
        "요리",   "호기심", "표현",  "도움", "경쟁",   "계획",
        "고치기", "관찰", "창작",   "단체활동", "토론", "규칙",
        "고치기", "관찰", "자유",   "협동",   "목표", "규칙",
        "키우기", "분석", "상상",   "대화",   "관심", "안정"
    };

    // 36개의 WordDescTextData
    private string[] WordDescTextData = {
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

    private void Awake()
    {
        // Phase 오브젝트가 활성화되거나 생성되면, 해당 Phase의 ShellInfo들을 초기화
        AssignShellData();
    }

    /// <summary>
    /// phaseIndex에 따라 WordTextData / WordDescTextData에서
    /// 6개 항목을 골라 shellInfos 에 설정
    /// </summary>
    private void AssignShellData()
    {
        // 예: phaseIndex=0 이면 WordTextData[0..5], phaseIndex=1 이면 [6..11]
        int baseIndex = phaseIndex * 6; // 6개 단위로 구분

        // shellInfos 6개 순회
        for (int i = 0; i < shellInfos.Length; i++)
        {
            // 인덱스가 baseIndex + i
            int dataIndex = baseIndex + i;
            ShellInfo shell = shellInfos[i];
            if (shell != null)
            {
                // ShellInfo가 가진 데이터를 직접 설정해주거나,
                // public setter/메서드를 만들어 호출할 수도 있음
                shell.WordText = (WordTextData[dataIndex]);
                shell.WordDesc = (WordDescTextData[dataIndex]);

                // ShellInfo 측에서 UI를 갱신하도록 요청
                shell.DisplayInfo();
            }
        }
    }
}
