using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerStopData 
    {
        [field: SerializeField][field: Range(0f, 15f)] public float lightDecelerationForce { get; private set; } = 5f;
        [field: SerializeField][field: Range(0f, 15f)] public float mediunDecelerationForce { get; private set; } = 6.5f;
        [field: SerializeField][field: Range(0f, 15f)] public float hardDecelerationForce { get; private set; } = 5f;
    }
}
