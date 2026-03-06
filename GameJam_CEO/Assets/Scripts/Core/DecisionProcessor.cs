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
            var employee = request.requestingEmployee;

            // Apply immediate stat changes
            gs.ApplyStatChanges(outcome.budgetChange, outcome.moraleChange, outcome.peopleChange);

            // Apply happiness change to employee
            employee.happiness = Mathf.Clamp(employee.happiness + outcome.employeeHappinessChange, 0, 100);

            // Handle employee leaving
            if (outcome.employeeLeaves)
            {
                gs.ApplyStatChanges(0, 0, -1);
            }

            // Handle position change
            if (outcome.changesPosition)
            {
                employee.position = outcome.newPosition;
            }

            // Queue delayed effects
            if (outcome.hasDelayedEffect)
            {
                gs.pendingDelayedEffects.Add(new DelayedEffect
                {
                    turnsRemaining = outcome.delayedTurns,
                    budgetChange = outcome.delayedBudgetChange,
                    moraleChange = outcome.delayedMoraleChange,
                    sourceRequest = request
                });
            }

            // Relationship ripple
            ProcessRelationshipRipple(employee, outcome);

            // Record decision
            gs.RecordDecision(request, approved);

            OnDecisionProcessed?.Invoke(request, outcome);
        }

        void ProcessRelationshipRipple(EmployeeData employee, DecisionOutcome outcome)
        {
            if (employee.relationships == null) return;

            int happinessShift = outcome.employeeHappinessChange;
            if (happinessShift == 0) return;

            foreach (var rel in employee.relationships)
            {
                if (rel.colleague == null) continue;

                int ripple = Mathf.RoundToInt(happinessShift * 0.5f);

                switch (rel.type)
                {
                    case RelationshipType.Good:
                        // Same direction
                        rel.colleague.happiness = Mathf.Clamp(rel.colleague.happiness + ripple, 0, 100);
                        break;
                    case RelationshipType.Bad:
                        // Opposite direction
                        rel.colleague.happiness = Mathf.Clamp(rel.colleague.happiness - ripple, 0, 100);
                        break;
                    case RelationshipType.Neutral:
                        break;
                }
            }
        }
    }
}
