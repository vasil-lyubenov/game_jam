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

        public void UseTip(EmployeeData employee)
        {
            if (tipsRemaining <= 0 || employee == null) return;
            if (string.IsNullOrEmpty(employee.hrTip)) return;

            tipsRemaining--;
            OnTipUsed?.Invoke(employee.hrTip);
        }
    }
}
