using UnityEngine;

namespace GenshinImpactMovementSystem
{
    /*
     
    ��״̬��ͨ������IState�ӿ�������״̬���л��͸��¡�

    ע��ChangeState������ʹ����?.��������ȷ���ڵ�ǰ״̬��Ϊnullʱ�ŵ���Exit��Enter������

    */

    public abstract class StateMachine  //������
    {
        protected IState currentState;  //��ǰ״̬

        public void ChangeState(IState newState)
        { 
            currentState?.Exit();//�˳���ǰ״̬
            currentState = newState;
            currentState.Enter(); //������״̬
        }

        public void HandleInput()
        {
            currentState?.HandleInput(); //��������
        }

        public void Update()
        {
            currentState?.Update(); //����״̬
        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate(); //�������״̬
        }
    }
}
