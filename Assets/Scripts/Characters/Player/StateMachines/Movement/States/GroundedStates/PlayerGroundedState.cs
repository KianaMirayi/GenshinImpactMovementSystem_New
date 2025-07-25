using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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

        public override void Enter()
        {
            base.Enter();

            StartAnimation(playerMovementStateMachine.Player.AnimationData.GroundedParamaterHash);

            UpdateShouldSprint();

            UpdataCameraRecenteringState(playerMovementStateMachine.ReusableData.movementInput);
        }

        

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            

            Float();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(playerMovementStateMachine.Player.AnimationData.GroundedParamaterHash);
        }

        #endregion

        #region Main Methods

        private void Float()
        {
            Debug.Log("Float() called");
            /*
             	保证角色在地形表面自然移动，避免悬空或穿模。
             	动态适应不同坡度，提升角色与地形的物理交互体验。
             	结合速度修正和浮空力，兼顾动画与物理效果。
              */
            if (!(playerMovementStateMachine.currentState is PlayerGroundedState))
            {
                return;
            }

            Vector3 capsuleColliderCentterInWorldSpace = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.Collider.bounds.center;  // 1. 获取胶囊碰撞体的世界空间中心点

            Ray downwardsRayFromCapsuleColliderCenter = new Ray(capsuleColliderCentterInWorldSpace, Vector3.down); // 2. 从碰撞体中心点向下发射一条射线

            //3. 使用射线检测地面，限定射线距离和检测层
            if (Physics.Raycast(downwardsRayFromCapsuleColliderCenter, out RaycastHit hit, slopeData.floatRayDistance, playerMovementStateMachine.Player.layerData.groundLayer,QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal,-downwardsRayFromCapsuleColliderCenter.direction);// 4. 计算地面法线与角色向下方向的夹角（地面坡度角）

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);// 5. 根据坡度角获取速度修正系数

                if (slopeSpeedModifier == 0f)// 6. 如果修正系数为0，直接返回（不可移动或坡度过大）
                { 
                    return;
                }

                // 7. 计算角色需要浮空的距离（角色底部到地面的距离）
                float distanceToFloatingPoint = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.ColliderCenterInLocalSpace.y * playerMovementStateMachine.Player.transform.localScale.y - hit.distance ;

                if (distanceToFloatingPoint < 0.0001f)// 防止游戏初始化时角色开始抽搐(因为碰撞体嵌到地面被挤出去又掉下来)**
                {
                    return;
                }
                

                // 8. 如果距离为0，说明已经贴合地面，无需处理
                if (distanceToFloatingPoint == 0f)
                { 
                    return;
                }

                

                // 9. 计算需要施加的浮空力（考虑当前竖直速度）
                //slopeData.stepReachForce 一个可调节的浮空力系数，决定角色“贴合地面”时的响应强度。数值越大，角色越快贴合地面，越小则越柔和。
                //计算角色需要被“抬升”或“下压”的目标速度（或力），根据角色与地面的距离和响应系数决定。
                //抵消角色已经拥有的竖直速度，确保施加的力只用于纠正角色与地面的距离，而不是叠加已有速度。
                float amoutToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amoutToLift, 0f);

                playerMovementStateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);// 10. 给角色刚体施加竖直方向的力，使其浮空或贴合地面

            }

            

        }

        private float SetSlopeSpeedModifierOnAngle(float angle)  //使用动画曲线来表达不同斜率地面下移动速度乘数
        {
            float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);

            if (playerMovementStateMachine.ReusableData.movementOnSlopeSpeedModifier != slopeSpeedModifier)
            { 
                playerMovementStateMachine.ReusableData.movementOnSlopeSpeedModifier = slopeSpeedModifier;

                UpdataCameraRecenteringState(playerMovementStateMachine.ReusableData.movementInput);
                
            }


            return slopeSpeedModifier;

        }

        private void UpdateShouldSprint()
        {
            if (!playerMovementStateMachine.ReusableData.shouldSprint)
            {
                return;
            }

            if (playerMovementStateMachine.ReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            playerMovementStateMachine.ReusableData.shouldSprint = false;
        }


        private bool IsThereGroundUnderBeneath()
        {
            BoxCollider groundCheckCollider = playerMovementStateMachine.Player.capsuleColiderUtility.TriggerColliderData.GroundCheckCollider;

            Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;

            Collider[] overlappedGroundColldiers = Physics.OverlapBox(groundColliderCenterInWorldSpace,playerMovementStateMachine.Player.capsuleColiderUtility.TriggerColliderData.GroundCheckColliderExtends,groundCheckCollider.transform.rotation,playerMovementStateMachine.Player.layerData.groundLayer,QueryTriggerInteraction.Ignore);

            return overlappedGroundColldiers.Length > 0;


        }

        #endregion

        #region Reusable Methods

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            //playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;

            playerMovementStateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;

            playerMovementStateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }



        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            //playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;

            playerMovementStateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;

            playerMovementStateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }

        protected override void OnContactWithGroundExit(Collider collider)
        {
            base.OnContactWithGroundExit(collider);

            if (IsThereGroundUnderBeneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - playerMovementStateMachine.Player.capsuleColiderUtility.capsuleColliderData.ColliderVerticalExtents, Vector3.down);

            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, movementData.GroundToFallRayDistance, playerMovementStateMachine.Player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
            { 
                OnFall();
                
            }
        }

        

        protected virtual void OnFall()
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.FallingState);
        }

        #endregion


        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.RunningState);


        }

        /*
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.IdlingState);
        }
        */

        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ChangeState(playerMovementStateMachine.dashingState);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            Debug.Log(" OnJumpStarted");

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.JumpingState);

            
        }

        protected virtual void OnMove()
        {
            if (playerMovementStateMachine.ReusableData.shouldSprint)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.SprintingState);

                return;
            }

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
