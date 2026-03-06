using UnityEngine;
using TMPro;

namespace CEOGame.UI
{
    public class StatsPanel : MonoBehaviour
    {
        public TMP_Text budgetText;
        public TMP_Text moraleText;
        public TMP_Text peopleText;

        public void UpdateStats(int budget, int morale, int people)
        {
            budgetText.text = $"Budget: ${budget}";
            moraleText.text = $"Morale: {morale}%";
            peopleText.text = $"People: {people}";
        }
    }
}
