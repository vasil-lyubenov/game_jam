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
        public Button approveButton;
        public Button denyButton;

        public void ShowRequest(RequestData request)
        {
            gameObject.SetActive(true);

            var employee = request.requestingEmployee;
            if (employee.portrait != null)
                portraitImage.sprite = employee.portrait;
            nameText.text = employee.employeeName;
            dialogueText.text = request.requestDialogue;
            outcomeText.text = "";

            approveButton.interactable = true;
            denyButton.interactable = true;
        }

        public void ShowOutcome(string text)
        {
            approveButton.interactable = false;
            denyButton.interactable = false;
            outcomeText.text = text;
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}
