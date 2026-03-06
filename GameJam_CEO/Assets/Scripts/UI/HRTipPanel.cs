using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class HRTipPanel : MonoBehaviour
    {
        // public TMP_Text employeeNameText;
        public TMP_Text tipsRemainingText;
        public Button useTipButton;

        [Header("Tip Bubble")]
        public GameObject tipBubble;
        public TMP_Text tipBubbleText;

        [Header("Stats Display")]
        public TMP_Text budgetText;
        public TMP_Text peopleText;
        public TMP_Text happinessText;
        public TMP_Text tipsText;

        public void ShowEmployee(EmployeeData employee, int tipsRemaining)
        {
            // employeeNameText.text = employee.employeeName;
            tipsRemainingText.text = $"Tips: {tipsRemaining}";
            HideTipBubble();
            useTipButton.interactable = tipsRemaining > 0 && !string.IsNullOrEmpty(employee.hrTip);
        }

        public void ShowTipBubble(string tipText, int tipsRemaining)
        {
            tipsRemainingText.text = $"Tips: {tipsRemaining}";
            if (tipBubble != null) tipBubble.SetActive(true);
            if (tipBubbleText != null) tipBubbleText.text = tipText;
            useTipButton.interactable = false;
        }

        public void HideTipBubble()
        {
            if (tipBubble != null) tipBubble.SetActive(false);
        }

        public void UpdateStats(int budget, int morale, int people, int tips)
        {
            if (budgetText != null) budgetText.text = $"Budget: ${budget}";
            if (peopleText != null) peopleText.text = $"People: {people}";
            if (happinessText != null) happinessText.text = $"Happiness: {morale}%";
            if (tipsText != null) tipsText.text = $"Tips: {tips}";
        }
    }
}
