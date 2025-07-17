using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerFallingState : PlayerAirboneState
    {
        public PlayerFallData fallData;
        public PlayerFallingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            fallData = airboneData.FallData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f;

            ResetVerticalVelocity();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
            

        }



        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {


        }
        #endregion

        #region Main Methods
        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if (playerVerticalVelocity.y >= -fallData.FallSpeedLimit) //下降时的向量是负值，当下降时的向量大于限制时返回
            {
                return;
            }

            Vector3 limitedVelocity = new Vector3(0f, -fallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            playerMovementStateMachine.Player.Rigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
        }

        #endregion
    }
}
