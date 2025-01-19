using DataPostAPI.Enums;
using DataPostAPI.Models;
using System.Collections.Generic;

namespace DataPostAPI.Handlers
{
    public delegate void AnomalyEventHandler(AnomalyEvent anomalyEvent);

    public class AnomalyDetectionSystem
    {
        private PriorityQueue<AnomalyEvent, int> _eventQueue;
        public event AnomalyEventHandler OnAnomalyDetected;

        public AnomalyDetectionSystem()
        {
            _eventQueue = new PriorityQueue<AnomalyEvent, int>();
        }

        public void DetectAnomaly(AnomalyEvent anomalyEvent)
        {
            // Add to priority queue
            _eventQueue.Enqueue(anomalyEvent, (int)anomalyEvent.Priority);

            // Raise the event
            OnAnomalyDetected?.Invoke(anomalyEvent);
        }

        public AnomalyEvent GetNextHighestPriorityEvent()
        {
            return _eventQueue.TryDequeue(out AnomalyEvent nextEvent, out int priority)
                ? nextEvent
                : null;
        }
    }
}
