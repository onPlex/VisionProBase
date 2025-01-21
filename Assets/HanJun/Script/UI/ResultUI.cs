using TMPro;
using UnityEngine;

namespace Jun
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField]  private YJH.MainContentManager mainContentManager;
        [SerializeField] private ShellManager shellManager;
        [SerializeField] private GameObject[] resultViews;
        [SerializeField] private UnityEngine.UI.Image pearlImage;
        [SerializeField] private Sprite[] pearlSprites;

        [SerializeField] private MeshRenderer pearlMesh;
        [SerializeField] private Material[] pearlColors;

        [SerializeField] private TMP_Text text_Name;
        [SerializeField] private TMP_Text text_Desc;
        [SerializeField] private TMP_Text text_Activiies;
        [SerializeField] private TMP_Text text_Job;


        private void OnEnable()
        {
            ResultCareerEvent();
        }

        private void ResultCareerEvent()
        {
            // 열거형 값을 기반으로 이미지, 텍스트 업데이트
            // SetImage((int)shellManager.CareerType);
            // SetText((int)shellManager.CareerType);

             // 열거형 값을 기반으로 이미지, 텍스트 업데이트
            SetImage((int)mainContentManager.finalCareer);
            SetText((int)mainContentManager.finalCareer);
        }

        private void SetImage(int index)
        {
            pearlImage.sprite = pearlSprites[index];
            pearlMesh.material = pearlColors[index];
        }

        private void SetText(int index)
        {
            text_Name.text = pearlType[index];
            text_Desc.text = character[index];
        }

        public void SetResult2View()
        {
            resultViews[0].SetActive(false);
            resultViews[1].SetActive(true);
           // text_Activiies.text = activities[(int)shellManager.CareerType];
            text_Activiies.text = activities[(int)mainContentManager.finalCareer];
        }

        public void SetResult3View()
        {
            resultViews[1].SetActive(false);
            resultViews[2].SetActive(true);
           // text_Job.text = job[(int)shellManager.CareerType];
             text_Job.text = job[(int)mainContentManager.finalCareer];
        }

        private string[] pearlType = new string[] { "뚝딱진주", "궁금진주", "상상진주", "친절진주", "열정진주", "꼼꼼진주" };
        private string[] character = new string[]
{
    "- 솔직하고 성실한 성격을 가지고 있어요.\n - 몸을 많이 움직이는 활동을 좋아해요.\n - 새로운 아이디어를 생각하기 보다는 기계나 도구를 다루는 일을 좋아해요.",
    "- 호기심이 많고 독립적인 성격을 가지고 있어요.\n - 세심히 관찰하며 새로운 정보를 알아가는 것을 좋아해요.\n - 새로운 문제를 창의적으로 해결하는 것을 좋아해요.",
    "- 상상력이 많고 감정이 풍부한 성격을 가지고 있어요.\n - 개성 있는 독특한 방법으로 스스로를 표현하는 것을 좋아해요.\n - 글쓰기, 음악, 미술을 좋아하고 나만의 세계로 빠져드는 것을 좋아해요.",
    "- 이해심 많고 사교적인 성격을 가지고 있어요.\n - 어려운 상황에 처한 사람을 도와주는 것을 좋아해요.\n - 친구들과 사이좋게 지내고, 함께 활동하는 것을 좋아해요.",
    "- 사람들과 잘 어울리고, 리더십 있는 성격이에요.\n - 친구들을 설득하고 이끄는 것을 좋아해요.\n - 말을 자신 있게 잘하고, 모든 일에 열심히 참여해요.",
    "- 책임감이 강하고 정직한 성격이에요.\n - 스스로 계획을 세우고 꾸준히 실천하는 것을 좋아해요.\n - 친구들과의 약속이나 학교 규칙, 질서를 잘 지켜요."
};

        private string[] activities = new string[]
        {
    "- 나만의 텃밭을 가꾸어 보세요.\n - 작은 도구나 기구를 가지고 DIY*를 완성해 보세요.\n (*DIY : 가정에서 쓰이는 가구, 장식 등을 직접 만드는 활동) \n - 레시피를 따라 먹고 싶은 음식을 만들어 보세요.",
    "- 챗GPT와 같은 대화형 인공지능 서비스를 활용하여 학교나 학원에서 배운 내용에 대한 새로운 정보를 탐색해 보세요. \n - 현장학습이 있다면 배울 내용에 대해 미리 찾아보세요.\n - 관심 있는 사물이나 현상을 관찰하고 새롭게 알게 된 것을 정리해 보세요.",
    "- 예술작품(영화, 전시, 공연, 연주회 등)을 보고 느낀 점을 자유롭게 표현해 보세요.\n - 글쓰기, 음악, 미술활동 등을 통해 나만의 작품을 만들어보세요.\n - 노래, 춤, 연주, 연기, 그리기 등 예술활동을 할 수 있는 동아리에 참여해 보세요.",
    "- 어려운 이웃을 보살피는 봉사활동에 꾸준히 참여해 보세요.\n - 학교생활에 어려움을 겪는 주변 친구를 돕기 위해 노력해 보세요.\n - 사람들과 함께 모여 문제를 해결하는 경험을 쌓아보세요.",
    "- 바자회 등에 참여해 물건을 팔고 이익을 내는 활동을 해보세요.\n - 학급 행사와 같은 행사를 직접 계획하고, 사람들을 이끌어 실행해 보세요.\n - 여러 사람 앞에서 발표하는 경험을 쌓아보세요.",
    "- 숙제 시작 전, 잠자기 전 등 하루에 5분 정도 책상이나 방의 물건들을 정리하는 습관을 가져보세요. 나만의 정리 기준을 세워봐도 좋아요.\n - 용돈기입장을 활용하여 용돈이 들어오고 나가는 과정을 꼼꼼히 기록해 보세요.\n - 하루, 일주일, 한 달 등 기간별로 계획을 세우고 그대로 실천해 보세요."
        };

        private string[] job = new string[]
        {
    "동물사육사, 농업기술자, 관광가이드, 응급구조사, 경호원, 운전기사, 운동선수, 요리사, 소방관, 항해사, 미용사, 프로게이머, 직업군인, 항공기 정비원, 컴퓨터프로그래머 나노공학기술자, 로봇컨설턴트, 드론조종사, 디지털장의사 등",
    "대학교수, 물리학자, 화학자, 수학자, 통계학자, 천문학자, 지질학자, 기상연구원, 생물학자, 생명공학자, 유전공학자, 의사, 수의사, 약사, 컴퓨터 \n 프로그래머, 사이버범죄수사관, 인공지능전문가, 재난관리전문가, 사물인터넷개발자 등",
    "PD, 지휘자, 작곡가, 연예인, 음악평론가, 화가, 애니메이터, 제품디자이너, 공연기획자, 패션디자이너, 영화감독, 사진작가, 공예원, 소설가, 카피라이터, 시인, 문학평론가, 공간 스토리텔러, 컴퓨터 그래픽디자이너, 게임테크니컬 아티스트 등",
    "간호사, 보육교사, 교사, 놀이치료사, 유치원교사, 파티플래너, 바리스타, 승무원, 실버케어복지사, 언어치료사, 물리치료사, 심리상담사, 병원코디네이터, 레크레이션지도자, 스포츠매니저, 여행상품 개발자, 해외영업원, 사회복지사, 사회단체활동가, 성직자 등",
    "경영인, 국회의원, 언론인, 판매인, 정부행정관리자, 경엉컨설턴트, 호텔경영자, 변호사, 아나운서, 방송기자, 논설위원, 투자상담사, 부동산중개인, 텔레마케터, 쇼핑호스트, 광고전문가, 금융자산운용가, 회계사, 외교관, 해외영업원 등",
    "사무직종사자, 사서, 비서, 회계사, 세무사, 은행원, 인사사무원, 일반공무원, 우편사무원, 보건의료정보관리사, 여론조사전문가, 문화재보존가, 정보시스템 운영자, 판사, 검사, 임상병리사, 데이터베이스 개발자, 관세사, GIS전문가, 법무사 등"
        };

    }
}