using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    //当侧视玩家向某一侧移动时，玩家会缓慢地旋转并以圆周运动运动，在玩家上方观察会很明显
    //而正视玩家向前或者向后时将会禁用该功能
    //可以通过虚拟相机的HorizontalRencentering来实现
    //HorizontalRencentering会以一定的速度自动重新居中相机，并可以设置开始自动重新居中之前需要等待的时间
    //重新居中的速度被称为Recentering Time，表示相机需要多久才能完全重新居中
    //开始重新居中之前的时间被称为Wait Time， 表示在相机开始重新居中之前需要等待多久

    [Serializable]
    public class PlayerCameraRecenteringData
    {
        [field: SerializeField][field: Range(0f, 360f)] public float minAngle { get; private set; }
        [field: SerializeField][field: Range(0f, 360f)] public float maxAngle { get; private set; }
        [field: SerializeField][field: Range(-1f, 20f)] public float WaitTime { get; private set; }
        [field: SerializeField][field: Range(-1f, 20f)] public float RecenteringTime { get; private set; }

        public bool IsWithInRange(float angle)
        { 
            return angle >=minAngle && angle <= maxAngle;
        }
    }
}
