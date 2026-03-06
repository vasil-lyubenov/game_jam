using System;
using System.Collections.Generic;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class GameState : MonoBehaviour
    {
        public static GameState Instance { get; private set; }

        [Header("Company Roster")]
        public EmployeeData[] allEmployees;

        [Header("Starting Values")]
        public int budget = 1000;
        public int morale = 70;
        public int people = 45;
        public int currentDay = 1;
        public bool gameOver;

        [Header("Decision History")]
        public List<RequestData> approvedRequests = new();
        public List<RequestData> deniedRequests = new();
        public List<DelayedEffect> pendingDelayedEffects = new();

        public event Action<int, int, int> OnStatsChanged;
        public event Action<int> OnDayChanged;
        public event Action OnGameOver;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ApplyStatChanges(int budgetDelta, int moraleDelta, int peopleDelta)
        {
            budget = Mathf.Clamp(budget + budgetDelta, 0, 9999);
            morale = Mathf.Clamp(morale + moraleDelta, 0, 100);
            people = Mathf.Max(people + peopleDelta, 0);
            OnStatsChanged?.Invoke(budget, morale, people);

            if (budget <= 0 || morale <= 0 || people <= 0)
            {
                gameOver = true;
                OnGameOver?.Invoke();
            }
        }

        public void RecordDecision(RequestData request, bool approved)
        {
            if (approved)
                approvedRequests.Add(request);
            else
                deniedRequests.Add(request);
        }

        public void SetDay(int day)
        {
            currentDay = day;
            OnDayChanged?.Invoke(currentDay);
        }

        public void TriggerGameOver()
        {
            gameOver = true;
            OnGameOver?.Invoke();
        }
    }
}
