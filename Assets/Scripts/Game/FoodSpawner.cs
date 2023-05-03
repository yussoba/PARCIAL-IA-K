using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform spawnArea;
    public float spawnTime = 3f; // tiempo de espera para spawnear otra comida

    private void Start()
    {
        InvokeRepeating("SpawnFood", 0f, spawnTime);
    }

    private void SpawnFood()
    {
        // Calcular posición aleatoria dentro del spawnArea
        float x = Random.Range(-spawnArea.localScale.x / 2f, spawnArea.localScale.x / 2f);
        float z = Random.Range(-spawnArea.localScale.z / 2f, spawnArea.localScale.z / 2f);
        Vector3 spawnPosition = spawnArea.position + new Vector3(x, 0f, z);

        // Crear instancia de la comida en la posición aleatoria
        Instantiate(foodPrefab, new Vector3(x, 0f, z), Quaternion.identity);
    }

}

