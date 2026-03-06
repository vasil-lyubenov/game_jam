using UnityEngine;

namespace CEOGame.UI
{
    public class ClockDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform hourHand;
        [SerializeField] RectTransform minuteHand;
        [SerializeField] int minuteRotations = 3;

        public void UpdateClock(float timeRemaining, float dayDuration)
        {
            float elapsed = dayDuration - timeRemaining;
            float fraction = Mathf.Clamp01(elapsed / dayDuration);

            // Hour hand: 0 to -360 over the full day
            float hourAngle = -fraction * 360f;
            hourHand.localRotation = Quaternion.Euler(0f, 0f, hourAngle);

            // Minute hand: multiple full rotations over the day
            float minuteAngle = -fraction * 360f * minuteRotations;
            minuteHand.localRotation = Quaternion.Euler(0f, 0f, minuteAngle);
        }
    }
}
