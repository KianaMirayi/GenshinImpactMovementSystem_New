using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{

    [RequireComponent(typeof(PlayerInput))]  //ȷ��Player�������PlayerInput������Զ���ӣ�
    public class Player : MonoBehaviour
    {
        private PlayerMovementStateMachine movementStateMachine; 

        public PlayerInput Input { get; private set; }

        public Rigidbody Rigidbody { get; private set; }

        public Animator Animator { get; private set; }

        public Transform MainCameraTransform { get; private set; }  //���������Transform���

        [field: Header("Reference")]
        [field: SerializeField]public PlayerSO Data { get; private set; }  //������ҵ�����

        [field: Header("Collisions")]
        [field: SerializeField] public PlayerCapsuleColliderUtility capsuleColiderUtility { get; private set; }

        [field: Header("Animations")]
        [field :SerializeField] public PlayerAnimationData AnimationData { get; private set; }

        [field: SerializeField] public PlayerLayerData layerData { get; private set; }

        [field: Header("Camera")]
        [field : SerializeField] public PlayerCameraUtility cameraUtility { get; private set; }







         private SkinnedMeshRenderer skinnedMeshRenderer;  //�����õ�SkinnedMeshRenderer���



        private void Awake()
        {
            Input = GetComponent<PlayerInput>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();



            capsuleColiderUtility.Initialize(gameObject);
            capsuleColiderUtility.CalculateCapsuleColliderDimensions();

            cameraUtility.Initialize();

            AnimationData.Initialize();

            MainCameraTransform = Camera.main.transform;

            movementStateMachine = new PlayerMovementStateMachine(this);  //ʵ������ҵ��ƶ�״̬��


               skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();


            

            
        }


        private void OnValidate()
        {
            capsuleColiderUtility.Initialize(gameObject);

            capsuleColiderUtility.CalculateCapsuleColliderDimensions();

            Debug.Log(capsuleColiderUtility.capsuleColliderData.Collider.center.y);
        }
        private void Start()
        {
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);  //��ʼʱ�������״̬



            Debug.Log(skinnedMeshRenderer.bounds.size);
        }

        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnterEvent(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExitEvent(collider);
        }

        private void Update()
        {
            movementStateMachine.HandleInput();  //��������

            movementStateMachine.Update();  //����������Ч���޹ص�����
        }

        private void FixedUpdate()
        {
            movementStateMachine.PhysicsUpdate(); // ����������Ч����ص�����
        }


        public void OnMovementStateAnimationEnterEvent()
        { 
            movementStateMachine.OnAnimationEnterEvent();
        }

        public void OnMovementStateAnimationExitrEvent()
        {
            movementStateMachine.OnAnimationExitEvent();
        }

        public void OnMovementStateAnimationTransitionEvent()
        {
            movementStateMachine.OnAnimationTransitaionEvent();
        }

    }
}
