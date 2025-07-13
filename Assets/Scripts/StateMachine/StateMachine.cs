using UnityEngine;

namespace GenshinImpactMovementSystem
{
    /*
     
    该状态机通过引用IState接口来管理状态的切换和更新。

    注意ChangeState方法中使用了?.操作符来确保在当前状态不为null时才调用Exit和Enter方法。

    */

    public abstract class StateMachine  //抽象类
    {
        protected IState currentState;  //当前状态

        public void ChangeState(IState newState)
        { 
            currentState?.Exit();//退出当前状态
            currentState = newState;
            currentState.Enter(); //进入新状态
        }

        public void HandleInput()
        {
            currentState?.HandleInput(); //处理输入
        }

        public void Update()
        {
            currentState?.Update(); //更新状态
        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate(); //物理更新状态
        }
    }
}
