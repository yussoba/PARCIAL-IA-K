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

    float _currentEnergy;

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
        _currentEnergy = 0;

        _hunter.ChangeColor(Color.yellow);
    }

    public void OnUpdate()
    {
        _currentEnergy += Time.deltaTime;

        Movement();

        Debug.Log("Gaste 1 punto de energia, me quedan " + _currentEnergy + " puntos de energia.");

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
        _hunter.RestoreColor();
    }

    void Movement()
    {
        Transform nextWaypoint = _allWaypoints[_currentWaypoint];

        Vector3 dir = nextWaypoint.position - _hunter.transform.position;

        dir.y = 0;

        _hunter.transform.forward = dir;

        _hunter.transform.position += _hunter.transform.forward * _hunter.Speed * Time.deltaTime;

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
