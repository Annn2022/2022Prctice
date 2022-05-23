using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseState : StateBase
{
    private Animator myAni;

    private float timer;
    public DefenseState(StateType stateType, Animator animator) : base(stateType)
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
        myAni.CrossFade(AniName.defense, 0.1f);
        FightController.Instence.isDefense = true;
    }

    public override void OnExit()
    {
        timer = 0;
        FightController.Instence.defSuccess = false;
        FightController.Instence.isDefense = false;
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > Data.defenseTime)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
        }
    }
}

public class DefComboState : StateBase
{
    private Animator myAni;

    private float timer;
    public DefComboState(StateType stateType, Animator animator) : base(stateType)
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
        myAni.CrossFade(AniName.defCombo, 0.1f);
        FightController.Instence.isAttack = true;

    }

    public override void OnExit()
    {
        timer = 0;
        FightController.Instence.isAttack = false;

    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > Data.defComboTime)
        {
            FightController.Instence.SetStateFSM(StateType.IDLE);
        }
    }
}

public class RollState : StateBase
{
    private Animator myAni;

    private float timer;
    public RollState(StateType stateType, Animator animator) : base(stateType)
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
        myAni.CrossFade(AniName.Roll, 0.1f);

    }

    public override void OnExit()
    {
        timer = 0;

    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > Data.rollTime)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
        }
    }
}
