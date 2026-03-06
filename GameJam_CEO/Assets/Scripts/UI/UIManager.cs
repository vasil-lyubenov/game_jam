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

        [Header("Employee Animation")]
        public EmployeeAnimator employeeAnimator;

        [Header("HR Tip")]
        // public HRTipPanel hrTipPanel;

        RequestData currentRequest;
        RequestData pendingRequest;

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

            if (employeeAnimator != null)
            {
                employeeAnimator.OnWalkInComplete += OnWalkInComplete;
                employeeAnimator.OnWalkOutComplete += OnWalkOutComplete;
            }

            // Button listeners
            requestPanel.approveButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlayApprove();
                OnPlayerDecision(true);
            });
            requestPanel.denyButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlayDeny();
                OnPlayerDecision(false);
            });
            // hrTipPanel.useTipButton.onClick.AddListener(OnHRTipClicked);
            menuButton.onClick.AddListener(OnMenuClicked);
            charshaButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlayCompanySheet();
                companyPanel.Toggle();
            });
            vizitkaButton.onClick.AddListener(() => {
                AudioManager.Instance?.PlayCardPickup();
                employeeInfoPanel.Toggle();
            });

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
            // Skip to last dialogue line and show approve/deny buttons
            requestPanel.SkipToLastLine();

            // Disable all UI except approve/deny
            requestPanel.nextButton.gameObject.SetActive(false);
            charshaButton.interactable = false;
            vizitkaButton.interactable = false;
            menuButton.interactable = false;
        }
        void OnDayEnded() { }

        void OnRequestServed(RequestData request)
        {
            Debug.Log($"[UIManager] OnRequestServed: request={request.name}");
            pendingRequest = request;

            if (employeeAnimator != null)
            {
                employeeAnimator.SetEmployeeSprite(request.requestingEmployee.portrait);
                employeeAnimator.PlayWalkIn();
            }
            else
            {
                ShowPendingRequest();
            }
        }

        void OnWalkInComplete()
        {
            Debug.Log("[UIManager] OnWalkInComplete");
            ShowPendingRequest();
        }

        void ShowPendingRequest()
        {
            if (pendingRequest == null) return;
            currentRequest = pendingRequest;
            pendingRequest = null;

            // Re-enable UI buttons for the new request
            charshaButton.interactable = true;
            vizitkaButton.interactable = true;
            menuButton.interactable = true;

            // Reset per-request timer
            turnManager.ResetTimer();

            requestPanel.ShowRequest(currentRequest);
            employeeInfoPanel.ShowEmployee(currentRequest.requestingEmployee, currentRequest);
            companyPanel.ShowForEmployee(currentRequest.requestingEmployee);
        }

        void OnNoMoreRequests()
        {
            Debug.Log("[UIManager] OnNoMoreRequests");
            requestPanel.Clear();
        }

        void OnPlayerDecision(bool approved)
        {
            if (currentRequest == null) return;
            Debug.Log($"[UIManager] OnPlayerDecision: approved={approved}, request={currentRequest.name}");

            // Re-enable UI buttons after decision
            charshaButton.interactable = true;
            vizitkaButton.interactable = true;
            menuButton.interactable = true;

            // Stop timer (will be reset on next request)
            turnManager.Pause();

            decisionProcessor.ProcessDecision(currentRequest, approved);
        }

        void OnDecisionProcessed(RequestData request, DecisionOutcome outcome)
        {
            Debug.Log($"[UIManager] OnDecisionProcessed: request={request.name}, gameOver={gameState.gameOver}");
            requestPanel.ShowOutcome(outcome.outcomeText);
            currentRequest = null;
            StartCoroutine(ShowOutcomeThenWalkOut());
        }

        IEnumerator ShowOutcomeThenWalkOut()
        {
            Debug.Log("[UIManager] ShowOutcomeThenWalkOut: waiting 2s...");
            yield return new WaitForSeconds(2f);
            Debug.Log("[UIManager] ShowOutcomeThenWalkOut: wait complete, starting walk out");

            if (employeeAnimator != null)
            {
                employeeAnimator.PlayWalkOut();
            }
            else
            {
                OnWalkOutComplete();
            }
        }

        void OnWalkOutComplete()
        {
            Debug.Log($"[UIManager] OnWalkOutComplete: gameOver={gameState.gameOver}");
            requestPanel.Clear();

            // Hide toggle panels between requests
            employeeInfoPanel.gameObject.SetActive(false);
            companyPanel.gameObject.SetActive(false);

            if (gameState.gameOver)
            {
                Debug.Log("[UIManager] Game over — skipping ServeNextRequest");
                return;
            }

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
            if (employeeAnimator != null)
            {
                employeeAnimator.OnWalkInComplete -= OnWalkInComplete;
                employeeAnimator.OnWalkOutComplete -= OnWalkOutComplete;
            }
        }
    }
}