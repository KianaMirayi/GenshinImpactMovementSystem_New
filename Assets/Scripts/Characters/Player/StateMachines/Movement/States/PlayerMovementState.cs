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

        protected PlayerAirboneData airboneData;

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

            airboneData = playerMovementStateMachine.Player.Data.AirboneData;

            InitializeData();

            SetBaseCameraRecenteringData();
        }

       

        private void InitializeData()
        {
            SetBaseRotationData();
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

        

        public virtual void HandleInput()  //HandleInput() 是状态机接口 IState 的标准方法，每帧由状态机调用。
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

        public virtual void OnAnimationEnterEvent()
        {
            //throw new NotImplementedException();
        }

        public virtual void OnAnimationExitEvent()
        {
            //throw new NotImplementedException();
        }

        public virtual void OnAnimationTransitaionEvent()
        {
            //throw new NotImplementedException();
        }

        public virtual void OnTriggerEnterEvent(Collider collider)
        {
            //throw new NotImplementedException();
            if (playerMovementStateMachine.Player.layerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);

                return;
            }
        }

        public void OnTriggerExitEvent(Collider collider)
        {
            //throw new NotImplementedException();
            if (playerMovementStateMachine.Player.layerData.IsGroundLayer(collider.gameObject.layer))
            { 
                OnContactWithGroundExit(collider);

                return;
                
            }

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


            Vector3 currentPlayerHorizontalVelocity = GetPlayerHoriztontalVelocity();//是获得角色“此刻”在地面上的移动速度向量，角色在物理世界中可能因为惯性、外力等原因，当前速度并不等于目标速度，获取当前速度是为了后续做“速度矫正”，确保角色速度能精确响应输入
            playerMovementStateMachine.Player.Rigidbody.AddForce(movementSpeed * targetRotationDirection - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
            //movementSpeed * targetRotationDirection：这是期望的目标速度向量，方向由输入和相机决定，大小由速度参数决定
            //currentPlayerHorizontalVelocity：这是当前的实际速度向量
            //两者做差，得到需要补偿的速度变化量（Δv） => Δv = 目标速度向量 - 当前速度向量，这个 Δv 就是我们希望刚体“立刻”达到的速度变化
            //AddForce(..., ForceMode.VelocityChange) 的作用是瞬时改变刚体速度，不考虑质量和时间，直接将刚体速度加上 Δv
            Debug.Log("Speed: "+movementSpeed); //

            //AddForce对速度的改变不是立即生效的，而Velocity对速度的改变是立即生效的，一般推荐使用AddForce
            //ForceMode中的VelocityChange对速度的设置既不依赖于当前角色的质量，也不依赖于时间;
            //AddForce方法是对当前已经存在的力施加影响，而VelocityChange是直接设置速度的变化量,所以在使用AddForce之前要减去玩家当前的运动向量和速度；
            //角色每帧都会被“拉”到目标速度，响应极其灵敏
            //不会因为物理引擎的阻力、摩擦、质量等因素导致速度漂移或延迟
            //角色的移动完全由输入和参数控制，物理表现高度可控
        }



        private float Rotate(Vector3 targetAngle) //根据目标方向计算目标旋转角度，并平滑旋转角色朝向
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

        //计算目标旋转角度（可选是否叠加相机朝向），并更新状态机中的目标旋转数据
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

        protected void ResetVerticalVelocity()
        { 
            Vector3 playerHorizontalVelocity = GetPlayerHoriztontalVelocity();

            playerMovementStateMachine.Player.Rigidbody.velocity = playerHorizontalVelocity;
        }


        protected virtual void AddInputActionsCallbacks()
        {
            playerMovementStateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;

            playerMovementStateMachine.Player.Input.PlayerActions.Look.started += OnMouseMovementStarted;
            playerMovementStateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceld;
        }

        

        protected virtual void RemoveInputActionsCallbacks()
        {
            playerMovementStateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            playerMovementStateMachine.Player.Input.PlayerActions.Look.started -= OnMouseMovementStarted;
            playerMovementStateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;

            playerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceld;

        }

        

        protected void DecelerateHorizontally()
        { 
            Vector3 playerHoriztontalVelocity = GetPlayerHoriztontalVelocity();  //向玩家当前方向的反方向施加力以达到减速的目的

            playerMovementStateMachine.Player.Rigidbody.AddForce(-playerHoriztontalVelocity * playerMovementStateMachine.ReusableData.movementDecelerationForce, ForceMode.Acceleration);  //Acceleration依赖于时间
            //使用 ForceMode.Acceleration,表示这个力是加速度(单位 m/(s*s))与刚体质量无关
            //物理引擎会在每个 FixedUpdate 里根据这个加速度逐步减小角色的速度,直到趋近于零
            //该方法模拟了摩擦力或刹车力,让角色在没有输入时逐渐减速,而不是瞬间停下
        }

        protected void DecelerateVertically()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            playerMovementStateMachine.Player.Rigidbody.AddForce(-playerVerticalVelocity * playerMovementStateMachine.ReusableData.movementDecelerationForce, ForceMode.Acceleration);
        }

        protected bool IsMovingHorizontally(float minMagnitude = 0.1f)
        {
            Vector3 playerHorizontalVelocity = GetPlayerHoriztontalVelocity();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);

            return playerHorizontalMovement.magnitude < minMagnitude;
            
        }

        protected void SetBaseRotationData()
        {
            playerMovementStateMachine.ReusableData.RotationData = movementData.baseRotationData;
            playerMovementStateMachine.ReusableData.TimeToReachTargetRotation = playerMovementStateMachine.ReusableData.RotationData.targetRotationReachTime;
        }

        protected bool IsMovingUp(float minVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minVelocity;
        }

        protected bool IsMovingDown(float minVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < -minVelocity;
        }

        protected virtual void OnContactWithGround(Collider collider)
        {

        }

        protected virtual void OnContactWithGroundExit(Collider collider)
        {

        }

        protected void UpdataCameraRecenteringState(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero)
            {
                return;
            }

            if (movementInput == Vector2.up)
            { 
                DisableCameraRecentering();

                return;
            }

            float cameraVerticalAngle = playerMovementStateMachine.Player.MainCameraTransform.eulerAngles.x;  //x是相机垂直角度

            if (cameraVerticalAngle >= 270f)
            {
                cameraVerticalAngle -= 360f;
            }


            cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);


            if (movementInput == Vector2.down)
            {
                SetCameraRenteringState(cameraVerticalAngle, playerMovementStateMachine.ReusableData.BackwardsCameraRenteringData);

                return;
            }

            SetCameraRenteringState(cameraVerticalAngle, playerMovementStateMachine.ReusableData.SidewaysCameraRenteringData);
        }

        protected void SetCameraRenteringState(float cameraVerticalAngle, List<PlayerCameraRecenteringData> cameraRenteringData)
        {
            foreach (PlayerCameraRecenteringData recenteringData in cameraRenteringData)
            {
                if (!recenteringData.IsWithInRange(cameraVerticalAngle))
                {
                    continue;
                }

                EnableCameraRecentering(recenteringData.WaitTime, recenteringData.RecenteringTime);

                return;

            }

            DisableCameraRecentering();
        }

        protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
        {
            float movemntSpeed = GetMovementSpeed();

            if (movemntSpeed == 0f)  //避免下面的EnableRecentering方法中除以0
            {
                movemntSpeed = movementData.baseSpeed;
            }

            playerMovementStateMachine.Player.cameraUtility.EnableRecentering(waitTime, recenteringTime, movementData.baseSpeed, movemntSpeed);
        }

        protected void DisableCameraRecentering()
        { 
            playerMovementStateMachine.Player.cameraUtility.DisableRecentering();
        }

        protected void SetBaseCameraRecenteringData()
        {
            playerMovementStateMachine.ReusableData.BackwardsCameraRenteringData = movementData.BackwardsCameraRenteringData;

            playerMovementStateMachine.ReusableData.SidewaysCameraRenteringData = movementData.SidewaysCameraRenteringData;
        }

        #endregion


        #region Input Methods

        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            playerMovementStateMachine.ReusableData.shouldWalk = !playerMovementStateMachine.ReusableData.shouldWalk;
        }


        protected virtual void OnMovementCanceld(InputAction.CallbackContext context)  
        {
            DisableCameraRecentering();
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            UpdataCameraRecenteringState(context.ReadValue<Vector2>());
        }

        private void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
            UpdataCameraRecenteringState(playerMovementStateMachine.ReusableData.movementInput);
        }

        #endregion

    }
}
