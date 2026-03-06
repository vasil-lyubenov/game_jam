using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class HRTipPanel : MonoBehaviour
    {
        public TMP_Text employeeNameText;
        public TMP_Text insightText;
        public TMP_Text tipsRemainingText;
        public Button useTipButton;

        public void ShowEmployee(EmployeeData employee, int tipsRemaining)
        {
            employeeNameText.text = employee.employeeName;
            tipsRemainingText.text = $"Tips: {tipsRemaining}";
            insightText.text = "???";
            useTipButton.interactable = tipsRemaining > 0;
        }

        public void ShowInsight(string insight, int tipsRemaining)
        {
            insightText.text = string.IsNullOrEmpty(insight) ? "No insight available." : insight;
            tipsRemainingText.text = $"Tips: {tipsRemaining}";
            useTipButton.interactable = false;
        }

        public void SetButtonEnabled(bool enabled)
        {
            useTipButton.interactable = enabled;
        }
    }
}
