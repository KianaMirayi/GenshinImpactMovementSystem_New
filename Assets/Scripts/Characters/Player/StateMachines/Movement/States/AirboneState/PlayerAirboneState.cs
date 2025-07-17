using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerAirboneState : PlayerMovementState
    {
        public PlayerAirboneState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }

        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            ResetSprintState();
        }

        #endregion

        #region Reusable Methods

        protected override void OnContactWithGround(Collider collider)
        { 
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.LightLandingState);
        }


        protected virtual void ResetSprintState()
        { 
            playerMovementStateMachine.ReusableData.shouldSprint = false;
        }
        #endregion
    }
}
