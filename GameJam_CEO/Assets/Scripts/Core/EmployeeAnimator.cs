using System;
using System.Collections;
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

        Coroutine waitRoutine;

        public void SetEmployeeSprite(Sprite sprite)
        {
            if (employeeImage != null && sprite != null)
            {
                employeeImage.sprite = sprite;
            }
        }

        public void PlayWalkIn()
        {
            if (waitRoutine != null) StopCoroutine(waitRoutine);
            animator.SetTrigger("walk_in");
            waitRoutine = StartCoroutine(WaitForClipEnd(() => OnWalkInComplete?.Invoke()));
        }

        public void PlayWalkOut()
        {
            if (waitRoutine != null) StopCoroutine(waitRoutine);
            animator.SetTrigger("walk_out");
            waitRoutine = StartCoroutine(WaitForClipEnd(() => OnWalkOutComplete?.Invoke()));
        }

        IEnumerator WaitForClipEnd(Action callback)
        {
            // Wait a frame for the trigger to be consumed and transition to start
            yield return null;
            yield return null;

            // Now get the current clip length from the state info
            var info = animator.GetCurrentAnimatorStateInfo(0);
            float clipDuration = 4.5f;

            // Fallback: if length is 0 or very small, use a reasonable default
            if (clipDuration <= 0.01f)
            {
                clipDuration = 4.5f;
            }

            // Wait for the clip duration to elapse
            yield return new WaitForSeconds(clipDuration);

            waitRoutine = null;
            callback?.Invoke();
        }
    }
}
