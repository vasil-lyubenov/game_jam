using UnityEngine;
using UnityEngine.UI;
using CEOGame.Core;
using CEOGame.Data;

namespace CEOGame.UI
{
    public class EnvironmentDisplay : MonoBehaviour
    {
        [SerializeField] Image backgroundImage;
        [SerializeField] Image deskImage;
        [SerializeField] DayCycleManager dayCycleManager;

        [Header("Backgrounds (Morning / Midday / Sunset)")]
        [SerializeField] Sprite[] backgrounds = new Sprite[3];

        [Header("Desks (Morning / Midday / Sunset)")]
        [SerializeField] Sprite[] desks = new Sprite[3];

        void Start()
        {
            if (dayCycleManager != null)
                dayCycleManager.OnPhaseChanged += SetEnvironment;
        }

        void OnDestroy()
        {
            if (dayCycleManager != null)
                dayCycleManager.OnPhaseChanged -= SetEnvironment;
        }

        public void SetEnvironment(TimeOfDay phase)
        {
            int index = (int)phase;
            if (index < backgrounds.Length && backgrounds[index] != null)
                backgroundImage.sprite = backgrounds[index];
            if (index < desks.Length && desks[index] != null)
                deskImage.sprite = desks[index];
        }
    }
}
