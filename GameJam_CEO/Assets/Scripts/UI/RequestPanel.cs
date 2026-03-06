using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class RequestPanel : MonoBehaviour
    {
        [Header("Display")]
        public Image portraitImage;
        public TMP_Text nameText;
        public TMP_Text dialogueText;
        public TMP_Text outcomeText;

        [Header("Buttons")]
        public Button nextButton;
        public Button approveButton;
        public Button denyButton;

        string[] lines;
        int currentLine;

        void Awake()
        {
            nextButton.onClick.AddListener(AdvanceLine);
        }

        public void ShowRequest(RequestData request)
        {
            gameObject.SetActive(true);

            lines = request.dialogueLines;
            currentLine = 0;

            var employee = request.requestingEmployee;
            if (employee.portrait != null)
                portraitImage.sprite = employee.portrait;
            nameText.text = employee.employeeName;
            // outcomeText.text = "";

            ShowCurrentLine();
        }

        void ShowCurrentLine()
        {
            dialogueText.text = lines != null && lines.Length > 0 ? lines[currentLine] : "";

            bool isLast = lines == null || lines.Length == 0 || currentLine >= lines.Length - 1;
            nextButton.gameObject.SetActive(!isLast);
            approveButton.gameObject.SetActive(isLast);
            denyButton.gameObject.SetActive(isLast);
        }

        void AdvanceLine()
        {
            if (lines == null || currentLine >= lines.Length - 1) return;
            currentLine++;
            ShowCurrentLine();
        }

        public void ShowOutcome(string text)
        {
            approveButton.interactable = false;
            denyButton.interactable = false;
            nextButton.gameObject.SetActive(false);
            // outcomeText.text = text;
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}
