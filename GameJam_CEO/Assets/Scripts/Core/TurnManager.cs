using System;
using UnityEngine;

namespace CEOGame.Core
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Settings")]
        public float dayDuration = 100f;

        float timeRemaining;
        bool timerRunning;
        bool timeUpFired;

        public float DayDuration => dayDuration;
        public float TimeRemaining => timeRemaining;
        public bool TimerRunning => timerRunning;

        public event Action<float> OnTimerTick;
        public event Action OnTimeUp;

        void Start()
        {
            timeRemaining = dayDuration;
            timerRunning = true;
            timeUpFired = false;
            OnTimerTick?.Invoke(timeRemaining);
        }

        void Update()
        {
            if (!timerRunning || GameState.Instance.gameOver) return;

            timeRemaining -= Time.deltaTime;
            OnTimerTick?.Invoke(timeRemaining);

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                timerRunning = false;
                if (!timeUpFired)
                {
                    timeUpFired = true;
                    OnTimeUp?.Invoke();
                }
            }
        }

        public void ResetTimer()
        {
            timeRemaining = dayDuration;
            timerRunning = true;
            timeUpFired = false;
        }

        public void Pause() => timerRunning = false;
        public void Resume() => timerRunning = true;
    }
}
