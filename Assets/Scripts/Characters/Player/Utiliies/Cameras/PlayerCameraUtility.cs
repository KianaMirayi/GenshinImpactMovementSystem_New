using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{

    [Serializable]
    public class PlayerCameraUtility
    {
        [field: SerializeField] public CinemachineVirtualCamera virtualCamera { get; private set; }
        [field: SerializeField] public float defaultHorizontalWaitTime { get; private set; } = 0f;
        [field: SerializeField] public float defaultHorizontalRecenteringTime { get; private set; } = 4f;

        private CinemachinePOV cinemachinePOV;

        public void Initialize()
        { 
            cinemachinePOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        public void EnableRecentering(float waitTime = -1f, float recenteringTime = -1f, float baseMovementSpeed = 1f, float movementSpeed = 1f)
        { 
            cinemachinePOV.m_HorizontalRecentering.m_enabled = true;

            cinemachinePOV.m_HorizontalRecentering.CancelRecentering();  //取消相机正在进行的任何现有重新居中操作

            if (waitTime == -1f)  //设置等待时间
            {
                waitTime = defaultHorizontalWaitTime;
            }

            if (recenteringTime == -1f) //设置重新居中时间
            { 
                recenteringTime = defaultHorizontalRecenteringTime;
            }

            recenteringTime = (baseMovementSpeed * recenteringTime) / movementSpeed;   //实现运动速度越快，重新居中的时间越快

            cinemachinePOV.m_HorizontalRecentering.m_WaitTime = waitTime;
            cinemachinePOV.m_HorizontalRecentering.m_RecenteringTime = recenteringTime;

        }

        public void DisableRecentering()
        { 
            cinemachinePOV.m_HorizontalRecentering.m_enabled = false;
            
        }
    }
}
