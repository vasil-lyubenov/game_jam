using System;
using System.Collections.Generic;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Settings")]
        public float dayDuration = 180f;
        public int totalDays = 3;

        float timeRemaining;
        bool timerRunning;

        public event Action<float> OnTimerTick;
        public event Action OnDayEnded;

        void Start()
        {
            StartDay();
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
                EndDay();
            }
        }

        void StartDay()
        {
            timeRemaining = dayDuration;
            timerRunning = true;
            OnTimerTick?.Invoke(timeRemaining);
        }

        public void Pause() => timerRunning = false;
        public void Resume() => timerRunning = true;

        public void EndDay()
        {
            timerRunning = false;
            ProcessDelayedEffects();
            OnDayEnded?.Invoke();

            if (GameState.Instance.currentDay >= totalDays)
            {
                GameState.Instance.TriggerGameOver();
            }
            else
            {
                GameState.Instance.SetDay(GameState.Instance.currentDay + 1);
                StartDay();
            }
        }

        void ProcessDelayedEffects()
        {
            var gs = GameState.Instance;
            var remaining = new List<DelayedEffect>();

            foreach (var effect in gs.pendingDelayedEffects)
            {
                var e = effect;
                e.turnsRemaining--;

                if (e.turnsRemaining <= 0)
                {
                    gs.ApplyStatChanges(e.budgetChange, e.moraleChange, 0);
                }
                else
                {
                    remaining.Add(e);
                }
            }

            gs.pendingDelayedEffects = remaining;
        }
    }
}
