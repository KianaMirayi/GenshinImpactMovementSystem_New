using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        //从高处坠落且没有输入时进入硬着陆状态，在进入硬着陆状态的一瞬间禁用输入，在硬着陆状态的最后一个帧开始调用转换动画进入待机状态，硬着陆状态中的某一个帧启用输入，在退出硬着陆状态时启用输入
        public PlayerHardLandingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f;

            base.Enter();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.Disable();  //进入硬着陆状态时不能移动


            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.Enable();  //推出硬着陆状态时可以移动
        }



        public override void OnAnimationTransitaionEvent()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }

        public override void OnAnimationExitEvent()
        {
            playerMovementStateMachine.Player.Input.PlayerActions.Movement.Enable();  //在硬着陆动画中的某一个帧可以移动

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVelocity();

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
