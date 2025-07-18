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

            cinemachinePOV.m_HorizontalRecentering.CancelRecentering();  //ȡ��������ڽ��е��κ��������¾��в���

            if (waitTime == -1f)  //���õȴ�ʱ��
            {
                waitTime = defaultHorizontalWaitTime;
            }

            if (recenteringTime == -1f) //�������¾���ʱ��
            { 
                recenteringTime = defaultHorizontalRecenteringTime;
            }

            recenteringTime = (baseMovementSpeed * recenteringTime) / movementSpeed;   //ʵ���˶��ٶ�Խ�죬���¾��е�ʱ��Խ��

            cinemachinePOV.m_HorizontalRecentering.m_WaitTime = waitTime;
            cinemachinePOV.m_HorizontalRecentering.m_RecenteringTime = recenteringTime;

        }

        public void DisableRecentering()
        { 
            cinemachinePOV.m_HorizontalRecentering.m_enabled = false;
            
        }
    }
}
