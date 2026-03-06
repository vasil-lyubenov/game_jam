using UnityEngine;
using TMPro;

namespace CEOGame.UI
{
    public class CompanyPanel : MonoBehaviour
    {
        public TMP_Text logText;

        System.Text.StringBuilder logBuilder = new();

        public void AddLogEntry(string entry)
        {
            logBuilder.AppendLine(entry);
            logText.text = logBuilder.ToString();
        }

        public void ClearLog()
        {
            logBuilder.Clear();
            logText.text = "";
        }
    }
}
