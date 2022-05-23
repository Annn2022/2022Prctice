using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    protected StateType stateType;
    public StateBase(StateType stateType)
    {
        this.stateType = stateType;
    }

    public abstract StateType GetState();
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();

    
}
