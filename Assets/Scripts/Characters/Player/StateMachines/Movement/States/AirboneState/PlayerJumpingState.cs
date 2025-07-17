using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace GenshinImpactMovementSystem
{
    public class PlayerJumpingState : PlayerAirboneState
    {

        private bool shouldKeepRotating;

        private bool canStartingFalling;

        private PlayerJumpData jumpData;


        public PlayerJumpingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            jumpData = airboneData.JumpData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f; //跳跃时玩家不可以移动

            playerMovementStateMachine.ReusableData.RotationData = jumpData.RotationData;

            playerMovementStateMachine.ReusableData.movementDecelerationForce = jumpData.DecelerationForce;

            shouldKeepRotating = playerMovementStateMachine.ReusableData.movementInput != Vector2.zero;  //如果在跳跃状态中仍有输入，则应该旋转

            Jump();

        }

        public override void Update()
        {
            base.Update();

            if (!canStartingFalling && IsMovingUp(0f))
            { 
                canStartingFalling = true;
            }

            if (!canStartingFalling || GetPlayerVerticalVelocity().y > 0f)
            {
                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.FallingState);  //只要垂直方向的向量为负值，就认定为坠落状态；
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            shouldKeepRotating = playerMovementStateMachine.ReusableData.movementInput != Vector2.zero;  //如果在跳跃状态中仍有输入，则应该旋转**

            if (shouldKeepRotating)
            {
                UpdateTargetRotation(GetMovementDirection());//**
                RotateTowardsTargetRotation();
            }

            if (IsMovingUp())
            { 
                DecelerateVertically();
            }


        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            canStartingFalling = false;
        }



        #endregion


        #region Main Methods
        private void Jump()
        {
            Vector3 jumpForce = playerMovementStateMachine.ReusableData.currentJumpForce;  //临时跳跃力

            Vector3 jumpDirection = playerMovementStateMachine.Player.transform.forward;

            if (shouldKeepRotating)
            {
                jumpDirection = GetTargetRotationDirection(playerMovementStateMachine.ReusableData.CurrentTargetRotation.y);
            }

            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            Vector3 capsuleColliderCenterInWorldSpace = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, jumpData.JumpToGroundRayDistance, playerMovementStateMachine.Player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if (IsMovingUp())
                {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;

                    Debug.Log("forceModifier : " + forceModifier + "groudAngle: " + groundAngle );
                }

                if (IsMovingDown())
                { 
                    float forceModifier = jumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;

                    Debug.Log("forceModifier : " + forceModifier + "groudAngle: " + groundAngle);

                }
            }

            ResetVelocity();//重置速度以避免跳跃当前速度影响

            playerMovementStateMachine.Player.Rigidbody.AddForce(jumpForce,ForceMode.VelocityChange);
        }

        #endregion

        #region Reusable methods
        protected override void ResetSprintState()
        {
            

        }

        #endregion
    }
}
