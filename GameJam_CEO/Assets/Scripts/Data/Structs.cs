using System;
using UnityEngine;

namespace CEOGame.Data
{
    [Serializable]
    public struct DecisionOutcome
    {
        public int budgetChange;
        public int moraleChange;
        public int peopleChange;
        public int employeeHappinessChange;
        public bool employeeLeaves;
        [TextArea(2, 4)]
        public string outcomeText;
    }
}
