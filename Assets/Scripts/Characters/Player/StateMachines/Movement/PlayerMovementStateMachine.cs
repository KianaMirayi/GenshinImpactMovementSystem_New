using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{

    /*
    
    ��ҵ��ƶ�״̬�����̳���StateMachine�ࡣ

    ��ҵ��ƶ�״̬��������һ�������״̬������һ��״̬����������������ҵĲ�ͬ�ƶ�״̬��

    ͨ�����ַ�����Player�ű��Ͳ��ý�ÿ��״̬��ʵ����һ�Σ�����ͨ��״̬��������ͬ��״̬��

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


        public PlayerMovementStateMachine(Player _player)  //ʵ������ҵľ���״̬
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
