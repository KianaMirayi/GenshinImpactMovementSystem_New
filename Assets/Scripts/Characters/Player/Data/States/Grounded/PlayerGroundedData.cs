using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerGroundedData
    {
        [field: SerializeField][field: Range(0f,25f)] public float baseSpeed { get; private set; } = 5f;
        [field: SerializeField][field: Range(0f,5f)] public float GroundToFallRayDistance { get; private set; } = 1f;

        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }  //不同斜率下的速度乘数

        [field: SerializeField] public PlayerRotationData baseRotationData { get; private set; }

        [field: SerializeField] public PlayerWalkData WalkData { get; private set; }

        [field: SerializeField] public PlayerIdleData IdleData { get; private set; }

        [field: SerializeField] public PlayerRunData RunData { get; private set; }

        [field: SerializeField] public PlayerDashData DashData { get; private set; }

        [field: SerializeField] public PlayerSprintData SprintData { get; private set; }

        [field: SerializeField] public PlayerStopData StopData { get; private set; }

        [field: SerializeField] public PlayerRollData RollData { get; private set; }

        [field: SerializeField] public List<PlayerCameraRecenteringData> SidewaysCameraRenteringData { get; private set; }
        [field: SerializeField] public List<PlayerCameraRecenteringData> BackwardsCameraRenteringData { get; private set; }


    }
}
