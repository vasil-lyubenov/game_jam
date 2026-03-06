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
        public int budget = 100000;
        public int morale = 80;
        public int people = 30;
        public bool gameOver;

        [Header("Decision History")]
        public List<RequestData> approvedRequests = new();
        public List<RequestData> deniedRequests = new();

        public event Action<int, int, int> OnStatsChanged;
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
            budget = Mathf.Clamp(budget + budgetDelta, 0, 999999);
            morale = Mathf.Clamp(morale + moraleDelta, 0, 100);
            people = Mathf.Max(people + peopleDelta, 0);
            OnStatsChanged?.Invoke(budget, morale, people);
        }

        public void RecordDecision(RequestData request, bool approved)
        {
            if (approved)
                approvedRequests.Add(request);
            else
                deniedRequests.Add(request);
        }

        public void TriggerGameOver()
        {
            gameOver = true;
            OnGameOver?.Invoke();
        }
    }
}
