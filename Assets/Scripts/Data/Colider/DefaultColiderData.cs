using System;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class DefaultColiderData  //������ײ��ĳ�ʼĬ�ϸ߶ȡ����ġ��Ͱ뾶
    {
        [field: SerializeField] public float Height { get; private set; } = 1.62f;
        [field: SerializeField] public float CenterY { get; private set; } = 0.81f;
        [field: SerializeField] public float Radius { get; private set; } = 0.2f;
    }
}
