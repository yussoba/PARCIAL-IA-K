using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAgentState : IState
{
    Hunter _hunter;

    int _currentWaypoint;

    Transform[] _allWaypoints;

    int _sing;

    float _arriveDistance;

    FSM<AgentStates> _fsm;

    public PatrolAgentState(FSM<AgentStates> fsm, Hunter hunter, Transform[] allWaypoints)
    {
        _hunter = hunter;

        _fsm = fsm;

        _allWaypoints = allWaypoints;

        _sing = 1;

        _arriveDistance = _hunter.ArriveDistance * _hunter.ArriveDistance;

    }

    public void OnEnter()
    {
        _hunter.ChangeColor(Color.yellow);
        Debug.Log("Patrullo");
    }

    public void OnUpdate()
    {
        _hunter.UseEnergy();
        
        if (_hunter.currentEnergy < 0)
        {
            Debug.Log("Me quede sin energia: " + _hunter.currentEnergy);
            _fsm.ChangeState(AgentStates.Idle);
        }

        if (_hunter.TargetOnSight())
        {
            _fsm.ChangeState(AgentStates.Chase);
        }

        Movement();

        Debug.Log("Gaste 1 punto de energia, me quedan " + _hunter.currentEnergy + " puntos de energia.");
    }

    public void OnExit()
    {
        
    }

    void Movement()
    {
        Transform nextWaypoint = _allWaypoints[_currentWaypoint];

        var transform = _hunter.transform;
        var position = transform.position;
        Vector3 dir = nextWaypoint.position - position;

        dir.y = 0;

        transform.forward = dir;

        position += transform.forward * _hunter.Speed * Time.deltaTime;
        transform.position = position;

        if (dir.sqrMagnitude <= _arriveDistance)
        {
            /*Opcion B*/

            _currentWaypoint += _sing;

            if (_currentWaypoint > _allWaypoints.Length - 1 || _currentWaypoint < 0)
            {
                _sing *= -1;

                _currentWaypoint += _sing * 2;
            }
            //_fsm.ChangeState(AgentStates.Idle);
        }
    }
}
