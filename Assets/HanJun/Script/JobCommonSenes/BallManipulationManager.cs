using System.Collections.Generic;
using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Jun
{
    /// <summary>
    /// 볼(또는 기타 오브젝트)을 잡아(Select) 드래그/회전하는 동안 게이지를 충전하고,
    /// 포인터가 해제될 때 Shoot()을 호출하는 매니저 예시
    /// </summary>
    public class BallManipulationManager : MonoBehaviour
    {
        // PieceSelectionBehavior와 동일하게 -1이면 "선택안됨"
        internal const int k_Deselected = -1;

        // ★ 편의상 "Selection" 구조체에 chargeTime 추가
        struct Selection
        {
            public BallSelectionBehavior Ball;    // 현재 선택된 볼
            public Vector3 PositionOffset;        // 초기 오프셋
            public Quaternion RotationOffset;     // 초기 회전오프셋
            public float chargeTime;              // 잡는 동안 누적하는 게이지 시간
        }

        // 포인터 ID -> 선택 상태(볼/오프셋/게이지) 매핑
        readonly Dictionary<int, Selection> m_CurrentSelections = new();

        // 게이지 관련 (예: 최대 충전시간)
        [SerializeField] float m_MaxChargeTime = 3f;

        // 포인터(ID)별로 “이전 프레임 Rotation”을 기록해둘 사전
        Dictionary<int, Quaternion> m_PrevRotations = new Dictionary<int, Quaternion>();

        // 포인터(ID) -> 선택된 BallSelectionBehavior 매핑
        Dictionary<int, BallSelectionBehavior> m_Selections = new Dictionary<int, BallSelectionBehavior>();



        void OnEnable()
        {
            // 멀티터치 지원
            EnhancedTouchSupport.Enable();
        }

        void Update()
        {
            foreach (var touch in Touch.activeTouches)
            {
                var spatialPointerState = EnhancedSpatialPointerSupport.GetPointerState(touch);
                var interactionId = spatialPointerState.interactionId;

                // 1) Touch라면 무시 (원하는 경우에 따라)
                if (spatialPointerState.Kind == SpatialPointerKind.Touch)
                    continue;

                // 2) Moved 단계에서 → 회전 Delta 계산 & ChargeGauge 호출
                if (spatialPointerState.phase == SpatialPointerPhase.Moved)
                {
                    // (A) 현재 이 포인터로 잡힌 볼이 있는지 확인
                    if (m_Selections.TryGetValue(interactionId, out var ball))
                    {
                        // (B) 이전 프레임 Rotation 가져오기 (없으면 현재값 저장)
                        if (!m_PrevRotations.TryGetValue(interactionId, out var prevRot))
                            prevRot = spatialPointerState.inputDeviceRotation;

                        // (C) Delta Rotation 계산
                        Quaternion currentRot = spatialPointerState.inputDeviceRotation;
                        Quaternion deltaRot = currentRot * Quaternion.Inverse(prevRot);
                        Vector3 deltaEuler = deltaRot.eulerAngles;
                        float deltaY = deltaEuler.y; // y축 회전량

                        // (D) ChargeGauge(deltaY) 호출
                        ball.ChargeGauge(deltaY);

                        // (E) 이번 프레임 Rotation 저장
                        m_PrevRotations[interactionId] = currentRot;
                    }
                }
                else if (spatialPointerState.phase == SpatialPointerPhase.Ended ||
                         spatialPointerState.phase == SpatialPointerPhase.Cancelled)
                {
                    // 포인터 해제 → 발사 & 선택 해제
                    if (m_Selections.TryGetValue(interactionId, out var ball))
                    {
                        ball.Shoot();
                        ball.SetSelected(-1); // deselect
                    }

                    // 기록 제거
                    m_Selections.Remove(interactionId);
                    m_PrevRotations.Remove(interactionId);
                }
                else if (spatialPointerState.phase == SpatialPointerPhase.Began)
                {
                    // 새로 잡은(선택된) 오브젝트가 있는지 확인
                    var targetObj = spatialPointerState.targetObject;
                    if (targetObj && targetObj.TryGetComponent(out BallSelectionBehavior bsb))
                    {
                        // 이미 다른 포인터가 잡고 있지 않으면
                        if (bsb.selectingPointer < 0)
                        {
                            // 잡기 표시
                            bsb.SetSelected(interactionId);

                            // 매핑 기록
                            m_Selections[interactionId] = bsb;

                            // 초기 Rotation 기록
                            m_PrevRotations[interactionId] = spatialPointerState.inputDeviceRotation;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 선택 해제
        /// </summary>
        void DeselectBall(int interactionId)
        {
            if (m_CurrentSelections.TryGetValue(interactionId, out var selection))
            {
                selection.Ball.SetSelected(k_Deselected);
                m_CurrentSelections.Remove(interactionId);
            }
        }
    }
}
