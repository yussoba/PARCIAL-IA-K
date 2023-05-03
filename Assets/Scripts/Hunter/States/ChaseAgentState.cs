using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAgentState : IState
{
    float _currentEnergy;
    
    FSM<AgentStates> _fsm;
    
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        //TargetOnSight()
        
        if (_currentEnergy >= 10)
        {
            Debug.Log("Puntos de energia: " + _currentEnergy);
            _fsm.ChangeState(AgentStates.Idle);
        }
    }

    public void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
