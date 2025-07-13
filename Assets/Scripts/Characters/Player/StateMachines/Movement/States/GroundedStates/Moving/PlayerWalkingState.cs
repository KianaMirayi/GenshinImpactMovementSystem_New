using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public PlayerWalkingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }


        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = movementData.WalkData.speedModifier ;
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

        /*
        protected void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }
        */

        #endregion

        

    }


}
