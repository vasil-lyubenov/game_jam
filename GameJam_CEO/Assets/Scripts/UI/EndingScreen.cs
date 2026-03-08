using System.Collections;
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

        [Header("Comic Panels")]
        public Image panel1Image;
        public Image panel2Image;
        public CanvasGroup panel2Group;
        public float fadeDuration = 1.5f;

        [Header("Text & Buttons")]
        public TMP_Text endingText;
        public TMP_Text endingTitle;
        public Button advanceButton;
        public Button playAgainButton;

        [Header("Sprites — index order: VeryBad=0, Bad=1, Neutral=2, Good=3")]
        public Sprite[] panel1Sprites;
        public Sprite[] panel2Sprites;

        [Header("Ending Texts — index order: VeryBad=0, Bad=1, Neutral=2, Good=3")]
        [TextArea(3, 6)]
        public string[] endingTexts =
        {
            "Гинка изпадна в тежка депресия и цялата фирма усети как без нея всичко се разпада.",
            "Разходите те настигнаха, хората се демотивираха и проектът се разпадна.",
            "Просто още един ден, който минава и не оставя следа.",
            "Решенията ти вдигнаха морала и стабилизираха компанията."
        };

        [Header("Ending Texts Panel 2 — index order: VeryBad=0, Bad=1, Neutral=2, Good=3")]
        [TextArea(3, 6)]
        public string[] endingTexts2 =
        {
            "Мъжът на Гинка дойде в офиса… и всичко завърши с фатален край…",
            "Без пари и без екип, компанията обявява фалит, а ти оставаш само с празен офис и лоши решения.",
            "Утре пак същото. И вдругиден. И до пенсия.",
            "Празнувате успеха с тиймбилдинг в Дубай и всички говорят за следващия проект с усмивка."
        };

        public string[] endingTitles =
        {
            "Отмъщението за Гинка",
            "Краят на бюджета",
            "Какъв е смисъла в живота?",
            "Добре дошли в Дубай"
        };


        private int _currentIdx;

        void Awake()
        {
            panel.SetActive(false);
            advanceButton.onClick.AddListener(OnAdvanceClicked);
            playAgainButton.onClick.AddListener(OnPlayAgain);
        }

        public void Show(EndingType ending)
        {
            panel.SetActive(true);

            _currentIdx = (int)ending;
            int idx = _currentIdx; // VeryBad=0, Bad=1, Neutral=2, Good=3

            if (panel1Sprites != null && idx < panel1Sprites.Length && panel1Sprites[idx] != null)
                panel1Image.sprite = panel1Sprites[idx];
            if (panel2Sprites != null && idx < panel2Sprites.Length && panel2Sprites[idx] != null)
                panel2Image.sprite = panel2Sprites[idx];

            endingText.text = (endingTexts != null && idx < endingTexts.Length) ? endingTexts[idx] : "";
            endingTitle.text = (endingTitles != null && idx < endingTitles.Length) ? endingTitles[idx] : "";

            // Initial state: panel1 visible, panel2 hidden, text hidden
            panel2Group.alpha = 0f;
            panel2Image.gameObject.SetActive(false);
            playAgainButton.gameObject.SetActive(false);
            advanceButton.gameObject.SetActive(true);
        }

        void OnAdvanceClicked()
        {
            panel2Image.gameObject.SetActive(true);
            StartCoroutine(FadeInPanel2());
        }

        IEnumerator FadeInPanel2()
        {
            float elapsed = 0f;
            panel2Group.alpha = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                panel2Group.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            panel2Group.alpha = 1f;
            advanceButton.interactable = false;
            endingText.text = (endingTexts2 != null && _currentIdx < endingTexts2.Length) ? endingTexts2[_currentIdx] : "";

            playAgainButton.gameObject.SetActive(true);
        }

        void OnPlayAgain()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
