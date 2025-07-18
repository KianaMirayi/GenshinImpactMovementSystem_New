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

        public float movementDecelerationForce { get; set; } = 1f;

        public bool shouldWalk { get; set; }

        public bool shouldSprint { get; set; }

         public List<PlayerCameraRecenteringData> BackwardsCameraRenteringData { get; set; }
         public List<PlayerCameraRecenteringData> SidewaysCameraRenteringData { get; set; }



        //��ɫ������ơ�ÿ���ƶ�����תʱ��Ŀ��ǶȻᱻ���£���ɫ��ƽ����ת���ýǶ�
        private Vector3 currentTargetRotation;
        public ref Vector3 CurrentTargetRotation //��ǰĿ����ת�Ƕȣ�ͨ��ֻ�� y ������
        {
            get
            {
                return ref currentTargetRotation;
            }
        }


        //�ﵽĿ����ת�����ʱ��
        private Vector3 timeToReachTargetRotation;
        public ref Vector3 TimeToReachTargetRotation //����ƽ����ת�Ĳ�ֵ���㣬������ת���ɵĿ���
        {
            get
            {
                return ref timeToReachTargetRotation;
            }
        }


        //��ǰ��ת�������ٶȣ�����ƽ����ֵ��
        private Vector3 dampedTargetRotationCurrentVelocity;
        public ref Vector3 DampedTargetRotationCurrentVelocity //��� Mathf.SmoothDampAngle �ȷ�����ʵ��ƽ����תʱ���ٶȸ���
        {
            get
            {
                return ref dampedTargetRotationCurrentVelocity;
            }
        }


        //��ת��ֵ����ʱ��
        private Vector3 dampedTargetRotationPassedTime;
        public ref Vector3 DampedTargetRotationPassedTime //��¼��ǰ��תƽ�����������ĵ�ʱ�䣬������ֵ����
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
