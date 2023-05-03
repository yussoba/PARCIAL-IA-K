using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public GameObject food;
    //public float foodVisionRadius = 0;

    Vector3 _velocity;
    [SerializeField] float _maxSpeed;

    [Range(0f, 0.1f), SerializeField]
    float _maxForce;

    [Range(0f, 0.1f)]
    [SerializeField] float _maxSeekForce;
    [Range(0f, 0.1f)]
    [SerializeField] float _maxPursuitForce;

    [SerializeField] RandomDirTarget _pursuitTarget;

    void Start()
    {
        food = FindClosestFood();

        BoidsManager.Instance.RegisterNewBoid(this);

        Vector3 random = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        AddForce(random.normalized * _maxSpeed);
    }

    void Update()
    {
        AddForce(Separation() * BoidsManager.Instance.SeparationWeight +
             Alignment() * BoidsManager.Instance.AlignmentWeight +
             Cohesion() * BoidsManager.Instance.CohesionWeight +
             Arrive() * BoidsManager.Instance.ArriveWeight +
             Evade()* BoidsManager.Instance.EvadeWeight);

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;

        CheckBounds(); 
    }

    void CheckBounds()
    {
        transform.position = GameManager.Instance.SetObjectBoundPosition(transform.position);
    }

    Vector3 Separation()
    {
        //Variable donde vamos a recolectar todas las direcciones entre los flockmates 
        Vector3 desired = Vector3.zero;

        //Por cada boid
        foreach (Boid boid in BoidsManager.Instance.AllBoids)
        {
            //Si soy este boid a chequear, ignoro y sigo la iteracion
            if (boid == this) continue;

            //Saco la direccion hacia el boid
            Vector3 dirToBoid = boid.transform.position - transform.position;

            //Si esta dentro del rango de vision
            if (dirToBoid.sqrMagnitude <= BoidsManager.Instance.ViewRadius)
            {
                //En este caso me resto porque quiero separarme hacia el lado contrario
                desired -= dirToBoid;
            }
        }
        if (desired == Vector3.zero) return desired;

        return CalculateSteering(desired);
    }

    Vector3 Alignment()
    {
        //Variable donde vamos a recolectar todas las direcciones entre los flockmates 
        Vector3 desired = Vector3.zero;

        //Contador para acumular cantidad de boids a promediar
        int count = 0;

        //Por cada boid
        foreach (Boid boid in BoidsManager.Instance.AllBoids)
        {
            //Si soy este boid a chequear, ignoro y sigo la iteracion
            if (boid == this) continue;

            //Saco la direccion hacia el boid
            Vector3 dirToBoid = boid.transform.position - transform.position;

            //Si esta dentro del rango de vision
            if (dirToBoid.sqrMagnitude <= BoidsManager.Instance.ViewRadius)
            {
                //Sumo la direccion hacia donde esta yendo el boid
                desired += boid._velocity;

                //Sumo uno mas a mi contador para promediar
                count++;
            }
        }
        if (count == 0) return desired;

        //Promediamos todas las direcciones
        desired /= count;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion()
    {
        //Variable donde vamos a recolectar todas las direcciones entre los flockmates 
        Vector3 desired = Vector3.zero;

        //Contador para acumular cantidad de boids a promediar
        int count = 0;

        foreach (Boid boid in BoidsManager.Instance.AllBoids)
        {
            //Si soy este boid a chequear, ignoro y sigo la iteracion
            if (boid == this) continue;

            //Saco la direccion hacia el boid
            Vector3 dirToBoid = boid.transform.position - transform.position;

            //Si esta dentro del rango de vision
            if (dirToBoid.sqrMagnitude <= BoidsManager.Instance.ViewRadius)
            {
                //Sumo la posicion de cada boid
                desired += boid.transform.position;

                //Sumo uno mas a mi contador para promediar
                count++;
            }
        }
        if (count == 0) return desired;

        //Promediamos todas las posiciones
        desired /= count;

        //Restamos nuestra posicion para que tambien sea parte de la Cohesion
        desired -= transform.position;

        return CalculateSteering(desired);
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        return Vector3.ClampMagnitude((desired.normalized * _maxSpeed) - _velocity, _maxForce);
    }

    void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    Vector3 Arrive()
    {
        Vector3 desired = Vector3.zero;

        if (food != null)
        {
            Vector3 dirToFood = food.transform.position - transform.position;
            float distance = dirToFood.magnitude;

            if (distance < 1f)
            {
                desired = -_velocity.normalized * _maxSpeed * (distance / 1f);
            }
            else
            {
                desired = dirToFood.normalized * _maxSpeed * (distance / 5f);
            }
        }
        return CalculateSteering(desired);
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

    Vector3 Evade()
    {
        return -Pursuit();
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

    GameObject FindClosestFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        GameObject closestFood = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject food in foods)
        {
            float distance = Vector3.Distance(transform.position, food.transform.position);
            if (distance < closestDistance)
            {
                closestFood = food;
                closestDistance = distance;
            }
        }
        return closestFood;
    }

    private void OnDrawGizmos()
    {
        if (!BoidsManager.Instance) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(BoidsManager.Instance.ViewRadius));
    }
}
