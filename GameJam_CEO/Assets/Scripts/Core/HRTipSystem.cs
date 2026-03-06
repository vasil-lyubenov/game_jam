using System;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class HRTipSystem : MonoBehaviour
    {
        public int tipsRemaining = 2;

        public event Action<EmployeeData> OnTraitRevealed;

        public bool CanUseTip() => tipsRemaining > 0;

        public void UseTip(EmployeeData employee)
        {
            if (tipsRemaining <= 0 || employee == null) return;

            tipsRemaining--;
            employee.traitsRevealed = true;
            OnTraitRevealed?.Invoke(employee);
        }
    }
}
