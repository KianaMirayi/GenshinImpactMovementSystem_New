using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    //�����������ĳһ���ƶ�ʱ����һỺ������ת����Բ���˶��˶���������Ϸ��۲�������
    //�����������ǰ�������ʱ������øù���
    //����ͨ�����������HorizontalRencentering��ʵ��
    //HorizontalRencentering����һ�����ٶ��Զ����¾�����������������ÿ�ʼ�Զ����¾���֮ǰ��Ҫ�ȴ���ʱ��
    //���¾��е��ٶȱ���ΪRecentering Time����ʾ�����Ҫ��ò�����ȫ���¾���
    //��ʼ���¾���֮ǰ��ʱ�䱻��ΪWait Time�� ��ʾ�������ʼ���¾���֮ǰ��Ҫ�ȴ����

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
