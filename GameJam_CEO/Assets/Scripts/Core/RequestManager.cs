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
                .OrderBy(r => HasPrerequisites(r) ? 1 : 0)
                .ToList();

            currentQueue = new Queue<RequestData>(eligible);
        }

        public void ServeNextRequest()
        {
            if (GameState.Instance.gameOver) return;

            BuildQueue();

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

        bool HasPrerequisites(RequestData request)
        {
            return (request.requiresApproved != null && request.requiresApproved.Length > 0)
                || (request.requiresDenied != null && request.requiresDenied.Length > 0);
        }

        bool ArePrerequisitesMet(RequestData request)
        {
            var gameState = GameState.Instance;

            if (request.requiresApproved != null)
            {
                foreach (var req in request.requiresApproved)
                {
                    if (!gameState.approvedRequests.Contains(req)) return false;
                }
            }

            if (request.requiresDenied != null)
            {
                foreach (var req in request.requiresDenied)
                {
                    if (!gameState.deniedRequests.Contains(req)) return false;
                }
            }

            return true;
        }
    }
}
