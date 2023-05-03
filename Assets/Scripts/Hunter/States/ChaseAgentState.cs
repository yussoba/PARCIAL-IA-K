using UnityEngine;

public class ChaseAgentState : IState
{
    private FSM<AgentStates> _fsm;
    private Hunter _hunter;

    public ChaseAgentState(FSM<AgentStates> fsm, Hunter hunter)
    {
        _fsm = fsm;
        _hunter = hunter;
    }

    public void OnEnter()
    {
        _hunter.ChangeColor(Color.red);
        Debug.Log("Empiezo a perseguir target");
    }

    public void OnUpdate()
    {
        _hunter.UseEnergy();
        _hunter.Movement();
        
        if (_hunter.currentEnergy < 0 || !_hunter.TargetOnSight())
        {
            Debug.Log("Puntos de energia: " + _hunter.currentEnergy);
            _fsm.ChangeState(AgentStates.Idle);
        }  
    }

    public void OnExit()
    {
        
    }
}
