using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionNode : Node
{
    public enum Questions
    {
        SeeTarget,
        IsClose
    }

    [SerializeField] Node trueNode;
    [SerializeField] Node falseNode;

    [SerializeField] Questions question;

    public override void Execute(Hunter hunter)
    {
        switch(question)
        {
            case Questions.SeeTarget:
                if (hunter.TargetOnSight()) trueNode.Execute(hunter);
                else falseNode.Execute(hunter);
                break;

            case Questions.IsClose:
                if (hunter.TargetIsClose()) trueNode.Execute(hunter);
                else falseNode.Execute(hunter);
                break;
        }
    }
}
