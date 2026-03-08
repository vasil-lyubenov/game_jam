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
        public HRTipPanel hrTipPanel;

        [Header("Tutorial")]
        public TutorialManager tutorialManager;

        RequestData currentRequest;
        RequestData pendingRequest;

        void Start()
        {
            // Freeze timer immediately — tutorial runs before first request
            turnManager.Pause();

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
            if (hrTipPanel != null)
                hrTipPanel.useTipButton.onClick.AddListener(OnHRTipClicked);
            menuButton.onClick.AddListener(OnMenuClicked);
            pauseMenuPanel.resumeButton.onClick.AddListener(OnMenuClicked);
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
            statsPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people);
            if (hrTipPanel != null)
                hrTipPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people, hrTipSystem.tipsRemaining);
            requestPanel.Clear();

            // Disable side panels — no employee data to show yet
            charshaButton.interactable = false;
            vizitkaButton.interactable = false;

            // Run tutorial first, or start game directly if not configured
            if (tutorialManager != null && tutorialManager.ralicaData != null)
            {
                tutorialManager.OnTutorialComplete += OnTutorialComplete;
                tutorialManager.StartTutorial();
                if (employeeAnimator != null)
                {
                    employeeAnimator.SetEmployeeSprite(tutorialManager.ralicaData.portrait);
                    employeeAnimator.PlayWalkIn();
                }
                else
                {
                    ShowTutorialPanel();
                }
            }
            else
            {
                // No tutorial — build queue and serve first request (timer starts in ShowPendingRequest)
                requestManager.BuildQueue();
                requestManager.ServeNextRequest();
            }
        }

        void OnStatsChanged(int budget, int morale, int people)
        {
            statsPanel.UpdateStats(budget, morale, people);
            if (hrTipPanel != null)
                hrTipPanel.UpdateStats(budget, morale, people, hrTipSystem.tipsRemaining);
        }

        void OnTimerTick(float seconds)
        {
            clockDisplay.UpdateClock(requestManager.CompletedRequests, turnManager.ElapsedFraction);
        }

        void OnTimeUp()
        {
            // Skip to last dialogue line and show approve/deny buttons
            requestPanel.SkipToLastLine();

            // Disable non-decision buttons (visible but greyed out)
            requestPanel.nextButton.interactable = false;
            charshaButton.interactable = false;
            vizitkaButton.interactable = false;
            menuButton.interactable = false;
            if (hrTipPanel != null)
                hrTipPanel.useTipButton.interactable = false;
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
            if (tutorialManager != null && tutorialManager.TutorialActive)
                ShowTutorialPanel();
            else
                ShowPendingRequest();
        }

        void ShowTutorialPanel()
        {
            requestPanel.ShowTutorial(
                tutorialManager.ralicaData.portrait,
                tutorialManager.ralicaData.employeeName,
                tutorialManager.tutorialLines,
                OnTutorialLastLineNext
            );
        }

        void OnTutorialLastLineNext()
        {
            if (employeeAnimator != null)
                employeeAnimator.PlayWalkOut();
            else
                OnWalkOutComplete();
        }

        void OnTutorialComplete()
        {
            Debug.Log("[UIManager] OnTutorialComplete — starting real game");
            requestManager.BuildQueue();
            requestManager.ServeNextRequest();
        }

        void ShowPendingRequest()
        {
            if (pendingRequest == null) return;
            currentRequest = pendingRequest;
            pendingRequest = null;

            // Always re-enable side buttons so player can view employee info
            charshaButton.interactable = true;
            vizitkaButton.interactable = true;
            menuButton.interactable = true;
            requestPanel.nextButton.interactable = true;

            // Start a fresh 20s timer for this request
            turnManager.ResetTimer();

            // Advance day cycle based on request count
            if (dayCycleManager != null)
                dayCycleManager.NotifyRequestShown();

            requestPanel.ShowRequest(currentRequest);

            // If time is already up, skip dialogue and show approve/deny immediately
            //if (timeUp)
            //    requestPanel.SkipToLastLine();

            employeeInfoPanel.ShowEmployee(currentRequest.requestingEmployee, currentRequest);
            companyPanel.ShowForEmployee(currentRequest.requestingEmployee);
            if (hrTipPanel != null)
                hrTipPanel.ShowEmployee(currentRequest.requestingEmployee, hrTipSystem.tipsRemaining);
        }

        void OnNoMoreRequests()
        {
            Debug.Log("[UIManager] OnNoMoreRequests — showing ending");
            requestPanel.Clear();
            gameState.gameOver = true;
            OnGameOver();
        }

        void OnPlayerDecision(bool approved)
        {
            if (currentRequest == null) return;
            Debug.Log($"[UIManager] OnPlayerDecision: approved={approved}, request={currentRequest.name}");

            // Keep side buttons enabled so player can still view info
            charshaButton.interactable = true;
            vizitkaButton.interactable = true;
            menuButton.interactable = true;

            // Pause timer during decision processing
            turnManager.Pause();
            requestPanel.nextButton.interactable = false;

            decisionProcessor.ProcessDecision(currentRequest, approved);
        }

        void OnDecisionProcessed(RequestData request, DecisionOutcome outcome)
        {
            Debug.Log($"[UIManager] OnDecisionProcessed: request={request.name}");
            requestPanel.ShowOutcome(outcome.outcomeText);
            currentRequest = null;
            StartCoroutine(ShowOutcomeThenWalkOut());
        }

        IEnumerator ShowOutcomeThenWalkOut()
        {
            Debug.Log("[UIManager] ShowOutcomeThenWalkOut: waiting 2s...");
            yield return new WaitForSeconds(5f);
            Debug.Log("[UIManager] ShowOutcomeThenWalkOut: wait complete, starting walk out");

            // Hide tip bubble when walk-out starts, not when it ends
            if (hrTipPanel != null)
            {
                hrTipPanel.HideTipBubble();
            }

            // Hide outcome text when walk-out starts
            //requestPanel.outcomeText.text = "";

            if (employeeAnimator != null)
            {
                requestPanel.Clear();
                employeeAnimator.PlayWalkOut();
            }
            else
            {
                OnWalkOutComplete();
            }
        }

        void OnWalkOutComplete()
        {
            Debug.Log("[UIManager] OnWalkOutComplete");
            requestPanel.Clear();

            // Hide toggle panels between requests
            employeeInfoPanel.gameObject.SetActive(false);
            companyPanel.gameObject.SetActive(false);

            if (tutorialManager != null && tutorialManager.TutorialActive)
            {
                tutorialManager.EndTutorial(); // fires OnTutorialComplete → BuildQueue + ServeNextRequest
            }
            else
            {
                requestManager.ServeNextRequest();
            }
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
                Time.timeScale = 1f;
                turnManager.Resume();
            }
            else
            {
                pauseMenuPanel.Show();
                Time.timeScale = 0f;
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
            AudioManager.Instance?.PlayHRTipChime();
            if (hrTipPanel != null)
            {
                hrTipPanel.ShowTipBubble(tipText, hrTipSystem.tipsRemaining);
                hrTipPanel.UpdateStats(gameState.budget, gameState.morale, gameState.people, hrTipSystem.tipsRemaining);
            }
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
            if (tutorialManager != null)
                tutorialManager.OnTutorialComplete -= OnTutorialComplete;
        }
    }
}
