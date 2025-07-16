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

        #region Reusable Methods

        protected override void OnContactWithGround(Collider collider)
        { 
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }

        #endregion
    }
}
