using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class CapsuleColiderData
    {
        public CapsuleCollider Collider { get; private set; }

        public Vector3 ColliderCenterInLocalSpace { get; private set; }


        public void Initialize(GameObject gameObject) //初始化被传入游戏对象的碰撞体
        {
            if (gameObject == null)
            {
                return;
            }

            Collider = gameObject.GetComponent<CapsuleCollider>();

            if (Collider == null)// 如果不存在则添加新组件 **
            { 
                Collider = gameObject.AddComponent<CapsuleCollider>();
            }

            UpdateColliderData();
        }

        public void UpdateColliderData()
        {
            ColliderCenterInLocalSpace = Collider.center;
        }


    }
}
