using System.Collections.Generic;
using UnityEngine;
using CEOGame.Core;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class CompanyPanel : MonoBehaviour
    {
        [Header("Scroll Content")]
        public Transform scrollContent;

        [Header("Prefabs")]
        public TeamSection teamSectionPrefab;
        public EmployeeCard employeeCardPrefab;

        [Header("Relationship Colors")]
        public Color goodColor = new Color(0.2f, 0.75f, 0.2f);
        public Color neutralColor = new Color(0.15f, 0.15f, 0.15f);
        public Color badColor = new Color(0.75f, 0.2f, 0.2f);

        void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Toggle()
        {
            Debug.Log("Test turn on company panel");
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void ShowForEmployee(EmployeeData current)
        {
            foreach (Transform child in scrollContent)
                Destroy(child.gameObject);

            var allEmployees = GameState.Instance.allEmployees;
            if (allEmployees == null || allEmployees.Length == 0) return;

            var byTeam = new Dictionary<Team, List<EmployeeData>>();
            foreach (var emp in allEmployees)
            {
                if (emp == current) continue;
                if (!byTeam.ContainsKey(emp.team))
                    byTeam[emp.team] = new List<EmployeeData>();
                byTeam[emp.team].Add(emp);
            }

            foreach (var kvp in byTeam)
            {
                var section = Instantiate(teamSectionPrefab, scrollContent);
                section.headerText.text = kvp.Key.ToString();

                foreach (var emp in kvp.Value)
                {
                    var card = Instantiate(employeeCardPrefab, section.employeesRow);
                    card.nameText.text = emp.employeeName;
                    if (emp.portrait != null)
                        card.portraitImage.sprite = emp.portrait;
                    card.background.color = GetRelationshipColor(current, emp);
                }
            }
        }

        Color GetRelationshipColor(EmployeeData current, EmployeeData other)
        {
            if (current.goodRelationships != null)
                foreach (var e in current.goodRelationships)
                    if (e == other) return goodColor;

            if (current.badRelationships != null)
                foreach (var e in current.badRelationships)
                    if (e == other) return badColor;

            return neutralColor;
        }
    }
}
