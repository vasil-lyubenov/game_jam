using System;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class DayCycleManager : MonoBehaviour
    {
        [Header("Phase Thresholds (fraction of day elapsed)")]
        [SerializeField] float middayThreshold = 0.33f;
        [SerializeField] float sunsetThreshold = 0.66f;

        TimeOfDay currentPhase = TimeOfDay.Morning;
        public TimeOfDay CurrentPhase => currentPhase;

        public event Action<TimeOfDay> OnPhaseChanged;

        public TimeOfDay GetPhaseForTime(float timeRemaining, float dayDuration)
        {
            float elapsed = dayDuration - timeRemaining;
            float fraction = Mathf.Clamp01(elapsed / dayDuration);

            if (fraction >= sunsetThreshold) return TimeOfDay.Sunset;
            if (fraction >= middayThreshold) return TimeOfDay.Midday;
            return TimeOfDay.Morning;
        }

        public void UpdatePhase(float timeRemaining, float dayDuration)
        {
            TimeOfDay newPhase = GetPhaseForTime(timeRemaining, dayDuration);
            if (newPhase == currentPhase) return;

            currentPhase = newPhase;
            OnPhaseChanged?.Invoke(currentPhase);
        }
    }
}
