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
            if (gs.budget >= 900 && gs.morale >= 60 && gs.people >= 40)
                return EndingType.Good;

            // Very bad ending
            if (gs.budget < 400 && gs.morale < 25 && gs.people < 20)
                return EndingType.VeryBad;

            // Bad ending
            if (gs.budget < 600 || gs.morale < 35 || gs.people < 30)
                return EndingType.Bad;

            return EndingType.Neutral;
        }
    }
}
