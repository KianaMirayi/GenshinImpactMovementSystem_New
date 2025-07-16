using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementDecelerationForce = movementData.StopData.hardDecelerationForce;

            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.StrongForce;

        }

        #endregion

        #region Reusable Methods
        protected override void OnMove()
        {
            if (playerMovementStateMachine.ReusableData.shouldWalk)
            {
                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);
        }

        #endregion

    }
}
