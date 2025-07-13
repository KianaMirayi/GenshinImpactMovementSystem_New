using System;
using UnityEditor;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]

    //重新计算碰撞体的高度使贴合地形

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

            if (halfColliderHeight < capsuleColliderData.Collider.radius)  //避免当碰撞体的高度小于碰撞体半径的两倍时，碰撞体会向上移动
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

            capsuleColliderData.Collider.center = newColliderCenter; //本地坐标
        }
    }
}
