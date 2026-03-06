using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class EmployeeInfoPanel : MonoBehaviour
    {
        public Image portraitImage;
        public TMP_Text nameText;
        public TMP_Text positionText;
        public TMP_Text salaryText;
        public TMP_Text happinessText;
        public TMP_Text bioText;

        void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void ShowEmployee(EmployeeData employee, RequestData request)
        {
            if (portraitImage != null) portraitImage.sprite = employee.portrait;
            nameText.text = employee.employeeName;
            positionText.text = $"{employee.position} · {employee.team}";
            salaryText.text = $"Salary: {employee.salary}€";
            happinessText.text = $"Happiness: {employee.happiness}/100";
            bioText.text = employee.personalityBio;
        }

        string FormatCategory(RequestCategory category) => category switch
        {
            RequestCategory.SalaryRaise => "Salary Raise",
            RequestCategory.PositionChange => "Position Change",
            RequestCategory.TeamTransfer => "Team Transfer",
            RequestCategory.StrategicInvestment => "Strategic Investment",
            RequestCategory.Hiring => "Hiring",
            RequestCategory.Firing => "Firing",
            _ => category.ToString()
        };
    }
}
