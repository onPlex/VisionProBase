using UnityEngine;
using UnityEngine.UI;

namespace Jun
{
    public class BallPickAndShoot : MonoBehaviour
    {
        private Rigidbody rb;
        private bool isRotating = false;
        private bool isPulling = false;
        private float rotationSpeed = 100f;
        private Vector3 initialPosition;
        private float pullDistance = 0.5f;
        private bool isLaunched = false;
        private float launchForce;
        private float fallGravity;
        private Transform leftTarget, rightTarget;

        public void Initialize(float launchF, float fallG, Transform leftT, Transform rightT)
        {
            launchForce = launchF;
            fallGravity = fallG;
            leftTarget = leftT;
            rightTarget = rightT;
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            initialPosition = transform.position;
        }

        private void Update()
        {
            if (isPulling)
            {
                transform.position = initialPosition - transform.forward * pullDistance;
            }
            if (isRotating)
            {
                float horizontalInput = Input.GetAxis("Mouse X");
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + horizontalInput * rotationSpeed * Time.deltaTime, 0);
            }
        }

        public void StartPulling()
        {
            isPulling = true;
        }

        public void StopPulling()
        {
            isPulling = false;
            transform.position = initialPosition; // 원래 위치로 복귀
        }

        public void StartRotation()
        {
            isRotating = true;
        }

        public void StopRotation()
        {
            isRotating = false;
        }

        public void LaunchBall()
        {
            if (isLaunched) return;
            isLaunched = true;
            rb.isKinematic = false;
            rb.useGravity = true;

            Vector3 launchDirection = transform.forward + Vector3.up * 0.5f;
            rb.linearVelocity = launchDirection.normalized * launchForce;
        }

        public void FallBall()
        {
            if (isLaunched) return;
            isLaunched = true;
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.down * fallGravity;
        }
    }
}