using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerStateReusableData
    {

        //�������״̬֮����Ҫ�ظ�ʹ�õ�����,
        //such as shouldWalk and MovementInput
        

        public Vector2 movementInput { get; set; }

        public float movementSpeedModifier { get; set; } = 1f;

        public float movementOnSlopeSpeedModifier { get; set; } = 1f;

        public bool shouldWalk { get; set; }


        private Vector3 currentTargetRotation;
        public ref Vector3 CurrentTargetRotation
        {
            get
            {
                return ref currentTargetRotation;
            }
        }


        private Vector3 timeToReachTargetRotation;
        public ref Vector3 TimeToReachTargetRotation
        {
            get
            {
                return ref timeToReachTargetRotation;
            }
        }


        private Vector3 dampedTargetRotationCurrentVelocity;
        public ref Vector3 DampedTargetRotationCurrentVelocity
        {
            get
            {
                return ref dampedTargetRotationCurrentVelocity;
            }
        }


        private Vector3 dampedTargetRotationPassedTime;
        public ref Vector3 DampedTargetRotationPassedTime
        {
            get 
            {
                return ref dampedTargetRotationPassedTime;
            }
        }
    }
}
