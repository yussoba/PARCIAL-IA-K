using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirTarget : MonoBehaviour
{
    [SerializeField] float _maxSpeed;

    [Range(0, 0.1f)]
    [SerializeField] float _maxForce;

    public Vector3 Velocity { get { return _velocity; } }
    Vector3 _velocity;

    void Start()
    {
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),0);

        AddForce(randomDir.normalized * _maxSpeed);
    }

    void Update()
    {
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;

        CheckBounds();
    }

    void CheckBounds()
    {
        transform.position = GameManager.Instance.SetObjectBoundPosition(transform.position);
    }

    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _velocity);
    }
}
