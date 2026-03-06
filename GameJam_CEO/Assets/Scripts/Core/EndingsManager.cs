using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class EndingsManager : MonoBehaviour
    {
        [Header("Special Ending Triggers")]
        public RequestData ginkaRequest;

        public EndingType DetermineEnding()
        {
            var gs = GameState.Instance;

            // VeryBad: Ginka was denied (her husband kills the CEO)
            if (ginkaRequest != null && gs.deniedRequests.Contains(ginkaRequest))
                return EndingType.VeryBad;

            // Count how many stats are up/down 20% from starting values
            // Starting: budget=100000, morale=80, people=30
            int statsUp = 0;
            int statsDown = 0;
            if (gs.budget >= 120000) statsUp++;   else if (gs.budget < 80000) statsDown++;
            if (gs.morale >= 96)    statsUp++;   else if (gs.morale < 64)    statsDown++;
            if (gs.people >= 36)    statsUp++;   else if (gs.people < 24)    statsDown++;

            if (statsUp >= 2)   return EndingType.Good;
            if (statsDown >= 2) return EndingType.Bad;
            return EndingType.Neutral;
        }
    }
}
