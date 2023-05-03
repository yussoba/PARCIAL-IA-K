using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    [SerializeField] Actions _actionSelected;

    public enum Actions
    {
        Rest,
        Patrol,
        Chase,
        Attack
    }

    public override void Execute(Hunter hunter)
    {
        /*switch (_actionSelected)
        {   
            case Actions.Rest:
                hunter.Rest();
                break;

            case Actions.Patrol:
                hunter.Patrol();
                break;

            case Actions.Chase:
                hunter.Chase();
                break;

            case Actions.Attack:
                hunter.Attack();
                break;
        }*/
    }
}

