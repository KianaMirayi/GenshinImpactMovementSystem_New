using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public SlopeData slopeData { get; private set; }

        public PlayerGroundedState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            slopeData = playerMovementStateMachine.Player.capsuleColiderUtility.slopeData;
        }



        #region IState Methods

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Float();
        }

        #endregion

        #region Main Methods

        private void Float()
        {
            Vector3 capsuleColliderCentterInWorldSpace = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.Collider.bounds.center;  //碰撞体的世界坐标

            Ray downwardsRayFromCapsuleColliderCenter = new Ray(capsuleColliderCentterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleColliderCenter, out RaycastHit hit, slopeData.floatRayDistance, playerMovementStateMachine.Player.layerData.groundLayer,QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal,-downwardsRayFromCapsuleColliderCenter.direction);

                 float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0f)
                { 
                    return;
                }

                float distanceToFloatingPoint = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.ColliderCenterInLocalSpace.y * playerMovementStateMachine.Player.transform.localScale.y - hit.distance ;

                if (distanceToFloatingPoint == 0f)
                { 
                    return;
                }

                float amoutToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amoutToLift, 0f);

                playerMovementStateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }



        }

        private float SetSlopeSpeedModifierOnAngle(float angle)  //使用动画曲线来表达不同斜率地面下移动速度乘数
        {
            float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);

            playerMovementStateMachine.ReusableData.movementOnSlopeSpeedModifier = slopeSpeedModifier;

            return slopeSpeedModifier;

        }

        #endregion

        #region Reusable Methods

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;

            playerMovementStateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
        }

       

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;

            playerMovementStateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
        }

        #endregion


        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);


        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }

        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.dashingState);
        }

        protected virtual void OnMove()
        {
            if (playerMovementStateMachine.ReusableData.shouldWalk)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.WalkingState);

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);
        }

        #endregion
    }
}
