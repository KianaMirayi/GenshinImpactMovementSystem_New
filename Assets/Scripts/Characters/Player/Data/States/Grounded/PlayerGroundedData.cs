using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerGroundedData
    {
        [field: SerializeField][field: Range(0f,25f)] public float baseSpeed { get; private set; } = 5f;

        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }  //��ͬб���µ��ٶȳ���

        [field: SerializeField] public PlayerRotationData baseRotationData { get; private set; }

        [field: SerializeField] public PlayerWalkData WalkData { get; private set; }

        [field: SerializeField] public PlayerRunData RunData { get; private set; }

        [field: SerializeField] public PlayerDashData DashData { get; private set; }

        [field: SerializeField] public PlayerSprintData SprintData { get; private set; }

        [field: SerializeField] public PlayerStopData StopData { get; private set; }


    }
}
