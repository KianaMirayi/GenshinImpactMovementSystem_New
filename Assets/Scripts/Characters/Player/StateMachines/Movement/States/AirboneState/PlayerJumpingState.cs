using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerJumpingState : PlayerAirboneState
    {
        public PlayerJumpingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f; //��Ծʱ��Ҳ������ƶ�

            Jump();
        }



        #endregion


        #region Main Methods
        private void Jump()
        {
            Vector3 jumpForce = playerMovementStateMachine.ReusableData.currentJumpForce;  //��ʱ��Ծ��

            Vector3 playerForward = playerMovementStateMachine.Player.transform.forward;

            jumpForce.x *= playerForward.x;
            jumpForce.z *= playerForward.z;

            ResetVelocity();//�����ٶ��Ա�����Ծ��ǰ�ٶ�Ӱ��
            playerMovementStateMachine.Player.Rigidbody.AddForce(jumpForce,ForceMode.VelocityChange);
        }

        #endregion
    }
}
