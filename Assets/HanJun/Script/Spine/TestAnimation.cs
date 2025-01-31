using UnityEngine;

namespace Jun
{
    public class TestAnimation : MonoBehaviour
    {
        [SerializeField] private BanjangAnimation banjangAnimation;
        [SerializeField] private KimsiuAnimation kimsiuAnimation;
        [SerializeField] private ManAnimation manAnimation;
        [SerializeField] private MotherAnimation motherAnimation;
        [SerializeField] private TeacherAniamtion teacherAniamtion;
        [SerializeField] private WomanAnimation womanAnimation;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                banjangAnimation.PlaySadAnimation();
                kimsiuAnimation.PlayAngryAnimation();
                manAnimation.PlaySadAnimation();
                motherAnimation.PlayAngryAnimation();
                teacherAniamtion.PlayWowAnimation();
                womanAnimation.PlayHappyAnimation();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                banjangAnimation.PlayAnnoyAnimation();
                kimsiuAnimation.PlayEmbarrassedAnimation();
                manAnimation.PlayHelloAnimation();
                motherAnimation.PlaySadAnimation();
                womanAnimation.PlayHelloAnimation();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                kimsiuAnimation.PlayGoodAnimation();
                womanAnimation.PlaySadAnimation();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                kimsiuAnimation.PlayHelloAnimation();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                kimsiuAnimation.PlaySadAnimation();
            }
        }
    }
}