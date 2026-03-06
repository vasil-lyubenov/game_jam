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
        public Button advanceButton;
        public Button playAgainButton;

        [Header("Sprites — index order: VeryBad=0, Bad=1, Neutral=2, Good=3")]
        public Sprite[] panel1Sprites;
        public Sprite[] panel2Sprites;

        [Header("Ending Texts — index order: VeryBad=0, Bad=1, Neutral=2, Good=3")]
        [TextArea(3, 6)]
        public string[] endingTexts =
        {
            "Отказа на Гинка имаше последствия. Тя напусна. А след нея — и съпругът й.",
            "Бюджетът падна, хората се разочароваха. Компанията едва оцеля.",
            "Справихте се. Не блестящо, но компанията продължава.",
            "Страхотни решения. Компанията процъфтява и екипът е по-силен от всякога."
        };

        void Awake()
        {
            panel.SetActive(false);
            advanceButton.onClick.AddListener(OnAdvanceClicked);
            playAgainButton.onClick.AddListener(OnPlayAgain);
        }

        public void Show(EndingType ending)
        {
            panel.SetActive(true);

            int idx = (int)ending; // VeryBad=0, Bad=1, Neutral=2, Good=3

            if (panel1Sprites != null && idx < panel1Sprites.Length && panel1Sprites[idx] != null)
                panel1Image.sprite = panel1Sprites[idx];
            if (panel2Sprites != null && idx < panel2Sprites.Length && panel2Sprites[idx] != null)
                panel2Image.sprite = panel2Sprites[idx];

            endingText.text = (endingTexts != null && idx < endingTexts.Length) ? endingTexts[idx] : "";

            // Initial state: panel1 visible, panel2 hidden, text hidden
            panel2Group.alpha = 0f;
            panel2Image.gameObject.SetActive(false);
            endingText.gameObject.SetActive(false);
            playAgainButton.gameObject.SetActive(false);
            advanceButton.gameObject.SetActive(true);
        }

        void OnAdvanceClicked()
        {
            advanceButton.gameObject.SetActive(false);
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

            endingText.gameObject.SetActive(true);
            playAgainButton.gameObject.SetActive(true);
        }

        void OnPlayAgain()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
