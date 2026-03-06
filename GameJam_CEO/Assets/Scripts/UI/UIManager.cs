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
        public ClockDisplay clockDisplay;
        public CompanyPanel companyPanel;
        public EndingScreen endingScreen;

        [Header("Menu")]
        public Button menuButton;
        public PauseMenuPanel pauseMenuPanel;

        [Header("Panel Toggle Buttons")]
        public Button charshaButton;
        public Button vizitkaButton;

        [Header("Environment")]
        public EnvironmentDisplay environmentDisplay;
        public DayCycleManager dayCycleManager;

        [Header("HR Tip")]
        // public HRTipPanel hrTipPanel;

        RequestData currentRequest;

        void Start()
        {
            // Subscribe to core events
            gameState.OnStatsChanged += OnStatsChanged;
            gameState.OnGameOver += OnGameOver;

            turnManager.OnTimerTick += OnTimerTick;
            turnManager.OnTimeUp += OnTimeUp;

            requestManager.OnRequestServed += OnRequestServed;
            requestManager.OnNoMoreRequests += OnNoMoreRequests;

            decisionProcessor.OnDecisionProcessed += OnDecisionProcessed;

            hrTipSystem.OnTipUsed += OnTipUsed;

            // Button listeners
            requestPanel.approveButton.onClick.AddListener(() => OnPlayerDecision(true));
            requestPanel.denyButton.onClick.AddListener(() => OnPlayerDecision(false));
            // hrTipPanel.useTipButton.onClick.AddListener(OnHRTipClicked);
            menuButton.onClick.AddListener(OnMenuClicked);
            charshaButton.onClick.AddListener(() => companyPanel.Toggle());
            vizitkaButton.onClick.AddListener(() => employeeInfoPanel.Toggle());

            // Initialize display
            if (environmentDisplay != null)
                environmentDisplay.SetEnvironment(TimeOfDay.Morning);
            // statsPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people);
            // hrTipPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people, hrTipSystem.tipsRemaining);
            requestPanel.Clear();

            // Build queue and serve
            requestManager.BuildQueue();
            requestManager.ServeNextRequest();
        }

        void OnStatsChanged(int budget, int morale, int people)
        {
            // statsPanel.UpdateStats(budget, morale, people);
            // hrTipPanel.UpdateStats(budget, morale, people, hrTipSystem.tipsRemaining);
        }

        void OnTimerTick(float seconds)
        {
            clockDisplay.UpdateClock(seconds, turnManager.dayDuration);

            if (dayCycleManager != null)
                dayCycleManager.UpdatePhase(seconds, turnManager.dayDuration);
        }

        void OnTimeUp()
        {
            //companyPanel.AddLogEntry("--- Day ended ---");
        }
        void OnDayEnded() { }

        void OnRequestServed(RequestData request)
        {
            currentRequest = request;
            requestPanel.ShowRequest(request);
            employeeInfoPanel.ShowEmployee(request.requestingEmployee, request);
            companyPanel.ShowForEmployee(request.requestingEmployee);
            // hrTipPanel.ShowEmployee(request.requestingEmployee, hrTipSystem.tipsRemaining);
            // hrTipPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people, hrTipSystem.tipsRemaining);
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

        void OnMenuClicked()
        {
            if (pauseMenuPanel.IsVisible)
            {
                pauseMenuPanel.Hide();
                turnManager.Resume();
            }
            else
            {
                pauseMenuPanel.Show();
                turnManager.Pause();
            }
        }

        void OnHRTipClicked()
        {
            if (currentRequest == null) return;
            hrTipSystem.UseTip(currentRequest.requestingEmployee);
        }

        void OnTipUsed(string tipText)
        {
            // hrTipPanel.ShowTipBubble(tipText, hrTipSystem.tipsRemaining);
            // hrTipPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people, hrTipSystem.tipsRemaining);
        }

        void OnDestroy()
        {
            if (gameState != null)
            {
                gameState.OnStatsChanged -= OnStatsChanged;
                gameState.OnGameOver -= OnGameOver;
            }
            if (turnManager != null)
            {
                turnManager.OnTimerTick -= OnTimerTick;
                turnManager.OnTimeUp -= OnTimeUp;
            }
            if (requestManager != null)
            {
                requestManager.OnRequestServed -= OnRequestServed;
                requestManager.OnNoMoreRequests -= OnNoMoreRequests;
            }
            if (decisionProcessor != null)
                decisionProcessor.OnDecisionProcessed -= OnDecisionProcessed;
            if (hrTipSystem != null)
                hrTipSystem.OnTipUsed -= OnTipUsed;
        }
    }
}