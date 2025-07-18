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

            OnInitalize();
        }

        protected virtual void OnInitalize()
        { 
            
        }


        public void CalculateCapsuleColliderDimensions() //根据默认参数和坡度数据，重新计算并设置碰撞体的半径、高度和中心点
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

        public void ReCalculateCapsuleColliderCenter() //根据高度变化，重新计算碰撞体的中心点，保证碰撞体始终贴合角色底部
        {
            float colliderHeightDifference = defaultColliderData.Height - capsuleColliderData.Collider.height;

            Vector3 newColliderCenter = new Vector3(0f,defaultColliderData.CenterY + colliderHeightDifference / 2f, 0f);

            capsuleColliderData.Collider.center = newColliderCenter; //本地坐标
        }
    }
}
