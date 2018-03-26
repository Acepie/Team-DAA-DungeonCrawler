using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour {

    List<AbstractStatus> statuses;

    // Use this for initialization
    void Start()
    {
        statuses = new List<AbstractStatus>();
    }

    public void addStatus(AbstractStatus s)
    {
        statuses.Add(s);
        s.applyStatus(this.GetComponent<AbstractCreature>(), s.StatusDuration);
    }

    public void reduceStatusDuration()
    {
        foreach (AbstractStatus s in statuses)
        {
            if (s.TurnsUntilRemoved > 0)
            {
                s.TurnsUntilRemoved -= 1;
            }
            else
            {
                s.removeStatus(this.GetComponent<AbstractCreature>());
            }
        }
    }
}
