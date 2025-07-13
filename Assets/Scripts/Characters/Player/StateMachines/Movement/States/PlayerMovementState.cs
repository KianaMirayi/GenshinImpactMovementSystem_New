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

            InitializeData();
        }

        private void InitializeData()
        {
            playerMovementStateMachine.ReusableData.TimeToReachTargetRotation = movementData.baseRotationData.targetRotationReachTime;
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


            Vector3 currentPlayerHorizontalVelocity = GetPlayerHoriztontalVelocity();
            playerMovementStateMachine.Player.Rigidbody.AddForce(movementSpeed * targetRotationDirection - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);

            Debug.Log("Speed: "+movementSpeed); //

            //AddForce���ٶȵĸı䲻��������Ч�ģ���Velocity���ٶȵĸı���������Ч�ģ�һ���Ƽ�ʹ��AddForce
            //ForceMode�е�VelocityChange���ٶȵ����üȲ������ڵ�ǰ��ɫ��������Ҳ��������ʱ��;
            //AddForce�����ǶԵ�ǰ�Ѿ����ڵ���ʩ��Ӱ�죬��VelocityChange��ֱ�������ٶȵı仯��,������ʹ��AddForce֮ǰҪ��ȥ��ҵ�ǰ���˶��������ٶȣ�
        }

        

        private float Rotate(Vector3 targetAngle)
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
