using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAgent : MonoBehaviour
{
    public float velocidadMaxima = 5f;
    public float radioVision = 5f;
    public float radioSeparacion = 2f;
    public float distanciaArrive = 1f;

    private Vector3 velocidad;
    private Vector3 aceleracion;
    private List<Transform> agentesEnRadio;
    private Transform objetivoComida;
    private bool hayComidaCerca;
    private bool hayCazadorCerca;



    void Update()
    {
        // Detectar agentes y cazadores en el radio de visión
        DetectarAgentes();
        DetectarCazadores();

        // Aplicar las reglas de flocking
        Flocking();

        // Mover el agente
        Mover();
    }


    void DetectarAgentes()
    {
        agentesEnRadio.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radioVision);
        foreach (Collider collider in colliders)
        {
            if (collider != this.GetComponent<Collider>())
            {
                Transform agente = collider.transform;
                if (agente.CompareTag("Agente"))
                {
                    agentesEnRadio.Add(agente);
                }
            }
        }
    }

    void DetectarCazadores()
    {
        hayCazadorCerca = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radioVision);
        foreach (Collider collider in colliders)
        {
            if (collider != this.GetComponent<Collider>())
            {
                Transform cazador = collider.transform;
                if (cazador.CompareTag("Cazador"))
                {
                    hayCazadorCerca = true;
                }
            }
        }
    }
    void Flocking()
    {
        Vector3 cohesion = Vector3.zero;
        Vector3 alineacion = Vector3.zero;
        Vector3 separacion = Vector3.zero;
        int count = 0;

        foreach (Transform agente in agentesEnRadio)
        {
            float distancia = Vector3.Distance(transform.position, agente.position);
            if (distancia > 0 && distancia < radioVision)
            {
                cohesion += agente.position;
                alineacion += agente.forward;
                if (distancia < radioSeparacion)
                {
                    Vector3 direccion = transform.position - agente.position;
                    separacion += direccion.normalized / distancia;
                }
                count++;
            }
        }

        if (count > 0)
        {
            cohesion /= count;
            cohesion = cohesion - transform.position;
            cohesion = Vector3.ClampMagnitude(cohesion, velocidadMaxima);

            alineacion /= count;
            alineacion = Vector3.ClampMagnitude(alineacion, velocidadMaxima);

            separacion /= count;
            separacion = Vector3.ClampMagnitude(separacion, velocidadMaxima);
        }

        aceleracion = cohesion + alineacion + separacion;
    }

    void Mover()
    {
        if (hayComidaCerca)
        {
            Vector3 direccion = objetivoComida.position - transform.position;
            if (direccion.magnitude > distanciaArrive)
            {
                velocidad = Vector3.ClampMagnitude(velocidad + aceleracion * Time.deltaTime, velocidadMaxima);
                transform.position += velocidad * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(velocidad.normalized, Vector3.up);
            }
            else
            {
                Destroy(objetivoComida.gameObject);
                hayComidaCerca = false;
            }
        }
        else if (hayCazadorCerca)
        {
            /*aceleracion += -1f * (transform.position - cazadorCercano.position).normalized;*/
            velocidad = Vector3.ClampMagnitude(velocidad + aceleracion * Time.deltaTime, velocidadMaxima);
            transform.position += velocidad * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(velocidad.normalized, Vector3.up);
        }
        else
        {
            aceleracion = Vector3.ClampMagnitude(aceleracion, velocidadMaxima);
            velocidad = Vector3.ClampMagnitude(velocidad + aceleracion * Time.deltaTime, velocidadMaxima);
            transform.position += velocidad * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(velocidad.normalized, Vector3.up);
        }


    }
}