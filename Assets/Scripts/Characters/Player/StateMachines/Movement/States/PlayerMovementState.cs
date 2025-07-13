using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{

    //负责玩家的移动、旋转以及相机的旋转
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine playerMovementStateMachine;

        protected PlayerGroundedData movementData;

        //protected Vector2 playerMovmentStateMachine.ReusableData.MovementInput;

        

        //protected float baseSpeed = 5f;  //speed in m/s

        //protected float playerMovmentStateMachine.ReusableDataspeed.MovementSpeedModifier = 1f;

        //负责让相机平滑过渡到指定的角度
        /*
        protected Vector3 playerMovmentStateMachine.ReusableData.CurrentTargetRotation;
        protected Vector3 playerMovmentStateMachine.ReusableData.TimeToReachTargetRotation;
        protected Vector3 playerMovmentStateMachine.ReusableData.DampedTargetRotationCurrentVelocity;
        protected Vector3 playerMovmentStateMachine.ReusableData.DampedTargetRotationPassedTime;
        */

        //protected bool playerMovmentStateMachine.ReusableData.ShouldWalk;


        public PlayerMovementState(PlayerMovementStateMachine _playerMovementStateMachine)
        { 
            playerMovementStateMachine = _playerMovementStateMachine;

            movementData = playerMovementStateMachine.Player.Data.GroundedData;

            InitializeData();
        }

        private void InitializeData()
        {
            playerMovementStateMachine.ReusableData.TimeToReachTargetRotation = movementData.baseRotationData.targetRotationReachTime;
        }

        #region IState Methods
        public virtual void Enter()  //注意 这些方法是虚方法，子类可以重写它们
        {
            Debug.Log("State: " + GetType().Name);

            AddInputActionsCallbacks(); //Left Control键控制行走还是奔跑
        }

        

        public virtual void Exit()
        {
            //throw new System.NotImplementedException();
            RemoveInputActionsCallbacks();
        }

        

        public virtual void HandleInput()
        {
            //throw new System.NotImplementedException();
            ReadMovementInput();
        }
     

        public virtual void PhysicsUpdate()
        {
            //throw new System.NotImplementedException();
            Move();
        }

        public virtual void Update()
        {
            //throw new System.NotImplementedException();
        }

        #endregion





        #region Main Methods

        private void ReadMovementInput()  //读取玩家的输入
        {
            playerMovementStateMachine.ReusableData.movementInput = playerMovementStateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }


        private void Move()
        {
            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero || playerMovementStateMachine.ReusableData.movementSpeedModifier == 0f)
            {
                return;
            }

            //当玩家有任何移动的输入时，就调用AddForce()方法，让玩家向该方向以一定的速度移动
            Vector3 movementDirection = GetMovementDirection();

            float targetRotationYAngle = Rotate(movementDirection); //获取目标旋转角度并旋转玩家
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetMovementSpeed();


            Vector3 currentPlayerHorizontalVelocity = GetPlayerHoriztontalVelocity();
            playerMovementStateMachine.Player.Rigidbody.AddForce(movementSpeed * targetRotationDirection - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);

            Debug.Log("Speed: "+movementSpeed); //

            //AddForce对速度的改变不是立即生效的，而Velocity对速度的改变是立即生效的，一般推荐使用AddForce
            //ForceMode中的VelocityChange对速度的设置既不依赖于当前角色的质量，也不依赖于时间;
            //AddForce方法是对当前已经存在的力施加影响，而VelocityChange是直接设置速度的变化量,所以在使用AddForce之前要减去玩家当前的运动向量和速度；
        }

        

        private float Rotate(Vector3 targetAngle)
        {
            float directionAngle = UpdateTargetRotation(targetAngle);

            RotateTowardsTargetRotation();

            return directionAngle;
        }

        

        private void UpdateTargetRotationData(float targetAngle)
        {
            playerMovementStateMachine.ReusableData.CurrentTargetRotation.y = targetAngle; //更新目标旋转角度

            playerMovementStateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f; //重置平滑过渡的时间
        }

        private float AddCameraRotationToAngle(float angle)
        {
            angle += playerMovementStateMachine.Player.MainCameraTransform.eulerAngles.y; //将摄像机的旋转角度添加到方向角度上, 相机的Y轴是平面轴

            if (angle > 360f)
            {
                angle -= 360f; //确保角度在0到360度之间
            }

            return angle;
        }

        private static float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; //计算玩家移动的方向角

            if (directionAngle < 0f)
            {
                directionAngle += 360f; //确保角度在0到360度之间
            }

            return directionAngle;
        }

        #endregion

        #region Reusable Methods
        protected Vector3 GetMovementDirection()
        {
            return new Vector3(playerMovementStateMachine.ReusableData.movementInput.x, 0f, playerMovementStateMachine.ReusableData.movementInput.y);
        }

        protected float GetMovementSpeed()
        { 
            return movementData.baseSpeed * playerMovementStateMachine.ReusableData.movementSpeedModifier * playerMovementStateMachine.ReusableData.movementOnSlopeSpeedModifier;

            
        }


        protected Vector3 GetPlayerHoriztontalVelocity()
        {
            Vector3 playerHorizontalVelocity = playerMovementStateMachine.Player.Rigidbody.velocity;

            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }

        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, playerMovementStateMachine.Player.Rigidbody.velocity.y, 0f);
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = playerMovementStateMachine.Player.transform.eulerAngles.y;

            if (currentYAngle == playerMovementStateMachine.ReusableData.CurrentTargetRotation.y)
            {
                return;
            }

            float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle,playerMovementStateMachine.ReusableData.CurrentTargetRotation.y, ref playerMovementStateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, playerMovementStateMachine.ReusableData.TimeToReachTargetRotation.y - playerMovementStateMachine.ReusableData.DampedTargetRotationPassedTime.y);

            playerMovementStateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothYAngle, 0f);

            playerMovementStateMachine.Player.Rigidbody.MoveRotation(targetRotation);  //旋转玩家
        }

        protected float UpdateTargetRotation(Vector3 targetAngle, bool shouldConsiderCameraRoration = true)
        {
            float directionAngle = GetDirectionAngle(targetAngle);

            if (shouldConsiderCameraRoration)
            { 
                directionAngle = AddCameraRotationToAngle(directionAngle);
                
            }


            if (directionAngle != playerMovementStateMachine.ReusableData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //获取目标旋转方向
        }

        protected void ResetVelocity()
        { 
            playerMovementStateMachine.Player.Rigidbody.velocity = Vector3.zero; //重置玩家的速度
        }


        protected virtual void AddInputActionsCallbacks()
        {
            playerMovementStateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
        }

        

        protected virtual void RemoveInputActionsCallbacks()
        {
            playerMovementStateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
        }

        #endregion


        #region Input Methods

        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ReusableData.shouldWalk = !playerMovementStateMachine.ReusableData.shouldWalk;
        }


        #endregion

    }
}
