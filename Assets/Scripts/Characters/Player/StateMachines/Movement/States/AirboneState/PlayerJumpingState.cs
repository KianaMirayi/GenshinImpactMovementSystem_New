using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerJumpingState : PlayerAirboneState
    {
        public PlayerJumpingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f; //跳跃时玩家不可以移动

            Jump();
        }



        #endregion


        #region Main Methods
        private void Jump()
        {
            Vector3 jumpForce = playerMovementStateMachine.ReusableData.currentJumpForce;  //临时跳跃力

            Vector3 playerForward = playerMovementStateMachine.Player.transform.forward;

            jumpForce.x *= playerForward.x;
            jumpForce.z *= playerForward.z;

            ResetVelocity();//重置速度以避免跳跃当前速度影响
            playerMovementStateMachine.Player.Rigidbody.AddForce(jumpForce,ForceMode.VelocityChange);
        }

        #endregion
    }
}
