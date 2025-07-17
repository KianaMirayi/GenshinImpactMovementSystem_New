using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{

    /*
    
    ��״̬�ӿڶ�����״̬����ÿ��״̬����ʵ�ֵķ�����

    ����ӿ��е�ÿһ�������������˸ýӿڵ����ж����뱻ʵ�֣��ҿ������Լ���ʵ�֣�

    */

    public interface IState
    {
        public void Enter();

        public void Exit();

        public void HandleInput();  //Run any logic regarding reading Input

        public void Update();  //Run any non-Physics related Logics

        public void PhysicsUpdate();  //Run any Physics related Logics

        public void OnAnimationEnterEvent();

        public void OnAnimationExitEvent();

        public void OnAnimationTransitaionEvent();  //Used for transition to other states when the animation enters a certain frame
        public void OnTriggerEnterEvent(Collider collider);

        public void OnTriggerExitEvent(Collider collider);
    }
}
