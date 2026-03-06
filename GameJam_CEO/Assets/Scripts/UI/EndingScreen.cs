using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using CEOGame.Data;
using CEOGame.Core;

namespace CEOGame.UI
{
    public class EndingScreen : MonoBehaviour
    {
        public GameObject panel;
        public TMP_Text titleText;
        public TMP_Text narrativeText;
        public TMP_Text finalStatsText;
        public Button playAgainButton;

        void Awake()
        {
            panel.SetActive(false);
            playAgainButton.onClick.AddListener(OnPlayAgain);
        }

        public void Show(EndingType ending)
        {
            panel.SetActive(true);
            var gs = GameState.Instance;

            titleText.text = ending switch
            {
                EndingType.Good => "A Thriving Company",
                EndingType.Neutral => "Steady as She Goes",
                EndingType.Bad => "Troubled Waters",
                EndingType.VeryBad => "Total Collapse",
                _ => "The End"
            };

            narrativeText.text = ending switch
            {
                EndingType.Good => "Under your leadership, the company flourished. Employees are happy, the budget is healthy, and the team is stronger than ever.",
                EndingType.Neutral => "You kept the ship afloat, but nothing spectacular happened. The company survives to see another quarter.",
                EndingType.Bad => "Things took a turn for the worse. Budget cuts, low morale, or staff departures have left the company struggling.",
                EndingType.VeryBad => "The company is in ruins. Key people left, the budget is depleted, and morale has hit rock bottom.",
                _ => ""
            };

            finalStatsText.text = $"Final Budget: ${gs.budget}\nFinal Morale: {gs.morale}%\nFinal People: {gs.people}";
        }

        void OnPlayAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
