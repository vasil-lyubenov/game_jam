using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace CEOGame.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public Button startButton;
        public Button quitButton;

        void Start()
        {
            startButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
            quitButton.onClick.AddListener(() => Application.Quit());
        }
    }
}
