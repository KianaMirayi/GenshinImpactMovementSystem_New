using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    [Serializable]
    public class PlayerAnimationData
    {


        [Header("State Group Paramater Names")]
        [field: SerializeField] private string groundParamataerName = "Grounded";
        [field: SerializeField] private string movingParamataerName = "Moving";
        [field: SerializeField] private string stoppingParamataerName = "Stopping";
        [field: SerializeField] private string landingParamataerName = "Landing";
        [field: SerializeField] private string airboneParamataerName = "Airbone";

        [Header("Grounded Paramater Names")]
        [field: SerializeField] private string idleParamaterName = "isIdling";
        [field: SerializeField] private string walkParamaterName = "isWalking";
        [field: SerializeField] private string runParamataerName = "isRunning";
        [field: SerializeField] private string sprintParamataerName = "isSprinting";
        [field: SerializeField] private string mediumStopParamataerName = "isMediumStopping";
        [field: SerializeField] private string hardStopParamataerName = "isHardingStopping";
        [field: SerializeField] private string hardLandParamataerName = "isHardLanding";
        [field: SerializeField] private string rollParamataerName = "isRolling";
        [field: SerializeField] private string dashParamataerName = "isDashing";


        [Header("Airbone Paramater Names")]
        [field: SerializeField] private string fallParamataerName = "isFalling";

        public int GroundedParamaterHash { get; private set; }
        public int MovingParamaterHash { get; private set; }
        public int StoppingParamaterHash { get; private set; }
        public int LandingParamaterHash { get; private set; }
        public int AirboneParamaterHash { get; private set; }


        public int IdleParamaterHash { get; private set; }
        public int WalkParamaterHash { get; private set; }
        public int RunParamaterHash { get; private set; }
        public int SprintParamaterHash { get; private set; }
        public int MediumStopParamaterHash { get; private set; }
        public int HardStopParamaterHash { get; private set; }
        public int HardLandParamaterHash { get; private set; }
        public int RollParamaterHash { get; private set; }
        public int DashParamaterHash { get; private set; }
        public int FallParamaterHash { get; private set; }



        public void Initialize()
        {
            GroundedParamaterHash = Animator.StringToHash(groundParamataerName);
            MovingParamaterHash = Animator.StringToHash(movingParamataerName);
            StoppingParamaterHash = Animator.StringToHash(stoppingParamataerName);
            LandingParamaterHash = Animator.StringToHash(landingParamataerName);
            AirboneParamaterHash = Animator.StringToHash(airboneParamataerName);

            IdleParamaterHash = Animator.StringToHash(idleParamaterName);
            WalkParamaterHash = Animator.StringToHash(walkParamaterName);
            RunParamaterHash = Animator.StringToHash(runParamataerName);
            SprintParamaterHash = Animator.StringToHash(sprintParamataerName);
            MediumStopParamaterHash = Animator.StringToHash(mediumStopParamataerName);
            HardStopParamaterHash = Animator.StringToHash(hardStopParamataerName);
            HardLandParamaterHash = Animator.StringToHash(hardLandParamataerName);
            RollParamaterHash = Animator.StringToHash(rollParamataerName);
            DashParamaterHash = Animator.StringToHash(dashParamataerName);

            FallParamaterHash = Animator.StringToHash(fallParamataerName);

        }


    }
}
