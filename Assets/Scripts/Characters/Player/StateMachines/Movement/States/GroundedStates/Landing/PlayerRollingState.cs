using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerRollingState : PlayerLandingState
    {  //从高处坠落且有输入时进入翻滚状态，在翻滚状态下可以一边滚动一边向指定方向移动
       //翻滚可以进入步行、跑步、冲刺以及中等停止
       //当翻滚动画的最后一帧没有接收到输入的话，将进入中等停止状态；
       //当翻滚动画的最后一帧接收到输入的话，将根据walkToggle来判断是进入步行还是跑步
       //翻滚动画的任意时刻都允许冲刺
       //翻滚状态中不允许跳跃


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


            playerMovementStateMachine.ReusableData.shouldSprint = false; //翻滚结束之后不允许快速奔跑
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

        public override void OnAnimationTransitaionEvent()  //在翻滚动画最后一帧调用
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
