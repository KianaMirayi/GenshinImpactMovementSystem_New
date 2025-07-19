using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{ 
    //从较低的高度坠落进入轻着陆状态
    //从较高的高度坠落进入硬着陆状态或者翻滚状态(取决于是否有输入)

    public class PlayerFallingState : PlayerAirboneState
    {
        private PlayerFallData fallData;

        private Vector3 playerPositionOnEnter;
        public PlayerFallingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            fallData = airboneData.FallData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(playerMovementStateMachine.Player.AnimationData.FallParamaterHash);

            playerPositionOnEnter = playerMovementStateMachine.Player.transform.position;

            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f;

            ResetVerticalVelocity();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
            

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(playerMovementStateMachine.Player.AnimationData.FallParamaterHash);
        }



        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {


        }

        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = playerPositionOnEnter.y - playerMovementStateMachine.Player.transform.position.y;

            if (fallDistance <= airboneData.FallData.minDistanceToBeConsideredHardFall)  //若距离小于最小硬着陆判定距离，则进入轻着陆状态
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.LightLandingState);

                return;
            }

            if (playerMovementStateMachine.ReusableData.shouldWalk && !playerMovementStateMachine.ReusableData.shouldSprint || playerMovementStateMachine.ReusableData.movementInput == Vector2.zero) //若没有读取到输入且walkToggle为false，或者不在急速奔跑状态时坠落，则进入硬着陆状态
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.HardLandingState);

                return;
            }

            
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.RollingState);  //进入翻滚状态
            

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
