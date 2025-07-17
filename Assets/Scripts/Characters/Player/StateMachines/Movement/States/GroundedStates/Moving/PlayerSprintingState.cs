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

        private bool shouldResetSprintState;

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

            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.StrongForce;

            shouldResetSprintState = true;


            startTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (keepSprinting)
            { 
                return;
            }

            if (Time.time < startTime + sprintData.sprintToRunTime)//�����ǰʱ�������뼲��״̬��ʱ�仹û���� sprintToRunTime��������̳���ʱ�䣩����������ܣ�����״̬�л�
            {
                return;
            }

            StopSprinting();
        }

        public override void Exit()
        {
            base.Exit();


            if (shouldResetSprintState)
            { 
                keepSprinting = false;

                playerMovementStateMachine.ReusableData.shouldSprint = false;
                
            }
        }

        #region Main Methods
        private void StopSprinting()
        {
            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState); //֮��ỻ��ֹͣ״̬

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

        protected override void OnFall()
        {
            shouldResetSprintState = false;

            base.OnFall();
        }



        #endregion



        #region Input Methods
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;

            playerMovementStateMachine.ReusableData.shouldSprint = true;

        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.HardStoppingState);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            shouldResetSprintState = false;

            base.OnJumpStarted(context);
        }

        #endregion
    }
}
