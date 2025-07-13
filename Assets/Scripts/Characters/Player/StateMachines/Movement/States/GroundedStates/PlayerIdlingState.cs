using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public PlayerIdlingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {
        }


        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            playerMovementStateMachine.ReusableData.movementSpeedModifier = 0f;

            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            if (playerMovementStateMachine.ReusableData.movementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }

        

        #endregion
    }




}
