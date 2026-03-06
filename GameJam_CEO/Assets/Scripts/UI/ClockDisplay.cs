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
        [SerializeField] int minuteRotations = 3;
        [SerializeField] float hourStartAngle = -270f;  // 9 o'clock position
        [SerializeField] float hourTotalArc = 240f;     // 9 AM → 5 PM (8 hours on 12-hour face)

        [Header("Clock Face")]
        [SerializeField] Image clockFaceImage;
        [SerializeField] DayCycleManager dayCycleManager;
        [SerializeField] Sprite morningSprite;
        [SerializeField] Sprite middaySprite;
        [SerializeField] Sprite sunsetSprite;

        TimeOfDay lastPhase = (TimeOfDay)(-1);

        public void UpdateClock(float timeRemaining, float dayDuration)
        {
            float elapsed = dayDuration - timeRemaining;
            float fraction = Mathf.Clamp01(elapsed / dayDuration);

            // Hour hand: starts at 9 o'clock, sweeps clockwise to 5 o'clock
            float hourAngle = hourStartAngle - fraction * hourTotalArc;
            hourHand.localRotation = Quaternion.Euler(0f, 0f, hourAngle);

            // Minute hand: multiple full rotations over the day
            float minuteAngle = -fraction * 360f * minuteRotations;
            minuteHand.localRotation = Quaternion.Euler(0f, 0f, minuteAngle);

            // Update clock face sprite based on time-of-day phase
            if (dayCycleManager != null && clockFaceImage != null)
            {
                TimeOfDay phase = dayCycleManager.GetPhaseForTime(timeRemaining, dayDuration);
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
