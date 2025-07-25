using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerCapsuleColliderUtility : CapsuleColiderUtility
    {
        [field: SerializeField] public PlayerTriggerColliderData TriggerColliderData { get; private set; }


        protected override void OnInitalize()
        {
            base.OnInitalize();

            TriggerColliderData.Initialize();
        }
    }
}
