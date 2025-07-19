using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(playerMovementStateMachine.Player.AnimationData.LandingParamaterHash);
        }



        public override void Exit()
        {
            base.Exit();

            StopAnimation(playerMovementStateMachine.Player.AnimationData.LandingParamaterHash);
        }
    }


}
