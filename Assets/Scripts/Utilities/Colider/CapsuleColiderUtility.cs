using System;
using UnityEditor;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]

    //���¼�����ײ��ĸ߶�ʹ���ϵ���

    public class CapsuleColiderUtility
    {
        public CapsuleColiderData capsuleColliderData { get; private set; }

        [field: SerializeField]public DefaultColiderData defaultColliderData { get; private set; }

        [field: SerializeField]public SlopeData slopeData { get; private set; }


        public void Initialize(GameObject gameObject)
        {
            if (capsuleColliderData != null)
            {
                return;
            }

            capsuleColliderData = new CapsuleColiderData();


            capsuleColliderData.Initialize(gameObject);
        }

        public void CalculateCapsuleColliderDimensions()
        {
            SetCapsuleColliderRadius(defaultColliderData.Radius);

            SetCapsuleColliderHeight(defaultColliderData.Height * (1f - slopeData.stepHeightPercantage));

            ReCalculateCapsuleColliderCenter();

            float halfColliderHeight = capsuleColliderData.Collider.height / 2f;

            if (halfColliderHeight < capsuleColliderData.Collider.radius)  //���⵱��ײ��ĸ߶�С����ײ��뾶������ʱ����ײ��������ƶ�
            {
                SetCapsuleColliderRadius(halfColliderHeight);
            }

            capsuleColliderData.UpdateColliderData();
        }

        public void SetCapsuleColliderRadius(float radius)
        {
            capsuleColliderData.Collider.radius = radius;

        }
        
        public void SetCapsuleColliderHeight(float height)
        {
            capsuleColliderData.Collider.height = height;
        }

        public void ReCalculateCapsuleColliderCenter()
        {
            float colliderHeightDifference = defaultColliderData.Height - capsuleColliderData.Collider.height;

            Vector3 newColliderCenter = new Vector3(0f,defaultColliderData.CenterY + colliderHeightDifference / 2f, 0f);

            capsuleColliderData.Collider.center = newColliderCenter; //��������
        }
    }
}
