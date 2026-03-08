using System;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Ralica Data")]
        public EmployeeData ralicaData;

        [Header("Tutorial Dialogue")]
        [TextArea(2, 4)]
        public string[] tutorialLines = new string[]
        {
            "Добро утро! Аз съм Ралица от Човешки ресурси. Днес ще минават хора един по един с искания.",
            "Горе вдясно виждате таймера. Той ще тръгне чак когато първият служител влезе.",
            "Долу вдясно имате две листа хартия.",
            "Първият е ВИЗИТКАТА. Кратка справка за човека отпред с информация за позиция, заплата и щастие.",
            "Вторият е ЧАРШАФЪТ. Там е цялата компания, екипите и най-важното: кой с кого е в добри, неутрални или лоши отношения спрямо човека, който говори с вас.",
            "Вляво са общите показатели: Бюджет, Морал, Хора. Решенията ви ги променят. Имате бутон за помощ от HR. Можете да го ползвате 2 пъти на ден.",
            "Добре. Влизаме в режим. Успех!"
        };

        public bool TutorialActive { get; private set; }
        public event Action OnTutorialComplete;

        public void StartTutorial() => TutorialActive = true;

        public void EndTutorial()
        {
            TutorialActive = false;
            OnTutorialComplete?.Invoke();
        }
    }
}
