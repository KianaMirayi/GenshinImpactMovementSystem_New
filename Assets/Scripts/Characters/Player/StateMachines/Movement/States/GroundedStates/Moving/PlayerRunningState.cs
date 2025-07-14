using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerRunningState : PlayerMovingState
    {

        private float startTime;

        private PlayerSprintData sprintData;
        public PlayerRunningState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = movementData.RunData.speedModifier;

            startTime = Time.time;

        }

        public override void Update()
        {
            base.Update();

            if (!playerMovementStateMachine.ReusableData.shouldWalk)
            { 
                return;
            }

            if (Time.time < startTime + sprintData.runToWalkTime)
            { 
                return ;
            }

            StopRunning();
            
        }





        #endregion

        #region Main Methods
        private void StopRunning()
        {
            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState); //之后会换成停止状态

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.WalkingState);
        }

        #endregion


        /*
        #region Reusable Methods

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
        }



        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        }

        #endregion
        */



        #region Input Methods

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.WalkingState);


        }

        /*
        protected void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }
        */

        #endregion

        
    }
}
