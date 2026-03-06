using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CEOGame.Data;

namespace CEOGame.Core
{
    public class RequestManager : MonoBehaviour
    {
        [Header("All Requests")]
        public RequestData[] allRequests;

        Queue<RequestData> currentQueue = new();
        public RequestData CurrentRequest { get; private set; }

        public event Action<RequestData> OnRequestServed;
        public event Action OnNoMoreRequests;

        public void BuildQueue()
        {
            var gs = GameState.Instance;
            var eligible = allRequests
                .Where(r => ArePrerequisitesMet(r))
                .Where(r => !gs.approvedRequests.Contains(r) && !gs.deniedRequests.Contains(r))
                .OrderByDescending(r => r.priority)
                .ToList();

            currentQueue = new Queue<RequestData>(eligible);
        }

        public void ServeNextRequest()
        {
            if (GameState.Instance.gameOver) return;

            if (currentQueue.Count > 0)
            {
                CurrentRequest = currentQueue.Dequeue();
                OnRequestServed?.Invoke(CurrentRequest);
            }
            else
            {
                CurrentRequest = null;
                OnNoMoreRequests?.Invoke();
            }
        }

        bool ArePrerequisitesMet(RequestData request)
        {
            var gs = GameState.Instance;

            if (request.requiresApproved != null)
            {
                foreach (var req in request.requiresApproved)
                {
                    if (!gs.approvedRequests.Contains(req)) return false;
                }
            }

            if (request.requiresDenied != null)
            {
                foreach (var req in request.requiresDenied)
                {
                    if (!gs.deniedRequests.Contains(req)) return false;
                }
            }

            return true;
        }
    }
}
