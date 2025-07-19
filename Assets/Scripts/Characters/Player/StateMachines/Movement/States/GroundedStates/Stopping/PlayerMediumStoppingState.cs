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

            StartAnimation(playerMovementStateMachine.Player.AnimationData.MediumStopParamaterHash);

            playerMovementStateMachine.ReusableData.movementDecelerationForce = movementData.StopData.mediunDecelerationForce;

            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.MediumForce;

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(playerMovementStateMachine.Player.AnimationData.MediumStopParamaterHash);
        }
    }
}
