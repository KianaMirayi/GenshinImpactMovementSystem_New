using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{ 
    //�ӽϵ͵ĸ߶�׹���������½״̬
    //�ӽϸߵĸ߶�׹�����Ӳ��½״̬���߷���״̬(ȡ�����Ƿ�������)

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

            if (fallDistance <= airboneData.FallData.minDistanceToBeConsideredHardFall)  //������С����СӲ��½�ж����룬���������½״̬
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.LightLandingState);

                return;
            }

            if (playerMovementStateMachine.ReusableData.shouldWalk && !playerMovementStateMachine.ReusableData.shouldSprint || playerMovementStateMachine.ReusableData.movementInput == Vector2.zero) //��û�ж�ȡ��������walkToggleΪfalse�����߲��ڼ��ٱ���״̬ʱ׹�䣬�����Ӳ��½״̬
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.HardLandingState);

                return;
            }

            
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.RollingState);  //���뷭��״̬
            

        }

        #endregion

        #region Main Methods
        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if (playerVerticalVelocity.y >= -fallData.FallSpeedLimit) //�½�ʱ�������Ǹ�ֵ�����½�ʱ��������������ʱ����
            {
                return;
            }

            Vector3 limitedVelocity = new Vector3(0f, -fallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            playerMovementStateMachine.Player.Rigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
        }

        #endregion
    }
}
