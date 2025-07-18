using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData dashData;

        private float startTime; //记录进入冲刺状态的时间，用于判断连击冲刺

        private int consecutiveDashUsed;  //已冲刺次数

        private bool shouldKeepRotating; //是否需要持续旋转角色朝向，依据输入决定

        public PlayerDashingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            dashData = movementData.DashData;
        }

        #region IState Methods
        public override void Enter() //设置速度修正、旋转参数
        {
            playerMovementStateMachine.ReusableData.movementSpeedModifier = dashData.DashSpeedModifier;

            base.Enter();


            playerMovementStateMachine.ReusableData.currentJumpForce = airboneData.JumpData.StrongForce;


            playerMovementStateMachine.ReusableData.RotationData = dashData.RotationData;

            AddForceOnTransitionFromStationaryState(); //如果角色静止，调用 AddForceOnTransitionFromStationaryState() 给予初始冲刺力。

            shouldKeepRotating = playerMovementStateMachine.ReusableData.movementInput != Vector2.zero;  //当有任何移动的输入时都应该考虑旋转方向

            UpdateConsecutiveDashed();

            startTime = Time.time; //记录进入状态的时间
        }

        public override void OnAnimationTransitaionEvent()
        {

            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.HardStoppingState); //冲刺后如果没有后续输入的话进入停止状态

                return;
            }

            playerMovementStateMachine.ChangeState(playerMovementStateMachine.SprintingState);  //冲刺后有输入的话进入疾跑状态
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!shouldKeepRotating)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }



        

        #endregion



        #region Main Methods
        private void AddForceOnTransitionFromStationaryState()  //如果角色没有移动输入，给予角色一个冲刺方向的速度（沿角色当前朝向
        {
            if (playerMovementStateMachine.ReusableData.movementInput != Vector2.zero)
            {
                return; 
            }

            Vector3 characterRotationDirection = playerMovementStateMachine.Player.transform.forward;

            characterRotationDirection.y = 0f;

            UpdateTargetRotation(characterRotationDirection,false);

            playerMovementStateMachine.Player.Rigidbody.velocity = characterRotationDirection * GetMovementSpeed();
        }

        private void UpdateConsecutiveDashed()
        {
            if (!IsConsecutive())
            {
                consecutiveDashUsed = 0;
            }

            ++consecutiveDashUsed;

            if (consecutiveDashUsed == dashData.ConsecutiveDashesLimitAmount)
            {
                consecutiveDashUsed = 0;
            }

            playerMovementStateMachine.Player.Input.DisableActionFor(playerMovementStateMachine.Player.Input.PlayerActions.Dash,dashData.DashLimitReachedCooldown);
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;  // 判断当前游戏时间是否小于进入进入冲刺时的时间与冲刺时长之和
        }

        #endregion


        #region Input Methods

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            
        }

        /*
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            
        }
        */
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }

        #endregion

        #region Reusable Methods

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
        }

        

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
        }

        #endregion
    }
}
