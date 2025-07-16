using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{

    /*
    
    玩家的移动状态机，继承自StateMachine类。

    玩家的移动状态机并不是一个具体的状态，它是一个状态机容器，包含了玩家的不同移动状态。

    通过这种方法，Player脚本就不用将每个状态都实例化一次，而是通过状态机来管理不同的状态。

     */

    
    public class PlayerMovementStateMachine : StateMachine
    {

        public PlayerStateReusableData ReusableData { get; }

        public Player Player { get; }
        public PlayerIdlingState IdlingState { get; }

        public PlayerDashingState dashingState { get; }

        public PlayerWalkingState WalkingState { get; }

        public PlayerRunningState RunningState { get; }

        public PlayerSprintingState SprintingState { get; }

        public PlayerLightStoppingState LightStoppingState { get; }

        public PlayerMediumStoppingState MediumStoppingState { get; }

        public PlayerHardStoppingState HardStoppingState { get; }

        public PlayerJumpingState JumpingState { get; }


        public PlayerMovementStateMachine(Player _player)  //实例化玩家的具体状态
        { 
            Player = _player;
            ReusableData = new PlayerStateReusableData();


            IdlingState = new PlayerIdlingState(this);
            dashingState = new PlayerDashingState(this);


            WalkingState = new PlayerWalkingState(this);
            RunningState = new PlayerRunningState(this);
            SprintingState = new PlayerSprintingState(this);


            LightStoppingState = new PlayerLightStoppingState(this);
            MediumStoppingState = new PlayerMediumStoppingState(this);
            HardStoppingState = new PlayerHardStoppingState(this);

            JumpingState = new PlayerJumpingState(this);

        }
    }
}
