using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CEOGame.Core;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Core References")]
        public GameState gameState;
        public TurnManager turnManager;
        public RequestManager requestManager;
        public DecisionProcessor decisionProcessor;
        public HRTipSystem hrTipSystem;
        public EndingsManager endingsManager;

        [Header("UI Panels")]
        public StatsPanel statsPanel;
        public RequestPanel requestPanel;
        public EmployeeInfoPanel employeeInfoPanel;
        public TimerDisplay timerDisplay;
        public CompanyPanel companyPanel;
        public EndingScreen endingScreen;

        [Header("HR Tip")]
        public Button hrTipButton;

        RequestData currentRequest;

        void Start()
        {
            // Subscribe to core events
            gameState.OnStatsChanged += OnStatsChanged;
            gameState.OnDayChanged += OnDayChanged;
            gameState.OnGameOver += OnGameOver;

            turnManager.OnTimerTick += OnTimerTick;
            turnManager.OnDayEnded += OnDayEnded;

            requestManager.OnRequestServed += OnRequestServed;
            requestManager.OnNoMoreRequests += OnNoMoreRequests;

            decisionProcessor.OnDecisionProcessed += OnDecisionProcessed;

            hrTipSystem.OnTraitRevealed += OnTraitRevealed;

            // Button listeners
            requestPanel.approveButton.onClick.AddListener(() => OnPlayerDecision(true));
            requestPanel.denyButton.onClick.AddListener(() => OnPlayerDecision(false));
            hrTipButton.onClick.AddListener(OnHRTipClicked);

            // Initialize display
            statsPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people);
            timerDisplay.UpdateDay(gameState.currentDay);
            requestPanel.Clear();

            // Build first day queue and serve
            requestManager.BuildQueue(gameState.currentDay);
            requestManager.ServeNextRequest();
        }

        void OnStatsChanged(int budget, int morale, int people)
        {
            statsPanel.UpdateStats(budget, morale, people);
        }

        void OnDayChanged(int day)
        {
            timerDisplay.UpdateDay(day);
            requestManager.BuildQueue(day);
        }

        void OnTimerTick(float seconds)
        {
            timerDisplay.UpdateTimer(seconds);
        }

        void OnDayEnded()
        {
            companyPanel.AddLogEntry($"--- Day {gameState.currentDay} ended ---");
        }

        void OnRequestServed(RequestData request)
        {
            currentRequest = request;
            requestPanel.ShowRequest(request);
            employeeInfoPanel.ShowEmployee(request.requestingEmployee);
            UpdateHRTipButton();
        }

        void OnNoMoreRequests()
        {
            requestPanel.Clear();
        }

        void OnPlayerDecision(bool approved)
        {
            if (currentRequest == null) return;
            decisionProcessor.ProcessDecision(currentRequest, approved);
        }

        void OnDecisionProcessed(RequestData request, DecisionOutcome outcome)
        {
            string decision = gameState.approvedRequests.Contains(request) ? "Approved" : "Denied";
            companyPanel.AddLogEntry($"{decision}: {request.requestingEmployee.employeeName}'s {request.category}");

            requestPanel.ShowOutcome(outcome.outcomeText);
            StartCoroutine(ShowOutcomeThenNext());
        }

        IEnumerator ShowOutcomeThenNext()
        {
            yield return new WaitForSeconds(2f);
            requestManager.ServeNextRequest();
        }

        void OnGameOver()
        {
            var ending = endingsManager.DetermineEnding();
            endingScreen.Show(ending);
        }

        void OnHRTipClicked()
        {
            if (currentRequest == null) return;
            hrTipSystem.UseTip(currentRequest.requestingEmployee);
        }

        void OnTraitRevealed(EmployeeData employee)
        {
            employeeInfoPanel.ShowEmployee(employee);
            UpdateHRTipButton();
        }

        void UpdateHRTipButton()
        {
            hrTipButton.interactable = hrTipSystem.CanUseTip()
                && currentRequest != null
                && !currentRequest.requestingEmployee.traitsRevealed;
        }

        void OnDestroy()
        {
            if (gameState != null)
            {
                gameState.OnStatsChanged -= OnStatsChanged;
                gameState.OnDayChanged -= OnDayChanged;
                gameState.OnGameOver -= OnGameOver;
            }
            if (turnManager != null)
            {
                turnManager.OnTimerTick -= OnTimerTick;
                turnManager.OnDayEnded -= OnDayEnded;
            }
            if (requestManager != null)
            {
                requestManager.OnRequestServed -= OnRequestServed;
                requestManager.OnNoMoreRequests -= OnNoMoreRequests;
            }
            if (decisionProcessor != null)
                decisionProcessor.OnDecisionProcessed -= OnDecisionProcessed;
            if (hrTipSystem != null)
                hrTipSystem.OnTraitRevealed -= OnTraitRevealed;
        }
    }
}
