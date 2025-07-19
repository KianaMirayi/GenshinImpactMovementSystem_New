using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinImpactMovementSystem
{
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = transform.parent.GetComponent<Player>();
        }

        public void TriggerOnMovementStateAnimationEnterEvent()
        {
            if (IsInAnimationTransition())
            { 
                return;
            }

            player.OnMovementStateAnimationEnterEvent();
        }

        public void TriggerOnMovementSateAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            player.OnMovementStateAnimationExitrEvent();
        }

        public void TriggerOnMovementAnimationTransitionEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }

            player.OnMovementStateAnimationTransitionEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)  //AnimatorÖÐµÄlayer
        { 
            return player.Animator.IsInTransition(layerIndex);
        }
    }
}
