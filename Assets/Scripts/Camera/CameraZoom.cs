using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class CameraZoom : MonoBehaviour
    {
        //������������������Զ����
        [SerializeField][Range(0f, 10f)] private float defaultDistance = 6f;
        [SerializeField][Range(0f, 10f)] private float maxDistance = 6f;
        [SerializeField][Range(0f, 10f)] private float minDistance = 1f;

        //����������������ٶȺ�������
        [SerializeField][Range(0f, 10f)] private float smoothing = 4f;
        [SerializeField][Range(0f, 10f)] private float zoomSensitivity = 1f;


        private float currentTargetDistance; //��ǰ�������Ŀ��ľ���

        private CinemachineFramingTransposer framingTransposer; //������Ŀ��ת������������ڿ�����������ӽǺ�λ��

        private CinemachineInputProvider inputProvider;

        private void Awake()
        {
            framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();

            inputProvider = GetComponent<CinemachineInputProvider>();

            currentTargetDistance = defaultDistance;
        }

        private void Update()
        {
            Zoom();  //���������
        }

        private void Zoom()
        {
            float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;

            //Debug.Log(zoomValue);

            currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue,minDistance,maxDistance); 

            float currentDistance = framingTransposer.m_CameraDistance;

            if (currentDistance == currentTargetDistance) //����ǰ����Ŀ���������������Ŀ�����ʱ��
            {
                return;
            }

            float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);

            framingTransposer.m_CameraDistance = lerpedZoomValue;
        }
    }
}
