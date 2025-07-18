using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private PlayerWalkData walkData;

        public PlayerWalkingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            walkData = movementData.WalkData;
        }


        #region IState Methods
        public override void Enter()
        {
            playerMovementStateMachine.ReusableData.movementSpeedModifier = movementData.WalkData.speedModifier;

            playerMovementStateMachine.ReusableData.BackwardsCameraRenteringData = walkData.BackwardsCameraRenteringData;

            base.Enter();


            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.WeakForce;

        }

        public override void Exit()
        {
            base.Exit();

            SetBaseCameraRecenteringData();
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

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);


        }

        protected override void OnMovementCanceld(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.LightStoppingState);

            base.OnMovementCanceld(context);
        }

        /*
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {


            playerMovementStateMachine.ChangeState(playerMovementStateMachine.LightStoppingState);

            base.OnMovementCanceld(context);
        }
        */

        /*
        protected void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }
        */

        #endregion



    }


}
