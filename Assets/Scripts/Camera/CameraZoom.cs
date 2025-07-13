using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class CameraZoom : MonoBehaviour
    {
        //控制摄像机的最近和最远距离
        [SerializeField][Range(0f, 10f)] private float defaultDistance = 6f;
        [SerializeField][Range(0f, 10f)] private float maxDistance = 6f;
        [SerializeField][Range(0f, 10f)] private float minDistance = 1f;

        //控制摄像机的缩放速度和灵敏度
        [SerializeField][Range(0f, 10f)] private float smoothing = 4f;
        [SerializeField][Range(0f, 10f)] private float zoomSensitivity = 1f;


        private float currentTargetDistance; //当前摄像机与目标的距离

        private CinemachineFramingTransposer framingTransposer; //摄像机的框架转置器组件，用于控制摄像机的视角和位置

        private CinemachineInputProvider inputProvider;

        private void Awake()
        {
            framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();

            inputProvider = GetComponent<CinemachineInputProvider>();

            currentTargetDistance = defaultDistance;
        }

        private void Update()
        {
            Zoom();  //缩放摄像机
        }

        private void Zoom()
        {
            float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;

            //Debug.Log(zoomValue);

            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue,minDistance,maxDistance); 

            float currentDistance = framingTransposer.m_CameraDistance;

            if (currentDistance == currentTargetDistance) //当当前距离目标距离等于摄像机离目标距离时；
            {
                return;
            }

            float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);

            framingTransposer.m_CameraDistance = lerpedZoomValue;
        }
    }
}
