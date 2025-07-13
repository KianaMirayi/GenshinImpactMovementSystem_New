using System;

using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class SlopeData
    {
        [field: SerializeField][field:Range(0f, 1f)] public float stepHeightPercantage { get; private set; } = 0.25f;
        [field: SerializeField][field: Range(0f, 5f)] public float floatRayDistance { get; private set; } = 1.65f;  //����ײ��Height�Դ�һЩ�ͺ�
        [field: SerializeField][field: Range(0f, 50f)] public float stepReachForce { get; private set; } = 25f;  
    }
}
