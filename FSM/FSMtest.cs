using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMtest : MonoBehaviour
{
    private FSMcontroller fsm;
    private Animator animator;
    private void Awake()
    {
        fsm = new FSMcontroller();
        fsm.AddState(new IdleState(StateType.IDLE,animator));
        fsm.SetState(StateType.IDLE);
    }
    private void Update()
    {
        fsm.OnTick();
    }
}
