using UnityEngine;
using UnityEngine.UI;
using CEOGame.Core;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class ClockDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform hourHand;
        [SerializeField] RectTransform minuteHand;
        [SerializeField] float hourStartAngle = -240f;  // 8 o'clock position

        [Header("Clock Face")]
        [SerializeField] Image clockFaceImage;
        [SerializeField] DayCycleManager dayCycleManager;
        [SerializeField] Sprite morningSprite;
        [SerializeField] Sprite middaySprite;
        [SerializeField] Sprite sunsetSprite;

        TimeOfDay lastPhase = (TimeOfDay)(-1);

        public void UpdateClock(int completedRequests, float timerFraction)
        {
            // Hour hand: base position from completed requests + smooth offset from current timer
            float hourAngle = hourStartAngle - ((completedRequests - 1) * 30f + timerFraction * 30f);
            hourHand.localRotation = Quaternion.Euler(0f, 0f, hourAngle);

            // Minute hand: one full 360° rotation per request
            float minuteAngle = -timerFraction * 360f;
            minuteHand.localRotation = Quaternion.Euler(0f, 0f, minuteAngle);

            // Update clock face sprite based on time-of-day phase
            if (dayCycleManager != null && clockFaceImage != null)
            {
                TimeOfDay phase = dayCycleManager.CurrentPhase;
                if (phase != lastPhase)
                {
                    lastPhase = phase;
                    clockFaceImage.sprite = phase switch
                    {
                        TimeOfDay.Morning => morningSprite,
                        TimeOfDay.Midday => middaySprite,
                        TimeOfDay.Sunset => sunsetSprite,
                        _ => morningSprite
                    };
                }
            }
        }
    }
}
