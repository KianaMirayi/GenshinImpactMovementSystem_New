using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public SlopeData slopeData { get; private set; }

        public PlayerGroundedState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            slopeData = playerMovementStateMachine.Player.capsuleColiderUtility.slopeData;
        }



        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            UpdateShouldSprint();

            UpdataCameraRecenteringState(playerMovementStateMachine.ReusableData.movementInput);
        }

        

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

        #endregion

        #region Main Methods

        private void Float()
        {
            /*
             	��֤��ɫ�ڵ��α�����Ȼ�ƶ����������ջ�ģ��
             	��̬��Ӧ��ͬ�¶ȣ�������ɫ����ε����������顣
             	����ٶ������͸���������˶���������Ч����
              */
            Vector3 capsuleColliderCentterInWorldSpace = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.Collider.bounds.center;  // 1. ��ȡ������ײ�������ռ����ĵ�

            Ray downwardsRayFromCapsuleColliderCenter = new Ray(capsuleColliderCentterInWorldSpace, Vector3.down); // 2. ����ײ�����ĵ����·���һ������

            //3. ʹ�����߼����棬�޶����߾���ͼ���
            if (Physics.Raycast(downwardsRayFromCapsuleColliderCenter, out RaycastHit hit, slopeData.floatRayDistance, playerMovementStateMachine.Player.layerData.groundLayer,QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal,-downwardsRayFromCapsuleColliderCenter.direction);// 4. ������淨�����ɫ���·���ļнǣ������¶Ƚǣ�

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);// 5. �����¶Ƚǻ�ȡ�ٶ�����ϵ��

                if (slopeSpeedModifier == 0f)// 6. �������ϵ��Ϊ0��ֱ�ӷ��أ������ƶ����¶ȹ���
                { 
                    return;
                }

                // 7. �����ɫ��Ҫ���յľ��루��ɫ�ײ�������ľ��룩
                float distanceToFloatingPoint = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.ColliderCenterInLocalSpace.y * playerMovementStateMachine.Player.transform.localScale.y - hit.distance ;

                // 8. �������Ϊ0��˵���Ѿ����ϵ��棬���账��
                if (distanceToFloatingPoint == 0f)
                { 
                    return;
                }

                // 9. ������Ҫʩ�ӵĸ����������ǵ�ǰ��ֱ�ٶȣ�
                //slopeData.stepReachForce һ���ɵ��ڵĸ�����ϵ����������ɫ�����ϵ��桱ʱ����Ӧǿ�ȡ���ֵԽ�󣬽�ɫԽ�����ϵ��棬ԽС��Խ��͡�
                //�����ɫ��Ҫ����̧��������ѹ����Ŀ���ٶȣ������������ݽ�ɫ�����ľ������Ӧϵ��������
                //������ɫ�Ѿ�ӵ�е���ֱ�ٶȣ�ȷ��ʩ�ӵ���ֻ���ھ�����ɫ�����ľ��룬�����ǵ��������ٶȡ�
                float amoutToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amoutToLift, 0f);

                playerMovementStateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);// 10. ����ɫ����ʩ����ֱ���������ʹ�両�ջ����ϵ���
            }



        }

        private float SetSlopeSpeedModifierOnAngle(float angle)  //ʹ�ö�����������ﲻͬб�ʵ������ƶ��ٶȳ���
        {
            float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);

            if (playerMovementStateMachine.ReusableData.movementOnSlopeSpeedModifier != slopeSpeedModifier)
            { 
                playerMovementStateMachine.ReusableData.movementOnSlopeSpeedModifier = slopeSpeedModifier;

                UpdataCameraRecenteringState(playerMovementStateMachine.ReusableData.movementInput);
                
            }


            return slopeSpeedModifier;

        }

        private void UpdateShouldSprint()
        {
            if (!playerMovementStateMachine.ReusableData.shouldSprint)
            {
                return;
            }

            if (playerMovementStateMachine.ReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            playerMovementStateMachine.ReusableData.shouldSprint = false;
        }


        private bool IsThereGroundUnderBeneath()
        {
            BoxCollider groundCheckCollider = playerMovementStateMachine.Player.capsuleColiderUtility.TriggerColliderData.GroundCheckCollider;

            Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;

            Collider[] overlappedGroundColldiers = Physics.OverlapBox(groundColliderCenterInWorldSpace,groundCheckCollider.bounds.extents,groundCheckCollider.transform.rotation,playerMovementStateMachine.Player.layerData.groundLayer,QueryTriggerInteraction.Ignore);

            return overlappedGroundColldiers.Length > 0;


        }

        #endregion

        #region Reusable Methods

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            //playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;

            playerMovementStateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;

            playerMovementStateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }



        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            //playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;

            playerMovementStateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;

            playerMovementStateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }

        protected override void OnContactWithGroundExit(Collider collider)
        {
            base.OnContactWithGroundExit(collider);

            if (IsThereGroundUnderBeneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.ColliderVerticalExtents, Vector3.down);

            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, movementData.GroundToFallRayDistance, playerMovementStateMachine.Player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
            { 
                OnFall();
                
            }
        }

        

        protected virtual void OnFall()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.FallingState);
        }

        #endregion


        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);


        }

        /*
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }
        */

        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.dashingState);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.JumpingState);
        }

        protected virtual void OnMove()
        {
            if (playerMovementStateMachine.ReusableData.shouldSprint)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.SprintingState);

                return;
            }

            if (playerMovementStateMachine.ReusableData.shouldWalk)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.WalkingState);

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);
        }

        #endregion
    }
}
