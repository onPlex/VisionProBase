using UnityEngine;

namespace Novaflo.Login
{
    public class KochatGameData
    {
        // 구분
        public string Kind { get; set; }
        // 게임명
        public string Title { get; set; }
        // 텍스트 (문서 참고)
        public string Cont { get; set; }
        // 이미지 타입 1~12 (컨텐츠 ID)
        public int ImgType { get; set; }
        // 1:완료, 2:시간종료나 기타 사유로 비종료
        public int Status { get; set; }
    }
}