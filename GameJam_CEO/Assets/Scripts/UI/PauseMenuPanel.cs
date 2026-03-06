using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CEOGame.UI
{
    public class PauseMenuPanel : MonoBehaviour
    {
        public GameObject panel;
        public Button resumeButton;
        public Button restartButton;
        public Button quitButton;

        void Awake()
        {
            panel.SetActive(false);
            resumeButton.onClick.AddListener(Hide);
            restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
            quitButton.onClick.AddListener(Application.Quit);
        }

        public void Show() => panel.SetActive(true);
        public void Hide() => panel.SetActive(false);
        public bool IsVisible => panel.activeSelf;
    }
}
