using UnityEngine;
using System;
using System.Collections;

public abstract class FSMState
{

    protected int type;
    protected float startTime;
    protected float delay;

    public abstract void enter();
    public abstract void exit();
    public abstract void update(float delta);
    public abstract void init();
    public abstract int checkTransitions();

    public float getTime()
    {
        return Time.time;
    }

    public FSMState()
    {
        this.startTime = -1;
    }

    public int getType()
    {
        return type;
    }

    public void startTimer()
    {
        startTime = getTime();
    }

    public void resetTimer()
    {
        startTime = -1;
    }
    // in msec
    protected float timeSinceStart()
    {
        if (startTime < 0) return 0;
        return (getTime() - startTime);
    }

    protected bool nextTurn()
    {
        return (delay - timeSinceStart()) <= 0;
    }

    public float timeToNextTurn()
    {
        return Mathf.Max(delay - timeSinceStart(), 0);
    }

    protected void setDelay(float delay)
    {
        // in secs
        this.delay = delay;
    }
}
