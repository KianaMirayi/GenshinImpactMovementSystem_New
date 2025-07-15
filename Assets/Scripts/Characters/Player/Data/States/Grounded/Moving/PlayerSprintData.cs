using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerSprintData
    {
        [field: SerializeField][field: Range(1f, 3f)] public float sprintSpeedModifier { get; private set; } = 1.7f;
        [field: SerializeField][field: Range(0f, 5f)] public float sprintToRunTime { get; private set; } = 1f; //疾跑状态自动切换到奔跑状态的持续时间
        [field: SerializeField][field: Range(0f, 2f)] public float runToWalkTime { get; private set; } = 0.5f;//奔跑状态自动切换到走路状态的持续时间


    }
}
