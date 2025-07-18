using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerRollingState : PlayerLandingState
    {  //�Ӹߴ�׹����������ʱ���뷭��״̬���ڷ���״̬�¿���һ�߹���һ����ָ�������ƶ�
       //�������Խ��벽�С��ܲ�������Լ��е�ֹͣ
       //���������������һ֡û�н��յ�����Ļ����������е�ֹͣ״̬��
       //���������������һ֡���յ�����Ļ���������walkToggle���ж��ǽ��벽�л����ܲ�
       //��������������ʱ�̶�������
       //����״̬�в�������Ծ


        private PlayerRollData rollData;
        public PlayerRollingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
            rollData = movementData.RollData;
        }

        #region IState Methods

        public override void Enter()
        {
            playerMovementStateMachine.ReusableData.movementSpeedModifier = rollData.RollSpeedModifier;

            base.Enter();


            playerMovementStateMachine.ReusableData.shouldSprint = false; //��������֮��������ٱ���
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (playerMovementStateMachine.ReusableData.movementInput != Vector2.zero)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitaionEvent()  //�ڷ����������һ֡����
        {
            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                playerMovementStateMachine.ChangeState(playerMovementStateMachine.MediumStoppingState);

                return;
            }

            OnMove();
        }



        #endregion

        #region
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {

        }

        #endregion
    }
}
