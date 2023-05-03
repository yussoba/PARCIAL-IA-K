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

    [SerializeField] Transform _target;

    [SerializeField] Node _decisionTreeStart;

    public float Speed => _speed;
    [SerializeField] float _speed;

    public float ArriveDistance => _arriveDistance;
    [SerializeField] float _arriveDistance;


    void Awake()
    {
        _FSM = new FSM<AgentStates>();

        _myMaterial = GetComponent<Renderer>().material;

        _originalColor = _myMaterial.color;

        IState idle = new IdleAgentState(_FSM);
        _FSM.AddState(AgentStates.Idle, idle);

        //IState patrol = new PatrolAgentState(_FSM, this, _allWaypoints);
        _FSM.AddState(AgentStates.Patrol, new PatrolAgentState(_FSM, this, _allWaypoints));

        _FSM.ChangeState(AgentStates.Idle);
    }
    void Start()
    {
        
    }

    void Update()
    {
        _FSM.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _decisionTreeStart.Execute(this);
            //GetDecision();
        }
    }

    public void Rest()
    {
        Debug.Log("Decision: Rest");
    }

    public void Patrol()
    {
        Debug.Log("Decision: Patrol");
    }

    public void Chase()
    {
        Debug.Log("Decision: Chase");
    }

    public void Attack()
    {
        Debug.Log("Decision: Attack");
    }

    public bool TargetOnSight()
    {
        return Vector3.Distance(transform.position, _target.position) <= _viewDistance;
    }

    public bool TargetIsClose()
    {
        return Vector3.Distance(transform.position, _target.position) <= _distanceToAttack;
    }

    /*void GetDecision()
    {
        if (_life > 10)
        {
            float distance = Vector3.Distance(transform.position, _target.position);

            if (TargetOnSight())
            {
                if (TargetIsClose())
                {
                    Attack();
                }
                else
                {
                    Chase();
                }
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            Rest();
        }
    }*/

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

    public void RestoreColor()
    {
        _myMaterial.color = _originalColor;
    }

}
