using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData dashData;

        private float startTime; //��¼������״̬��ʱ�䣬�����ж��������

        private int consecutiveDashUsed;  //�ѳ�̴���

        private bool shouldKeepRotating; //�Ƿ���Ҫ������ת��ɫ���������������

        public PlayerDashingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            dashData = movementData.DashData;
        }

        #region IState Methods
        public override void Enter() //�����ٶ���������ת����
        {
            playerMovementStateMachine.ReusableData.movementSpeedModifier = dashData.DashSpeedModifier;

            base.Enter();


            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.StrongForce;


            playerMovementStateMachine.ReusableData.RotationData = dashData.RotationData;

            AddForceOnTransitionFromStationaryState(); //�����ɫ��ֹ������ AddForceOnTransitionFromStationaryState() �����ʼ�������

            shouldKeepRotating = playerMovementStateMachine.ReusableData.movementInput != Vector2.zero;  //�����κ��ƶ�������ʱ��Ӧ�ÿ�����ת����

            UpdateConsecutiveDashed();

            startTime = Time.time; //��¼����״̬��ʱ��
        }

        public override void OnAnimationTransitaionEvent()
        {

            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.HardStoppingState); //��̺����û�к�������Ļ�����ֹͣ״̬

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.SprintingState);  //��̺�������Ļ����뼲��״̬
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!shouldKeepRotating)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }



        

        #endregion



        #region Main Methods
        private void AddForceOnTransitionFromStationaryState()  //�����ɫû���ƶ����룬�����ɫһ����̷�����ٶȣ��ؽ�ɫ��ǰ����
        {
            if (playerMovementStateMachine.ReusableData.movementInput != Vector2.zero)
            {
                return; 
            }

            Vector3 characterRotationDirection = playerMovementStateMachine.Player.transform.forward;

            characterRotationDirection.y = 0f;

            UpdateTargetRotation(characterRotationDirection,false);

            playerMovementStateMachine.Player.Rigidbody.velocity = characterRotationDirection * GetMovementSpeed();
        }

        private void UpdateConsecutiveDashed()
        {
            if (!IsConsecutive())
            {
                consecutiveDashUsed = 0;
            }

            ++consecutiveDashUsed;

            if (consecutiveDashUsed == dashData.ConsecutiveDashesLimitAmount)
            {
                consecutiveDashUsed = 0;
            }

            playerMovementStateMachine.Player.Input.DisableActionFor(playerMovementStateMachine.Player.Input.PlayerActions.Dash,dashData.DashLimitReachedCooldown);
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;  // �жϵ�ǰ��Ϸʱ���Ƿ�С�ڽ��������ʱ��ʱ������ʱ��֮��
        }

        #endregion


        #region Input Methods

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            
        }

        /*
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            
        }
        */
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }

        #endregion

        #region Reusable Methods

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
        }

        

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
        }

        #endregion
    }
}
