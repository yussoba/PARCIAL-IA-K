using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    FSM<AgentStates> _FSM;

    Material _myMaterial;
    Color _originalColor;

    [SerializeField] Transform[] _allWaypoints;
    [SerializeField] float _viewDistance;
    [SerializeField] float _distanceToAttack;

    private List<Boid> targets = new();

    public float Speed => _speed;
    public float currentEnergy;
    public float maxEnergy = 5;
    [SerializeField] float _speed;

    public float ArriveDistance => _arriveDistance;
    [SerializeField] float _arriveDistance;
    public Boid currentTarget;


    void Awake()
    {
        _FSM = new FSM<AgentStates>();

        _myMaterial = GetComponent<Renderer>().material;

        _originalColor = _myMaterial.color;
        targets.AddRange(FindObjectsOfType<Boid>());

        _FSM.AddState(AgentStates.Chase, new ChaseAgentState(_FSM, this));
        _FSM.AddState(AgentStates.Idle, new IdleAgentState(_FSM, this));
        _FSM.AddState(AgentStates.Patrol, new PatrolAgentState(_FSM, this, _allWaypoints));

        _FSM.ChangeState(AgentStates.Idle);
    }
    
    void Update()
    {
        _FSM.Update();
    }
    
    public bool TargetOnSight()
    {
        var minDistance = 0;
        foreach (var target in targets)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= _viewDistance)
            {
                currentTarget = target;
                return true;
            }
        }

        currentTarget = null;
        return false;
    }

    public bool TargetIsClose()
    {
        var minDistance = 0;
        foreach (var target in targets)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= _distanceToAttack)
            {
                return true;
            }
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Boid>())
        {
            targets.Remove(collision.gameObject.GetComponent<Boid>());
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _distanceToAttack);
    }

    public void ChangeColor(Color newColor)
    {
        _myMaterial.color = newColor;
    }
    
    public void Movement()
    {
        if (currentTarget == null)
            return;
        
        var dir = currentTarget.transform.position - transform.position;
        dir.y = 0;
        transform.forward = dir;
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    public void UseEnergy()
    {
        currentEnergy -= Time.deltaTime;
    }
    
    public void RecoveringEnergy()
    {
        currentEnergy += Time.deltaTime;
    }
}
