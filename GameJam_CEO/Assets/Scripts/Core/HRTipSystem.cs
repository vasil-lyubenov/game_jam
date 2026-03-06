using System;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class HRTipSystem : MonoBehaviour
    {
        public int tipsRemaining = 2;

        public event Action<string> OnTipUsed;

        public bool CanUseTip() => tipsRemaining > 0;

        public void UseTip(RequestData request)
        {
            if (tipsRemaining <= 0 || request == null) return;

            tipsRemaining--;
            OnTipUsed?.Invoke(request.hrTipInsight);
        }
    }
}
