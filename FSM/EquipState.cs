using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipState : StateBase
{
    private Animator myAni;
    public EquipState(StateType stateType, Animator animator) : base(stateType)
    {
        this.stateType = stateType;
        myAni = animator;
    }

    public override StateType GetState()
    {
        return stateType;
    }

    public override void OnEnter()
    {
        Debug.Log("进入Equip");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}

public class UnequipState : StateBase
{
    private Animator myAni;
    public UnequipState(StateType stateType, Animator animator) : base(stateType)
    {
        this.stateType = stateType;
        myAni = animator;
    }

    public override StateType GetState()
    {
        return stateType;
    }

    public override void OnEnter()
    {
        Debug.Log("进入unEquip");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}
