﻿using System.Collections;
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
            Debug.Log(s.StatusName + " " + s.TurnsUntilRemoved);
            if (s.TurnsUntilRemoved >= 0)
            {
                
                //Can be abstracted to DoT effects later
                if( s is Ignited)
                {
                    Ignited i = (Ignited)s;
                    c.UnderAttack(i.FireDamage);
                }
                s.TurnsUntilRemoved -= 1;
            }

            if(s.TurnsUntilRemoved == 0)
            {
                s.removeStatus(c);
            }
        }

        statuses.RemoveAll(s => s.TurnsUntilRemoved < 0 -1);

    }
}
