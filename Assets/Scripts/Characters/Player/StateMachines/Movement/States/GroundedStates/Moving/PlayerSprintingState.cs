using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData sprintData;

        private bool keepSprinting;

        private float startTime;

        public PlayerSprintingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }

        #region
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = sprintData.sprintSpeedModifier;

            startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (keepSprinting)
            { 
                return;
            }

            if (Time.time < startTime + sprintData.sprintToRunTime)
            {
                return;
            }

            StopSprinting();
        }

        public override void Exit()
        {
            base.Exit();

            keepSprinting = false;
        }

        #region Main Methods
        private void StopSprinting()
        {
            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState); //之后会换成停止状态

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);
        }

        #endregion



        #endregion

        #region Reusable Methods

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }

        

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
        }



        #endregion



        #region Input Methods
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;

        }

        #endregion
    }
}
