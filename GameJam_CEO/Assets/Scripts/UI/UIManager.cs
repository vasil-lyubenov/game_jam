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
        public EmployeeAnimator employeeAnimator;

        [Header("UI Panels")]
        public StatsPanel statsPanel;
        public RequestPanel requestPanel;
        public EmployeeInfoPanel employeeInfoPanel;
        public CompanyPanel companyPanel;
        public EndingScreen endingScreen;

        [Header("Menu")]
        public Button menuButton;
        public PauseMenuPanel pauseMenuPanel;

        [Header("HR Tip")]
        public HRTipPanel hrTipPanel;

        [Header("Day Cycle")]
        public DayCycleManager dayCycleManager;
        public EnvironmentDisplay environmentDisplay;
        public ClockDisplay clockDisplay;

        RequestData currentRequest;
        TimeOfDay lastAppliedPhase = TimeOfDay.Morning;
        bool timeUpLocked;
        bool gameOverPending;

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

            hrTipSystem.OnTraitRevealed += OnTraitRevealed;

            employeeAnimator.OnWalkInComplete += OnWalkInComplete;
            employeeAnimator.OnWalkOutComplete += OnWalkOutComplete;

            // Button listeners
            requestPanel.approveButton.onClick.AddListener(() => OnPlayerDecision(true));
            requestPanel.denyButton.onClick.AddListener(() => OnPlayerDecision(false));
            hrTipPanel.useTipButton.onClick.AddListener(OnHRTipClicked);
            menuButton.onClick.AddListener(OnMenuClicked);

            // Initialize display
            statsPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people);
            requestPanel.Clear();

            // Initialize environment
            lastAppliedPhase = TimeOfDay.Morning;
            environmentDisplay.SetEnvironment(TimeOfDay.Morning);

            // Build first queue and serve
            requestManager.BuildQueue();
            requestManager.ServeNextRequest();
        }

        void OnStatsChanged(int budget, int morale, int people)
        {
            statsPanel.UpdateStats(budget, morale, people);
        }

        void OnTimerTick(float seconds)
        {
            clockDisplay.UpdateClock(seconds, turnManager.DayDuration);
            dayCycleManager.UpdatePhase(seconds, turnManager.DayDuration);

            var currentPhase = dayCycleManager.CurrentPhase;
            if (currentPhase != lastAppliedPhase)
            {
                lastAppliedPhase = currentPhase;
                environmentDisplay.SetEnvironment(currentPhase);
            }
        }

        void OnRequestServed(RequestData request)
        {
            currentRequest = request;

            // Swap sprite and play walk-in; UI shown after animation completes
            var employee = request.requestingEmployee;
            if (employee.portrait != null)
                employeeAnimator.SetEmployeeSprite(employee.portrait);

            requestPanel.Clear();
            employeeAnimator.PlayWalkIn();
        }

        void OnWalkInComplete()
        {
            if (currentRequest == null) return;
            requestPanel.ShowRequest(currentRequest);
            employeeInfoPanel.ShowEmployee(currentRequest.requestingEmployee);
            hrTipPanel.ShowEmployee(currentRequest.requestingEmployee, hrTipSystem.tipsRemaining);
        }

        void OnNoMoreRequests()
        {
            requestPanel.Clear();
            gameState.TriggerGameOver();
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
            StartCoroutine(ShowOutcomeThenWalkOut());
        }

        IEnumerator ShowOutcomeThenWalkOut()
        {
            yield return new WaitForSeconds(2f);

            requestPanel.Clear();
            employeeAnimator.PlayWalkOut();
        }

        void OnWalkOutComplete()
        {
            // If time ran out or game over pending, end now
            if (timeUpLocked || gameOverPending)
            {
                timeUpLocked = false;
                gameOverPending = false;
                SetUILocked(false);
                gameState.TriggerGameOver();
                return;
            }

            // Rebuild queue to pick up newly eligible prerequisite-chained requests
            requestManager.BuildQueue();
            requestManager.ServeNextRequest();
        }

        void OnTimeUp()
        {
            if (currentRequest != null)
            {
                // Request is active — lock UI except approve/deny, game over after walk-out
                timeUpLocked = true;
                SetUILocked(true);
            }
            else
            {
                // No active request — game over immediately
                gameState.TriggerGameOver();
            }
        }

        void SetUILocked(bool locked)
        {
            menuButton.interactable = !locked;
            hrTipPanel.useTipButton.interactable = !locked;
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

        void OnTraitRevealed(EmployeeData employee)
        {
            employeeInfoPanel.ShowEmployee(employee);
            hrTipPanel.OnTraitRevealed(employee, hrTipSystem.tipsRemaining);
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
                hrTipSystem.OnTraitRevealed -= OnTraitRevealed;
            if (employeeAnimator != null)
            {
                employeeAnimator.OnWalkInComplete -= OnWalkInComplete;
                employeeAnimator.OnWalkOutComplete -= OnWalkOutComplete;
            }
        }
    }
}
