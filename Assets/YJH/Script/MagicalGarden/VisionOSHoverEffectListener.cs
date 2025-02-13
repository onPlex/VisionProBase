using UnityEngine;


namespace YJH.EmotionJewel
{


    public class VisionOSHoverEffectListener : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem hoverParticle;
        private Material hoverMaterial;
        private int isActiveID;

        void Start()
        {
            Renderer renderer = GetComponent<Renderer>();  // MeshRenderer 가져오기

            if (renderer != null)
            {
                hoverMaterial = renderer.material;
                isActiveID = Shader.PropertyToID("_PolySpatialHoverStateIsActive"); // Shader 속성 ID
            }
        }

        void Update()
        {
            if (hoverMaterial != null)
            {
                float isHovering = hoverMaterial.GetFloat(isActiveID);  // 0 또는 1 반환
                if (isHovering > 0.9f)
                {
                    if (!hoverParticle.isPlaying)
                        hoverParticle.Play();  // Hover 중이면 파티클 Play
                }
                else
                {
                    if (hoverParticle.isPlaying)
                        hoverParticle.Stop();  // Hover가 끝나면 파티클 Stop
                }
            }
        }
    }
}
