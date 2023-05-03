using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuitAgent : MonoBehaviour
{
    [SerializeField] float _maxSpeed;

    [Range(0f, 0.1f)]
    [SerializeField] float _maxSeekForce;
    [Range(0f, 0.1f)]
    [SerializeField] float _maxPursuitForce;

    [Range(0f, 0.1f)]
    [SerializeField]

    Vector3 _velocity;

    [SerializeField] RandomDirTarget _pursuitTarget;

    void Update()
    {

        AddForce(Pursuit());

        transform.position += _velocity * Time.deltaTime;

        transform.forward = _velocity;
    }

    Vector3 Pursuit()
    {
        //Calcula nuestro Desire en base al proximo frame del boid

        Vector3 futurePos = _pursuitTarget.transform.position + _pursuitTarget.Velocity;

        Vector3 agentToTarget = _pursuitTarget.transform.position - transform.position;

        //Si la distancia al boid es <= a la velocidad del boid

        if (agentToTarget.sqrMagnitude <= futurePos.sqrMagnitude)
        {
            Debug.DrawLine(transform.position, _pursuitTarget.transform.position, Color.red);
            return Seek(_pursuitTarget.transform.position);
        }

        Debug.DrawLine(transform.position, futurePos, Color.white);

        Vector3 desired = futurePos - transform.position;

        desired.Normalize();

        desired *= _maxSpeed;

        //Steering
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxPursuitForce);


        return steering;

    }

    Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desired = targetPosition - transform.position;
        desired.Normalize();
        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxSeekForce);

        return steering;
    }


    void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }
}
