using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementDecelerationForce = movementData.StopData.mediunDecelerationForce;
        }
    }
}
