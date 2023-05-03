using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAgentState : IState
{
    FSM<AgentStates> _fsm;

    float _ticksToPatrol;

    float _timeToRecover = 0;

    public IdleAgentState(FSM<AgentStates> fsm)
    {
        _fsm = fsm;
    }

    public void OnEnter()
    {
        _timeToRecover = 0;

        Debug.Log("No tengo mas energia, voy a descansar");
    }

    public void OnUpdate()
    {
        RecoveringEnergy();
    }

    public void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }
    
    public void OnExit()
    {
        Debug.Log("Recupere toda mi energia, voy a patrullar");

    }

    void RecoveringEnergy()
    {
        _timeToRecover += Time.deltaTime;

        if (_timeToRecover >= 10)
        {
            _fsm.ChangeState(AgentStates.Patrol);
        }
    }

}
