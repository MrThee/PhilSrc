using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

public class TimedChangeQueue<T,C> {

    private Queue<(float delay, C changes)> m_changeQueue;
    private float? mc_currentDelayCountdown;
    private System.Action<T,C> m_ApplyChange;

    public TimedChangeQueue(int queueCapacity, System.Action<T,C> ApplyChange){
        m_changeQueue = new Queue<(float,C)>(queueCapacity);
        mc_currentDelayCountdown = null;
        m_ApplyChange = ApplyChange;
    }

    public void EnqueueChange(float delay, ref C changes){
        m_changeQueue.Enqueue((delay, changes));
    }

    public void UpdateAndMaybeChange(float dt, T maybeChangeThis ) {
        if(m_changeQueue.Count == 0){
            return;
        }
        if(mc_currentDelayCountdown.HasValue == false){
            mc_currentDelayCountdown = m_changeQueue.Peek().delay;
        }
        if(mc_currentDelayCountdown.HasValue){
            mc_currentDelayCountdown -= dt;
            // Notice the use of a while loop here.
            // Allows consecutive changes w/ 0 delay to occur on the same frame.
            while(mc_currentDelayCountdown.HasValue && mc_currentDelayCountdown <= 0f){
                // trigger the action
                var c = m_changeQueue.Dequeue().changes;
                m_ApplyChange(maybeChangeThis, c);

                // Ready the next action + delay.
                // If the next delay is 0 or less than the dt residue, we loop again.
                // If there is no next action, then the countdown field no longer has a value.
                // If there IS an action AND the delay is larger than the dt residue (common case), we simply
                // read a delay and exit the loop.
                mc_currentDelayCountdown = (m_changeQueue.Count > 0) ? 
                    // at this point, delay countdown contains the dt residue w/ the correct negative signage.
                    (m_changeQueue.Peek().delay + mc_currentDelayCountdown) 
                    : null
                ;
            }
        }
    }

    public void ClearAndReset(){
        m_changeQueue.Clear();
        mc_currentDelayCountdown = null;
    }

    public bool hasActionsQueued => (m_changeQueue.Count > 0);
} // End TimedActionQueue

}