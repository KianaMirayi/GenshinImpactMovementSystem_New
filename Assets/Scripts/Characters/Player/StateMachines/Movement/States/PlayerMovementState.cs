using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{

    //������ҵ��ƶ�����ת�Լ��������ת
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine playerMovementStateMachine;

        protected PlayerGroundedData movementData;

        protected PlayerAirboneData airboneData;

        //protected Vector2 playerMovmentStateMachine.ReusableData.MovementInput;

        

        //protected float baseSpeed = 5f;  //speed in m/s

        //protected float playerMovmentStateMachine.ReusableDataspeed.MovementSpeedModifier = 1f;

        //���������ƽ�����ɵ�ָ���ĽǶ�
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
        public virtual void Enter()  //ע�� ��Щ�������鷽�������������д����
        {
            Debug.Log("State: " + GetType().Name);

            AddInputActionsCallbacks(); //Left Control���������߻��Ǳ���
        }

        

        public virtual void Exit()
        {
            //throw new System.NotImplementedException();
            RemoveInputActionsCallbacks();
        }

        

        public virtual void HandleInput()  //HandleInput() ��״̬���ӿ� IState �ı�׼������ÿ֡��״̬�����á�
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

        private void ReadMovementInput()  //��ȡ��ҵ�����
        {
            playerMovementStateMachine.ReusableData.movementInput = playerMovementStateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }


        private void Move()
        {
            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero || playerMovementStateMachine.ReusableData.movementSpeedModifier == 0f)
            {
                return;
            }

            //��������κ��ƶ�������ʱ���͵���AddForce()�������������÷�����һ�����ٶ��ƶ�
            Vector3 movementDirection = GetMovementDirection();

            float targetRotationYAngle = Rotate(movementDirection); //��ȡĿ����ת�ǶȲ���ת���
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetMovementSpeed();


            Vector3 currentPlayerHorizontalVelocity = GetPlayerHoriztontalVelocity();//�ǻ�ý�ɫ���˿̡��ڵ����ϵ��ƶ��ٶ���������ɫ�����������п�����Ϊ���ԡ�������ԭ�򣬵�ǰ�ٶȲ�������Ŀ���ٶȣ���ȡ��ǰ�ٶ���Ϊ�˺��������ٶȽ�������ȷ����ɫ�ٶ��ܾ�ȷ��Ӧ����
            playerMovementStateMachine.Player.Rigidbody.AddForce(movementSpeed * targetRotationDirection - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
            //movementSpeed * targetRotationDirection������������Ŀ���ٶ�����������������������������С���ٶȲ�������
            //currentPlayerHorizontalVelocity�����ǵ�ǰ��ʵ���ٶ�����
            //��������õ���Ҫ�������ٶȱ仯������v�� => ��v = Ŀ���ٶ����� - ��ǰ�ٶ���������� ��v ��������ϣ�����塰���̡��ﵽ���ٶȱ仯
            //AddForce(..., ForceMode.VelocityChange) ��������˲ʱ�ı�����ٶȣ�������������ʱ�䣬ֱ�ӽ������ٶȼ��� ��v
            Debug.Log("Speed: "+movementSpeed); //

            //AddForce���ٶȵĸı䲻��������Ч�ģ���Velocity���ٶȵĸı���������Ч�ģ�һ���Ƽ�ʹ��AddForce
            //ForceMode�е�VelocityChange���ٶȵ����üȲ������ڵ�ǰ��ɫ��������Ҳ��������ʱ��;
            //AddForce�����ǶԵ�ǰ�Ѿ����ڵ���ʩ��Ӱ�죬��VelocityChange��ֱ�������ٶȵı仯��,������ʹ��AddForce֮ǰҪ��ȥ��ҵ�ǰ���˶��������ٶȣ�
            //��ɫÿ֡���ᱻ��������Ŀ���ٶȣ���Ӧ��������
            //������Ϊ���������������Ħ�������������ص����ٶ�Ư�ƻ��ӳ�
            //��ɫ���ƶ���ȫ������Ͳ������ƣ�������ָ߶ȿɿ�
        }



        private float Rotate(Vector3 targetAngle) //����Ŀ�귽�����Ŀ����ת�Ƕȣ���ƽ����ת��ɫ����
        {
            float directionAngle = UpdateTargetRotation(targetAngle);

            RotateTowardsTargetRotation();

            return directionAngle;
        }

        

        private void UpdateTargetRotationData(float targetAngle)
        {
            playerMovementStateMachine.ReusableData.CurrentTargetRotation.y = targetAngle; //����Ŀ����ת�Ƕ�

            playerMovementStateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f; //����ƽ�����ɵ�ʱ��
        }

        private float AddCameraRotationToAngle(float angle)
        {
            angle += playerMovementStateMachine.Player.MainCameraTransform.eulerAngles.y; //�����������ת�Ƕ���ӵ�����Ƕ���, �����Y����ƽ����

            if (angle > 360f)
            {
                angle -= 360f; //ȷ���Ƕ���0��360��֮��
            }

            return angle;
        }

        private static float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; //��������ƶ��ķ����

            if (directionAngle < 0f)
            {
                directionAngle += 360f; //ȷ���Ƕ���0��360��֮��
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

            playerMovementStateMachine.Player.Rigidbody.MoveRotation(targetRotation);  //��ת���
        }

        //����Ŀ����ת�Ƕȣ���ѡ�Ƿ����������򣩣�������״̬���е�Ŀ����ת����
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
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //��ȡĿ����ת����
        }

        protected void ResetVelocity()
        { 
            playerMovementStateMachine.Player.Rigidbody.velocity = Vector3.zero; //������ҵ��ٶ�
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
            Vector3 playerHoriztontalVelocity = GetPlayerHoriztontalVelocity();  //����ҵ�ǰ����ķ�����ʩ�����Դﵽ���ٵ�Ŀ��

            playerMovementStateMachine.Player.Rigidbody.AddForce(-playerHoriztontalVelocity * playerMovementStateMachine.ReusableData.movementDecelerationForce, ForceMode.Acceleration);  //Acceleration������ʱ��
            //ʹ�� ForceMode.Acceleration,��ʾ������Ǽ��ٶ�(��λ m/(s*s))����������޹�
            //�����������ÿ�� FixedUpdate �����������ٶ��𲽼�С��ɫ���ٶ�,ֱ����������
            //�÷���ģ����Ħ������ɲ����,�ý�ɫ��û������ʱ�𽥼���,������˲��ͣ��
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

            float cameraVerticalAngle = playerMovementStateMachine.Player.MainCameraTransform.eulerAngles.x;  //x�������ֱ�Ƕ�

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

            if (movemntSpeed == 0f)  //���������EnableRecentering�����г���0
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
