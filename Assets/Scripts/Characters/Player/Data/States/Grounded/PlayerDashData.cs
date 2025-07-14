using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerDashData
    {
        [field: SerializeField][field: Range(1f, 3f)] public float DashSpeedModifier { get; private set; } = 2f;
        [field: SerializeField][field: Range(0f, 2f)] public float TimeToBeConsideredConsecutive { get; private set; } = 1f; //冲刺时间
        [field: SerializeField][field: Range(1, 10)] public int ConsecutiveDashesLimitAmount { get; private set; } = 2;  //可连续冲刺次数
        [field: SerializeField][field: Range(1f, 5f)] public float DashLimitReachedCooldown { get; private set; } = 1.75f;
    }
}
