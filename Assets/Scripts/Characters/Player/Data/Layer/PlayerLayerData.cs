using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerLayerData
    {
        [field: SerializeField] public LayerMask groundLayer { get; private set; }

        public bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (1 << layer & layerMask) != 0;
        }

        public bool IsGroundLayer(int layer)
        {
            return ContainsLayer(groundLayer, layer);
        }
    }
}
