using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject[] flowers;
    private float aggroRadius;
    private Vector3 startPos;
    private int flowerToPick;

    private bool foundFlower = false;
    private Vector3 flowerPos;
    private FlowerBehavior flower;
    private float Nectar;


    //-------------------------

    //-------------------------
    // Start is called before the first frame update
    void Start()
    {
        Nectar = 0;
        agent = GetComponent<NavMeshAgent>();
        //flower = GameObject.FindGameObjectsWithTag("Flower");
        startPos = transform.position;
        flowerToPick = Random.Range(0,2);
    }

    // Update is called once per frame
    void Update()
    {
        if(!foundFlower){
            Debug.Log("Flower not found");
        //explore code
            agent.SetDestination(transform.position+transform.forward);
            float rotateChance = Random.Range(0.0f, 10.0f);
            float rotateAmount = Random.Range(-5.0f, 20.0f);
            if(rotateChance < 0.1f){
                Debug.Log("Rotated");
                transform.RotateAround(transform.position, Vector3.up, rotateAmount);
            }
        }else{
            Debug.Log("Flower Is found");
            Vector3  temp = new Vector3(flowerPos.x, 1.69f, flowerPos.z);
            Debug.Log("Set Destination: " + agent.SetDestination(flowerPos));
            Debug.Log(agent.transform.position.x + " | " + agent.destination.x);
            Debug.Log(agent.transform.position.z + " | " + agent.destination.z);
            if(agent.transform.position.x == agent.destination.x &&
               agent.transform.position.z == agent.destination.z){
                   Debug.Log("Should Suck Nectar");
                //Pick up testing of suckNectar here
                //Need to get script of flower that was found
                if(flower.suckNectar()){ //(!flower.suckNectar() || Nectar==5)
                    //if full on nectar should go home
                    //if no full keep exploring
                    //for now just explore after collecting
                    Debug.Log("Nectar level is: " + Nectar);
                }
            }else{
                Debug.Log("Failed");
            }
        }


        //Test to see if navMesh is working
        /*
        Vector3 target = flower[flowerToPick].transform.position - transform.position;
        float dist = target.magnitude;
        if(dist <= aggroRadius){
            agent.SetDestination(flower[flowerToPick].transform.position);
        }else{
            agent.SetDestination(startPos);
        }
        */
    }

    public void foundFlowerFunc(Vector3 pos){
        Debug.Log("foundFlowerFunc was called");
        foundFlower = true;
        flowerPos = pos;
    }

    void OnCollisionEnter(Collision collision){
        Debug.Log("Bee hit something: " + collision);
        if(collision.gameObject.CompareTag("Flower")){
            Debug.Log("Object is a flower");
            flower = collision.gameObject.GetComponent<FlowerBehavior>();
            foundFlower = true;
            flowerPos = collision.transform.position;
            Debug.Log(flowerPos);
        }
    }
}
