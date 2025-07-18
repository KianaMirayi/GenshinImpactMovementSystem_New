using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerStateReusableData
    {

        //负责各种状态之间需要重复使用的数据,
        //such as shouldWalk and MovementInput
        

        public Vector2 movementInput { get; set; }

        public float movementSpeedModifier { get; set; } = 1f;

        public float movementOnSlopeSpeedModifier { get; set; } = 1f;

        public float movementDecelerationForce { get; set; } = 1f;

        public bool shouldWalk { get; set; }

        public bool shouldSprint { get; set; }

         public List<PlayerCameraRecenteringData> BackwardsCameraRenteringData { get; set; }
         public List<PlayerCameraRecenteringData> SidewaysCameraRenteringData { get; set; }



        //角色朝向控制。每次移动或旋转时，目标角度会被更新，角色会平滑旋转到该角度
        private Vector3 currentTargetRotation;
        public ref Vector3 CurrentTargetRotation //当前目标旋转角度（通常只用 y 分量）
        {
            get
            {
                return ref currentTargetRotation;
            }
        }


        //达到目标旋转所需的时间
        private Vector3 timeToReachTargetRotation;
        public ref Vector3 TimeToReachTargetRotation //用于平滑旋转的插值计算，决定旋转过渡的快慢
        {
            get
            {
                return ref timeToReachTargetRotation;
            }
        }


        //当前旋转的阻尼速度（用于平滑插值）
        private Vector3 dampedTargetRotationCurrentVelocity;
        public ref Vector3 DampedTargetRotationCurrentVelocity //配合 Mathf.SmoothDampAngle 等方法，实现平滑旋转时的速度跟踪
        {
            get
            {
                return ref dampedTargetRotationCurrentVelocity;
            }
        }


        //旋转插值已用时间
        private Vector3 dampedTargetRotationPassedTime;
        public ref Vector3 DampedTargetRotationPassedTime //记录当前旋转平滑过渡已消耗的时间，辅助插值计算
        {
            get 
            {
                return ref dampedTargetRotationPassedTime;
            }
        }

        public Vector3 currentJumpForce { get; set; }

        public PlayerRotationData RotationData { get; set; }
    }
}
