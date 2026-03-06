using System;
using UnityEngine;
using UnityEngine.UI;

namespace CEOGame.Core
{
    public class EmployeeAnimator : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] Image employeeImage;

        public event Action OnWalkInComplete;
        public event Action OnWalkOutComplete;

        public void SetEmployeeSprite(Sprite sprite)
        {
            if (employeeImage != null && sprite != null)
            {
                employeeImage.sprite = sprite;
            }
        }

        public void PlayWalkIn()
        {
            animator.SetTrigger("walk_in");
        }

        public void PlayWalkOut()
        {
            animator.SetTrigger("walk_out");
        }

        // Called by Animation Event at end of walk-in clip
        public void WalkInFinished()
        {
            OnWalkInComplete?.Invoke();
        }

        // Called by Animation Event at end of walk-out clip
        public void WalkOutFinished()
        {
            OnWalkOutComplete?.Invoke();
        }
    }
}
