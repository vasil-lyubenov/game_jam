using UnityEngine;
using TMPro;

namespace CEOGame.UI
{
    public class TimerDisplay : MonoBehaviour
    {
        public TMP_Text timerText;
        public TMP_Text dayText;

        public void UpdateTimer(float secondsRemaining)
        {
            int minutes = Mathf.FloorToInt(secondsRemaining / 60f);
            int seconds = Mathf.FloorToInt(secondsRemaining % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        public void UpdateDay(int day)
        {
            dayText.text = $"Day {day}";
        }
    }
}
