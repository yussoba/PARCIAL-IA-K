using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaypointAgent : MonoBehaviour
{

    [SerializeField] float _speed = 5;

    [SerializeField] Transform[] _allWaypoints;

    [SerializeField] float _arriveDistance;

    int _currentWaypoint;

    //bool _isBack;

    //int _sing = 1;

    Action _OnCurrentPath;

    private void Awake()
    {
        _OnCurrentPath = NormalPath;
    }
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        Transform nextWaypoint = _allWaypoints[_currentWaypoint];

        Vector3 dir = nextWaypoint.position - transform.position;

        dir.y = 0;

        transform.forward = dir;

        transform.position += transform.forward * _speed * Time.deltaTime;

        if (dir.sqrMagnitude <= _arriveDistance * _arriveDistance)
        {
            /*Opcion A*/

            _OnCurrentPath();

            /*Opcion B*/

            /*_currentWaypoint += _sing;

            if (_currentWaypoint > _allWaypoints.Length - 1 || _currentWaypoint < 0)
            {
                _sing *= -1;

                _currentWaypoint += _sing * 2;
            }*/

            /*Opcion C*/

            /*if (_isBack)
            {
                _currentWaypoint--;

                if (_currentWaypoint < 0)
                {
                    _isBack = false;
                    _currentWaypoint = 1;
                }  
            }
            else
            {
                _currentWaypoint++;

                if (_currentWaypoint >= _allWaypoints.Length)
                {
                    _isBack = true;

                    _currentWaypoint = _allWaypoints.Length - 2;
                }
            }*/
        }
    }

    void NormalPath()
    {
        _currentWaypoint++;

        if (_currentWaypoint >= _allWaypoints.Length)
        {
            _currentWaypoint--;

            _OnCurrentPath = BackwardPath;
        }
    }

    void BackwardPath()
    {
        _currentWaypoint--;

        if (_currentWaypoint < 0)
        {
            _currentWaypoint++;

            _OnCurrentPath = NormalPath;
        }
    }

}
