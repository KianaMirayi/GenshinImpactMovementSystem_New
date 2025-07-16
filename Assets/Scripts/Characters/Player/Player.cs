using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{

    [RequireComponent(typeof(PlayerInput))]  //确保Player组件上有PlayerInput组件（自动添加）
    public class Player : MonoBehaviour
    {
        private PlayerMovementStateMachine movementStateMachine; 

        public PlayerInput Input { get; private set; }

        public Rigidbody Rigidbody { get; private set; }

        public Transform MainCameraTransform { get; private set; }  //主摄像机的Transform组件

        [field: Header("Reference")]
        [field: SerializeField]public PlayerSO Data { get; private set; }  //处理玩家的数据

        [field: Header("Collisions")]
        [field: SerializeField] public CapsuleColiderUtility capsuleColiderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData layerData { get; private set; }







         private SkinnedMeshRenderer skinnedMeshRenderer;  //测试用的SkinnedMeshRenderer组件



        private void Awake()
        {
            capsuleColiderUtility.Initialize(gameObject);

            capsuleColiderUtility.CalculateCapsuleColliderDimensions();

            Input = GetComponent<PlayerInput>();

            movementStateMachine = new PlayerMovementStateMachine(this);  //实例化玩家的移动状态机

            Rigidbody = GetComponent<Rigidbody>();

               skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            MainCameraTransform = Camera.main.transform;

            

            
        }


        private void OnValidate()
        {
            capsuleColiderUtility.Initialize(gameObject);

            capsuleColiderUtility.CalculateCapsuleColliderDimensions();

            Debug.Log(capsuleColiderUtility.capsuleColliderData.Collider.center.y);
        }
        private void Start()
        {
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);  //开始时进入待机状态



            Debug.Log(skinnedMeshRenderer.bounds.size);
        }

        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnterEvent(collider);
        }

        private void Update()
        {
            movementStateMachine.HandleInput();  //处理输入

            movementStateMachine.Update();  //处理与物理效果无关的数据
        }

        private void FixedUpdate()
        {
            movementStateMachine.PhysicsUpdate(); // 处理与物理效果相关的数据
        }


       
    }
}
