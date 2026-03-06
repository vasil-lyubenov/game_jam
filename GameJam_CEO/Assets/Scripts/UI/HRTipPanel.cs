using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class HRTipPanel : MonoBehaviour
    {
        public TMP_Text employeeNameText;
        public TMP_Text traitsText;
        public TMP_Text tipsRemainingText;
        public Button useTipButton;

        public void ShowEmployee(EmployeeData employee, int tipsRemaining)
        {
            employeeNameText.text = employee.employeeName;
            tipsRemainingText.text = $"Tips: {tipsRemaining}";
            RefreshTraits(employee);
            useTipButton.interactable = tipsRemaining > 0 && !employee.traitsRevealed;
        }

        public void OnTraitRevealed(EmployeeData employee, int tipsRemaining)
        {
            tipsRemainingText.text = $"Tips: {tipsRemaining}";
            RefreshTraits(employee);
            useTipButton.interactable = false;
        }

        public void SetButtonEnabled(bool enabled)
        {
            useTipButton.interactable = enabled;
        }

        void RefreshTraits(EmployeeData employee)
        {
            if (employee.traitsRevealed && employee.hiddenTraits != null && employee.hiddenTraits.Length > 0)
            {
                var names = new string[employee.hiddenTraits.Length];
                for (int i = 0; i < employee.hiddenTraits.Length; i++)
                    names[i] = employee.hiddenTraits[i].ToString();
                traitsText.text = $"Traits: {string.Join(", ", names)}";
            }
            else
            {
                traitsText.text = employee.traitsRevealed ? "No traits" : "Traits: ???";
            }
        }
    }
}
