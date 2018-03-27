using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController {

    List<AbstractStatus> statuses = new List<AbstractStatus>();

    public void addStatus(AbstractCreature c, AbstractStatus s)
    {
        statuses.Add(s);
        s.applyStatus(c);
    }

    public void reduceStatusDuration(AbstractCreature c)
    {
        foreach (AbstractStatus s in statuses)
        {
            if (s.TurnsUntilRemoved > 0)
            {
                s.TurnsUntilRemoved -= 1;
            }

            if(s.TurnsUntilRemoved == 0)
            {
                s.removeStatus(c);
            }
        }
    }
}
