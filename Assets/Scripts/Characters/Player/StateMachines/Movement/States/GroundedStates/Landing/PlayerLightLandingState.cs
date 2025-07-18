using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f;

            base.Enter();


            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.StationaryForce;

            ResetVelocity();
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

        public override void OnAnimationTransitaionEvent()
        {


            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }

        #endregion

        #region Input Methods
        
        #endregion
    }
}
