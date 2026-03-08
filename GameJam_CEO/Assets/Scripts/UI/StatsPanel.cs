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
            budgetText.text = $"{budget:N0}€";
            moraleText.text = $"{morale}%";
            peopleText.text = $"{people}";
        }
    }
}
