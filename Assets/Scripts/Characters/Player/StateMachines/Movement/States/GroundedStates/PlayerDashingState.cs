using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData dashData;

        private float startTime;

        private int consecutiveDashUsed;  //�ѳ�̴���

        public PlayerDashingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            dashData = movementData.DashData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = dashData.DashSpeedModifier;

            AddForceOnTransitionFromStationaryState();

            UpdateConsecutiveDashed();

            startTime = Time.time;
        }

        public override void OnAnimationTransitaionEvent()
        {

            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.HardStoppingState); //��̺����û�к�������Ļ�����ֹͣ״̬

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.SprintingState);  //��̺����ǰ���Ļ����뼲��״̬
        }

        

        #endregion



        #region Main Methods
        private void AddForceOnTransitionFromStationaryState()
        {
            if (playerMovementStateMachine.ReusableData.movementInput != Vector2.zero)
            {
                return; 
            }

            Vector3 characterRotationDirection = playerMovementStateMachine.Player.transform.forward;

            characterRotationDirection.y = 0f;

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
            return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;  // �жϵ�ǰ��Ϸʱ���Ƿ���ڽ��������ʱ��ʱ������ʱ��֮��
        }

        #endregion


        #region Input Methods

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            
        }

        #endregion
    }
}
