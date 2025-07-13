using System;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class DefaultColiderData  //包含碰撞体的初始默认高度、中心、和半径
    {
        [field: SerializeField] public float Height { get; private set; } = 1.62f;
        [field: SerializeField] public float CenterY { get; private set; } = 0.81f;
        [field: SerializeField] public float Radius { get; private set; } = 0.2f;
    }
}
