using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class CapsuleColiderData
    {
        public CapsuleCollider Collider { get; private set; }

        public Vector3 ColliderCenterInLocalSpace { get; private set; }


        public void Initialize(GameObject gameObject) //��ʼ����������Ϸ�������ײ��
        {
            if (gameObject == null)
            {
                return;
            }

            Collider = gameObject.GetComponent<CapsuleCollider>();

            if (Collider == null)// ������������������� **
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
