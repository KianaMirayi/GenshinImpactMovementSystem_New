using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }


        #region IState Methods

        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementDecelerationForce = movementData.StopData.lightDecelerationForce;

            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.WeakForce;

        }

        #endregion
    }
}
