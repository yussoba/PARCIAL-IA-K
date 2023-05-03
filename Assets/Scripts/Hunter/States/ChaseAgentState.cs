using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAgentState : IState
{
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        
        
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
