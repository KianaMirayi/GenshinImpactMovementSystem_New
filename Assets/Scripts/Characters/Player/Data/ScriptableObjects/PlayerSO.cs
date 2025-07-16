using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [CreateAssetMenu(fileName ="Player", menuName ="Custom/Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        //拥有大部分贯穿整个系统的变量和方法
        //Hold individual State Data , such as specific State Speed Modifier or Rotation Reach Time

        [field: SerializeField]public PlayerGroundedData GroundedData { get; private set; }

        [field: SerializeField] public PlayerAirboneData AirboneData { get; private set; }
    }
        
    
}
