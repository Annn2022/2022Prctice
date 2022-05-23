using UnityEngine;

public class IdleState : StateBase
{
    private Animator myAni;
    public IdleState(StateType stateType, Animator animator) : base(stateType)
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
        Debug.Log("进入idle");
        myAni.SetBool(AniName.equip_bool, false);
        FightController.Instence.isEquip = false;

    }

    public override void OnExit()
    {
       

    }

    public override void OnUpdate()
    {

    }
}


public class AttackIdleState : StateBase
{
    private Animator myAni;
    float time_attackIdle = 0;
    public AttackIdleState
    (StateType stateType, Animator animator) : base(stateType)
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
        myAni.SetBool(AniName.equip_bool, true);
        FightController.Instence.isEquip = true;
        time_attackIdle = 0;
    }

    public override void OnExit()
    {
        time_attackIdle = 0;
    }

    public override void OnUpdate()
    {
        time_attackIdle += Time.deltaTime;
        //停止一段时间移动
        if (time_attackIdle > Data.attackIdleTime)
        {
            FightController.Instence.SetStateFSM(StateType.IDLE);
        }
    }
}

public class AttackJumpState : StateBase
{
    private Animator myAni;
    private float timer = 0;
    
    public AttackJumpState
        (StateType stateType, Animator animator) : base(stateType)
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
        Debug.Log("进入进攻Jump");
        myAni.CrossFade("jump", 0.1f);
        timer = 0;
    }

    public override void OnExit()
    {
        timer = 0;
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        
        if (timer > 1f)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
        }
    }
}

public class GetHitState : StateBase
{
    private Animator myAni;

    private float timer;
    
    public GetHitState
        (StateType stateType, Animator animator) : base(stateType)
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
        Debug.Log("进入受击状态");
        myAni.CrossFade(AniName.GetHit, 0.1f);
        timer = 0;
    }

    public override void OnExit()
    {
        timer = 0;
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        //停止一段时间移动
        if (timer > Data.PlayerGetHitTime)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
            
        }
    }
}


