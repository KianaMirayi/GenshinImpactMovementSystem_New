using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputActions { get; private set; }  //����InputSystem�Զ����ɵĽű���ʵ��

        public PlayerInputActions.PlayerActions PlayerActions { get; private set; } //����Action Map�е�Player

        private void Awake()
        {
            InputActions = new PlayerInputActions(); //ʵ����InputActions

            PlayerActions = InputActions.Player; //��ȡPlayer Action Map  PlayerAction��Action Map(���ڽṹ������)�����֣���Player������ṹ�����������
        }


        private void OnEnable()
        {
            InputActions.Enable(); //����InputActions
        }

        private void OnDisable()
        {
            InputActions.Disable(); //����InputActions
        }

        public void DisableActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action,seconds));

        }

        private IEnumerator DisableAction(InputAction action, float seconds)
        { 
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }
    }
} 
