using System;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class DayCycleManager : MonoBehaviour
    {
        [Header("Requests per phase change")]
        [SerializeField] int requestsPerPhase = 3;

        TimeOfDay currentPhase = TimeOfDay.Morning;
        int requestCount;

        public TimeOfDay CurrentPhase => currentPhase;

        public event Action<TimeOfDay> OnPhaseChanged;

        /// <summary>
        /// Called each time a new request is shown to the player.
        /// After every requestsPerPhase requests, the environment advances.
        /// </summary>
        public void NotifyRequestShown()
        {
            requestCount++;

            TimeOfDay newPhase;
            if (requestCount > requestsPerPhase * 2)
                newPhase = TimeOfDay.Sunset;
            else if (requestCount > requestsPerPhase)
                newPhase = TimeOfDay.Midday;
            else
                newPhase = TimeOfDay.Morning;

            if (newPhase == currentPhase) return;

            currentPhase = newPhase;
            OnPhaseChanged?.Invoke(currentPhase);
        }

        /// <summary>
        /// Returns the phase based on request count (used by ClockDisplay for face sprite).
        /// Timer parameters kept for API compatibility but ignored.
        /// </summary>
        public TimeOfDay GetPhaseForTime(float timeRemaining, float dayDuration)
        {
            return currentPhase;
        }

        public void UpdatePhase(float timeRemaining, float dayDuration)
        {
            // Phase is now driven by request count via NotifyRequestShown().
            // This method is kept for compatibility but does nothing.
        }
    }
}
