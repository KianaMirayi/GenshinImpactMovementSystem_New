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

        public Transform MainCameraTransform { get; private set; }  //���������Transform���

        [field: Header("Reference")]
        [field: SerializeField]public PlayerSO Data { get; private set; }  //������ҵ�����

        [field: Header("Collisions")]
        [field: SerializeField] public CapsuleColiderUtility capsuleColiderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData layerData { get; private set; }







         private SkinnedMeshRenderer skinnedMeshRenderer;  //�����õ�SkinnedMeshRenderer���



        private void Awake()
        {
            capsuleColiderUtility.Initialize(gameObject);

            capsuleColiderUtility.CalculateCapsuleColliderDimensions();

            Input = GetComponent<PlayerInput>();

            movementStateMachine = new PlayerMovementStateMachine(this);  //ʵ������ҵ��ƶ�״̬��

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
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);  //��ʼʱ�������״̬



            Debug.Log(skinnedMeshRenderer.bounds.size);
        }

        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnterEvent(collider);
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


       
    }
}
