using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        private PlayerIdleData idleData;

        public PlayerIdlingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            idleData = movementData.IdleData;
        }


        #region IState Methods
        public override void Enter()
        {
            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f;

            playerMovementStateMachine.ReusableData.BackwardsCameraRenteringData = idleData.BackwardsCameraRenteringData;

            base.Enter();

            StartAnimation(playerMovementStateMachine.Player.AnimationData.IdleParamaterHash);


            ResetVelocity();

            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.StationaryForce;
        }

        public override void Update()
        {
            base.Update();

            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
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

        public override void Exit()
        {
            base.Exit();

            StopAnimation(playerMovementStateMachine.Player.AnimationData.IdleParamaterHash);
        }
        

        #endregion
    }




}
