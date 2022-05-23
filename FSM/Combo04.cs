using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo04_1 : StateBase
{
    private Animator myAni;
    private float timer = 0;
    public Combo04_1(StateType stateType, Animator animator) : base(stateType)
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
        myAni.CrossFade(AniName.combo_04_1, 0.1f);
        

    }

    public override void OnExit()
    {
        timer = 0;
        FightController.Instence.isAttack = false;

    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 0.07f && timer <= 0.40f)
        {
            FightController.Instence.isAttack = true;
            
        }
        else if (timer > 0.50f)
        {
            FightController.Instence.isAttack = false;
        }
        if (timer > Data.combo0401Time)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
        }
    }
}

public class Combo04_2 : StateBase
{
    private Animator myAni;
    private float timer = 0;
    public Combo04_2(StateType stateType, Animator animator) : base(stateType)
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
        myAni.CrossFade(AniName.combo_04_2, 0.1f);

    }

    public override void OnExit()
    {
        timer = 0;
        FightController.Instence.isAttack = false;

    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 0.07f && timer <= 0.40f)
        {
            FightController.Instence.isAttack = true;
            
        }
        else if (timer >= 0.50f)
        {
            FightController.Instence.isAttack = false;
        }
        
        if (timer > Data.combo0402Time)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
        }
    }
}

public class Combo04_3 : StateBase
{
    private Animator myAni;
    
    private float timer = 0;
    public Combo04_3(StateType stateType, Animator animator) : base(stateType)
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
        myAni.CrossFade(AniName.combo_04_3, 0.1f);

    }

    public override void OnExit()
    {
        timer = 0;
        FightController.Instence.isAttack = false;

    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 0.50f && timer <= 0.80f)
        {
            FightController.Instence.isAttack = true;
            
        }
        else if (timer > 0.70f)
        {
            FightController.Instence.isAttack = false;
        }
        
        
        if (timer > Data.combo0403Time)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
        }
    }
}

public class Combo04_4 : StateBase
{
    private Animator myAni;
    
    private float timer = 0;
    public Combo04_4(StateType stateType, Animator animator) : base(stateType)
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
        myAni.CrossFade(AniName.combo_04_4, 0.1f);
        

    }

    public override void OnExit()
    {
        timer = 0;
        FightController.Instence.isAttack = false;

    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        
        if (timer >= 0.50f && timer <= 0.80f)
        {
            FightController.Instence.isAttack = true;
            
        }
        else if (timer > 0.80f)
        {
            FightController.Instence.isAttack = false;
        }
        
        if (timer > Data.combo0404Time)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
        }
    }
}

public class Combo04_5 : StateBase
{
    private Animator myAni;
    
    private float timer = 0;
    public Combo04_5(StateType stateType, Animator animator) : base(stateType)
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
        Debug.Log("进入A5");
        myAni.CrossFade(AniName.combo_04_5, 0.1f);
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
        if (timer > Data.combo0405Time)
        {
            FightController.Instence.SetStateFSM(StateType.ATTACKIDLE);
        }
    }
}

