using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CEOGame.UI
{
    public class TimerDisplay : MonoBehaviour
    {
        public TMP_Text timerText;
        public TMP_Text dayText;
        public Image timerFillImage;

        float maxTime;

        public void SetMaxTime(float max)
        {
            maxTime = max;
            if (timerFillImage != null)
                timerFillImage.fillAmount = 1f;
        }

        public void UpdateTimer(float secondsRemaining)
        {
            int minutes = Mathf.FloorToInt(secondsRemaining / 60f);
            int seconds = Mathf.FloorToInt(secondsRemaining % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";

            if (timerFillImage != null && maxTime > 0f)
                timerFillImage.fillAmount = secondsRemaining / maxTime;
        }

        public void UpdateDay(int day)
        {
            dayText.text = $"Day {day}";
        }
    }
}
