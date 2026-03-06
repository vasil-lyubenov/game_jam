using UnityEngine;
using TMPro;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class EmployeeInfoPanel : MonoBehaviour
    {
        public TMP_Text nameText;

        void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        public TMP_Text teamText;
        public TMP_Text positionText;
        public TMP_Text salaryText;
        public TMP_Text happinessText;
        public TMP_Text relationshipsText;
        public TMP_Text traitsText;

        public void ShowEmployee(EmployeeData employee)
        {
            gameObject.SetActive(true);

            nameText.text = employee.employeeName;
            teamText.text = $"Team: {employee.team}";
            positionText.text = $"Position: {employee.position}";
            salaryText.text = $"Salary: ${employee.salary}";
            happinessText.text = $"Happiness: {employee.happiness}/100";

            // Relationships
            if (employee.relationships != null && employee.relationships.Length > 0)
            {
                var sb = new System.Text.StringBuilder("Relationships:\n");
                foreach (var rel in employee.relationships)
                {
                    if (rel.colleague != null)
                        sb.AppendLine($"  {rel.colleague.employeeName} ({rel.type})");
                }
                relationshipsText.text = sb.ToString();
            }
            else
            {
                relationshipsText.text = "No known relationships";
            }

            // Hidden traits
            if (employee.traitsRevealed && employee.hiddenTraits != null && employee.hiddenTraits.Length > 0)
            {
                var traitNames = new string[employee.hiddenTraits.Length];
                for (int i = 0; i < employee.hiddenTraits.Length; i++)
                    traitNames[i] = employee.hiddenTraits[i].ToString();
                traitsText.text = $"Traits: {string.Join(", ", traitNames)}";
            }
            else
            {
                traitsText.text = employee.traitsRevealed ? "No traits" : "Traits: ???";
            }
        }
    }
}
