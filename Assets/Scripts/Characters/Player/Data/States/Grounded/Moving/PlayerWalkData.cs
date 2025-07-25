using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerWalkData
    {
        [field: SerializeField][field: Range(0f, 1f)] public float speedModifier { get; private set; } = 0.225f;
        [field: SerializeField] public List<PlayerCameraRecenteringData> BackwardsCameraRenteringData { get; private set; }


    }
}
