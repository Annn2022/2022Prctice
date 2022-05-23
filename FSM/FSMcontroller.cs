using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateType
{
    IDLE,
    ATTACKIDLE,
    ATTACKJUMP,
    ROLL,
    MOVE,
    GETHIT,
    EQUIP,
    UNEQUIP,
    ATK04_1,
    ATK04_2,
    ATK04_3,
    ATK04_4,
    ATK04_5,
    DEFENSE,
    DEFCOMBO,

    MAX
}
public class FSMcontroller
{
    //当前的状态
    public StateBase currentState;

    private Dictionary<StateType, StateBase> allSaveState;

    public FSMcontroller()
    {
        allSaveState = new Dictionary<StateType, StateBase>();
    }

    public void AddState(StateBase state)
    {
        if (allSaveState.ContainsKey(state.GetState()))
            return;

        allSaveState.Add(state.GetState(), state);

    }
    /// <summary>
    /// 转换状态类型，传入下一个状态
    /// </summary>
    /// <param name="stateType">新状态</param>
    public void SetState(StateType stateType)
    {
        if (currentState == allSaveState[stateType])
            return;

        currentState?.OnExit();
        currentState = allSaveState[stateType];
        currentState?.OnEnter();
    }

    public StateType GetCurState()
    {
        return currentState.GetState();
    }

    public void OnTick()
    {
        currentState?.OnUpdate();
    }
}
