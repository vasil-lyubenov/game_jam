using System;
using UnityEngine;

namespace CEOGame.Data
{
    [Serializable]
    public struct Relationship
    {
        public EmployeeData colleague;
        public RelationshipType type;
    }

    [Serializable]
    public struct DecisionOutcome
    {
        [Header("Immediate Effects")]
        public int budgetChange;
        public int moraleChange;
        public int peopleChange;
        public int employeeHappinessChange;
        public bool employeeLeaves;

        [Header("Position Change")]
        public bool changesPosition;
        public Position newPosition;

        [Header("Delayed Effects")]
        public bool hasDelayedEffect;
        public int delayedTurns;
        public int delayedBudgetChange;
        public int delayedMoraleChange;

        [Header("Feedback")]
        [TextArea(2, 4)]
        public string outcomeText;
    }

    [Serializable]
    public struct DelayedEffect
    {
        public int turnsRemaining;
        public int budgetChange;
        public int moraleChange;
        public RequestData sourceRequest;
    }
}
