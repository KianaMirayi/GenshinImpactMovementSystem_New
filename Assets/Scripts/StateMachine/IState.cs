using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{

    /*
    
    该状态接口定义了状态机中每个状态所需实现的方法。

    这个接口中的每一个方法在引用了该接口的类中都必须被实现，且可以有自己的实现；

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
