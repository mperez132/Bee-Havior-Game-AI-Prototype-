using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    public int totalFlowers;
    public Queue<GameObject> FlowerQueue;
    private int createdFlowerCounter;
    public GameObject flower;
    Vector3 spawnPoint;
    void Start() {
        FlowerQueue = new Queue<GameObject>();
        for (int i = 0; i < totalFlowers; i++) {
            GameObject seed = createFlower();
            seed.name = "Flower " + createdFlowerCounter.ToString();
            createdFlowerCounter++;
            FlowerQueue.Enqueue(seed);
        }
    }

    GameObject createFlower() {
        //Debug.Log("Flower Spawn called");
        Quaternion rotation = Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.up);
        spawnPoint.x = Random.Range (-68f, 80);
        spawnPoint.y = 0.35f;
        spawnPoint.z = Random.Range(-75f, 35f);
        
        Vector3 newPosition = new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z);
        GameObject clone = Instantiate(flower, newPosition, rotation);
        return clone;
    }
}