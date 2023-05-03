using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float _boundHeight;
    [SerializeField] float _boundWidth;

    public static GameManager Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }

    public Vector3 SetObjectBoundPosition(Vector3 pos)
    {
        float z = _boundHeight / 2;
        float x = _boundWidth / 2;

        if (pos.z > z) pos.z = - z;
        else if (pos.z < -z) pos.z = z;

        if (pos.x > x) pos.x = - x;
        else if (pos.x < -x) pos.x = x;

        pos.y = 0;
        return pos;

    }

    private void OnDrawGizmos()
    {
        float z = _boundHeight / 2;
        float x = _boundWidth / 2;

        Vector3 topLeft = new Vector3(-x, 0, z);
        Vector3 topRight = new Vector3(x, 0, z);
        Vector3 botRight = new Vector3(x, 0, -z);
        Vector3 BotLeft = new Vector3(-x, 0, -z);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(botRight, BotLeft);
        Gizmos.DrawLine(BotLeft, topLeft);


    }

}
