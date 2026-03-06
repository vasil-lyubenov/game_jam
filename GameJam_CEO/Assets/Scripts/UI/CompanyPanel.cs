using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

        [Header("Relationship Sprites")]
        public Sprite goodSprite;
        public Sprite neutralSprite;
        public Sprite badSprite;

        void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void ShowForEmployee(EmployeeData current)
        {
            foreach (Transform child in scrollContent)
                Destroy(child.gameObject);

            var allEmployees = GameState.Instance.allEmployees;
            if (allEmployees == null || allEmployees.Length == 0) return;

            // Group by team, preserving Team enum order
            var byTeam = new Dictionary<Team, List<EmployeeData>>();
            foreach (var emp in allEmployees)
            {
                if (emp == current) continue;
                if (!byTeam.ContainsKey(emp.team))
                    byTeam[emp.team] = new List<EmployeeData>();
                byTeam[emp.team].Add(emp);
            }

            foreach (Team team in Enum.GetValues(typeof(Team)))
            {
                if (!byTeam.ContainsKey(team) || byTeam[team].Count == 0) continue;

                var section = Instantiate(teamSectionPrefab, scrollContent);
                section.headerText.text = team.ToString();

                foreach (var emp in byTeam[team])
                {
                    var card = Instantiate(employeeCardPrefab, section.employeesRow);
                    card.nameText.text = emp.employeeName;
                    card.stateImage.sprite = GetStateSprite(current, emp);
                }
            }
        }

        Sprite GetStateSprite(EmployeeData current, EmployeeData other)
        {
            if (current.goodRelationships != null)
                foreach (var e in current.goodRelationships)
                    if (e == other) return goodSprite;

            if (current.badRelationships != null)
                foreach (var e in current.badRelationships)
                    if (e == other) return badSprite;

            return neutralSprite;
        }
    }
}
