using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputActions { get; private set; }  //创建InputSystem自动生成的脚本的实例

        public PlayerInputActions.PlayerActions PlayerActions { get; private set; } //引用Action Map中的Player

        private void Awake()
        {
            InputActions = new PlayerInputActions(); //实例化InputActions

            PlayerActions = InputActions.Player; //获取Player Action Map  PlayerAction是Action Map(属于结构体类型)的名字，而Player是这个结构体变量的名字
        }


        private void OnEnable()
        {
            InputActions.Enable(); //启用InputActions
        }

        private void OnDisable()
        {
            InputActions.Disable(); //禁用InputActions
        }
    }
} 
