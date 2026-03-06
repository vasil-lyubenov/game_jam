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
        [TextArea(2, 4)]
        public string outcomeText;
    }
}
