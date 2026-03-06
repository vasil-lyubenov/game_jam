using System;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class DecisionProcessor : MonoBehaviour
    {
        public event Action<RequestData, DecisionOutcome> OnDecisionProcessed;

        public void ProcessDecision(RequestData request, bool approved)
        {
            var outcome = approved ? request.approveOutcome : request.denyOutcome;
            var gs = GameState.Instance;

            gs.ApplyStatChanges(outcome.budgetChange, outcome.moraleChange, outcome.peopleChange);
            gs.RecordDecision(request, approved);

            OnDecisionProcessed?.Invoke(request, outcome);
        }
    }
}
