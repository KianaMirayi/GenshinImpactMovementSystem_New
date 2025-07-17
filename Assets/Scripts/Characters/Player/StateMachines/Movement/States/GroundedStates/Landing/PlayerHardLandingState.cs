using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        //�Ӹߴ�׹����û������ʱ����Ӳ��½״̬���ڽ���Ӳ��½״̬��һ˲��������룬��Ӳ��½״̬�����һ��֡��ʼ����ת�������������״̬��Ӳ��½״̬�е�ĳһ��֡�������룬���˳�Ӳ��½״̬ʱ��������
        public PlayerHardLandingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.Disable();  //����Ӳ��½״̬ʱ�����ƶ�

            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f;

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.Enable();  //�Ƴ�Ӳ��½״̬ʱ�����ƶ�
        }



        public override void OnAnimationTransitaionEvent()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }

        public override void OnAnimationExitEvent()
        {
            playerMovementStateMachine.Player.Input.PlayerActions.Movement.Enable();  //��Ӳ��½�����е�ĳһ��֡�����ƶ�

        }

        #endregion



        #region Reusable Methods

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.started += OnMovementStared;
        }

        

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.started -= OnMovementStared;
        }

        protected override void OnMove()
        {
            if (playerMovementStateMachine.ReusableData.shouldWalk)
            {
                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);
        }

        #endregion

        #region Input Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            base.OnMovementCanceled(context);
        }


        private void OnMovementStared(InputAction.CallbackContext context)
        {
            OnMove();
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            
        }

        #endregion
    }


}
