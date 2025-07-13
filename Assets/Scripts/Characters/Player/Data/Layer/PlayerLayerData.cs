using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerLayerData
    {
        [field: SerializeField] public LayerMask groundLayer { get; private set; }
    }
}
