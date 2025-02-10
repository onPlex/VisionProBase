using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField]
    [Tooltip(" true = O, false = X")]
    bool basketType; // true = O, false = X
    [SerializeField]
    int maxHitCount = 3;
    [SerializeField]
    int currentHitCount;
    [SerializeField]
    Animator myAnimator;

    [SerializeField] private Jun.OXQuizManager oXQuizManager;
    [SerializeField] private ParticleSystem hit;


    private void Start()
    {
        currentHitCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            currentHitCount++;
            if (currentHitCount >= maxHitCount)
            {
                currentHitCount = 0;
            }
            else
            {
                myAnimator.SetTrigger("Hit");
            }

            Debug.Log("BallCheck");
            myAnimator.SetTrigger("Hit");
            oXQuizManager.OnSelectO();
        }
    }

}
