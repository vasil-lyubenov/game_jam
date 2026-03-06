using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class EndingsManager : MonoBehaviour
    {
        public EndingType DetermineEnding()
        {
            var gs = GameState.Instance;

            // Good ending
            if (gs.budget >= 800 && gs.morale >= 55 && gs.people >= 38)
                return EndingType.Good;

            // Very bad ending
            if (gs.budget < 500 && gs.morale < 30 && gs.people < 25)
                return EndingType.VeryBad;

            // Bad ending
            if (gs.budget < 650 || gs.morale < 40 || gs.people < 32)
                return EndingType.Bad;

            return EndingType.Neutral;
        }
    }
}
