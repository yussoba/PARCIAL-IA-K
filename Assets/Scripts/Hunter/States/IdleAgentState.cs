using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAgentState : IState
{
    FSM<AgentStates> _fsm;
    private Hunter _hunter;

    public IdleAgentState(FSM<AgentStates> fsm, Hunter hunter)
    {
        _fsm = fsm;
        _hunter = hunter;
    }

    public void OnEnter()
    {
        _hunter.ChangeColor(Color.white);
        Debug.Log("Idle");
    }

    public void OnUpdate()
    {
        _hunter.RecoveringEnergy();
        
        if (_hunter.TargetOnSight() && _hunter.currentEnergy >= _hunter.maxEnergy)
        {
            _fsm.ChangeState(AgentStates.Chase);
        }
        
        if (_hunter.currentEnergy >= _hunter.maxEnergy)
        {
            _fsm.ChangeState(AgentStates.Patrol);
        }
    }
    
    public void OnExit()
    {
    }
}
