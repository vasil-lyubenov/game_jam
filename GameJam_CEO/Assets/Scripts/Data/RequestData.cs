using UnityEngine;

namespace CEOGame.Data
{
    [CreateAssetMenu(fileName = "NewRequest", menuName = "CEO Game/Request Data")]
    public class RequestData : ScriptableObject
    {
        [Header("Request Info")]
        public EmployeeData requestingEmployee;
        public RequestCategory category;
        [TextArea(3, 6)]
        public string requestDialogue;

        [Header("Scheduling")]
        public int earliestTurn;
        public int priority;

        [Header("Approve Outcome")]
        public DecisionOutcome approveOutcome;

        [Header("Deny Outcome")]
        public DecisionOutcome denyOutcome;

        [Header("Prerequisites")]
        [Tooltip("Requests that must have been approved for this to appear")]
        public RequestData[] requiresApproved;
        [Tooltip("Requests that must have been denied for this to appear")]
        public RequestData[] requiresDenied;
    }
}
