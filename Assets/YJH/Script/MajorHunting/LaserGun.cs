using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class LaserGun : MonoBehaviour
{
    [SerializeField] private InputActionReference gazeAction; // VisionOS에서 Gaze 입력을 가져옴
    public float rotationSpeed = 5.0f; // 회전 속도
    public bool lockYAxis = false; // Y축 회전 제한 여부

    void OnEnable()
    {
        gazeAction.action.Enable(); // Gaze Action 활성화
    }

    void OnDisable()
    {
        gazeAction.action.Disable(); // Gaze Action 비활성화
    }

    void Update()
    {
        if (gazeAction == null)
            return;

        // Gaze 데이터 가져오기
        var gazePointerState = gazeAction.action.ReadValue<SpatialPointerState>();

        if (gazePointerState.startInteractionRayDirection == Vector3.zero)
            return; // Gaze 데이터가 없으면 종료

        // 시선이 향하는 방향
        Vector3 gazeDirection = gazePointerState.startInteractionRayDirection;

        // 회전 로직 개선 (Y축 고정 옵션 추가)
        Quaternion targetRotation;
        if (lockYAxis)
        {
            // Y축 회전을 제한하여 수평으로만 따라가도록 설정
            Vector3 flattenedDirection = new Vector3(gazeDirection.x, 0, gazeDirection.z);
            targetRotation = Quaternion.LookRotation(flattenedDirection);
        }
        else
        {
            // 전체 방향을 향하도록 설정
            targetRotation = Quaternion.LookRotation(gazeDirection);
        }

        // 부드러운 회전 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
