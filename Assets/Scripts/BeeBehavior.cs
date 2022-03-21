using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BeeBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject currentTarget, hive, currentFlower;
    public bool foundFlower, isExploring, goingHome, atTarget, 
                nectarCollectionAvalible, isEffectPlaying;
    public float nectar, rotateChance, rotateAmount, lifeSpan;
    HiveBehavior hiveScript;
    //MAX_NECTAR is 50 because its approx 50mg of nectar
    private const float MAX_NECTAR = 50;
    //life span is == 2 minutes: 50 ticks per sec, 60 sec per min
    //3000 = 1 sec per 3000
    private const float MAX_LIFESPAN = 18000;
    public GameObject pickupEffect;
    public GameObject effect;


    // NOTE: here we potentially include "Boid" behavior
    //       and check other bees in our radius to prevent
    //       collision and simulate a swarm

    // Start is called before the first frame update
    void Start()
    {
        // Bee is at hive
        agent = GetComponent<NavMeshAgent>();
        hive = GameObject.FindGameObjectWithTag("Hive");
        hiveScript = hive.GetComponent<HiveBehavior>();
        agent.speed = 3;
        //Debug.Log(agent.isOnNavMesh);
        foundFlower = false;
        isExploring = true;
        goingHome = false;
        atTarget = false;
        isEffectPlaying = false;

        nectar = 0;
        lifeSpan = 0;
        nectarCollectionAvalible = true;

        currentTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTarget == null){
            // isExploring pathfinding
            if(isExploring){
                rotateChance = Random.Range(0.0f, 10.0f);
                rotateAmount = Random.Range(-20.0f, 20.0f);
                agent.SetDestination(transform.position+transform.forward);
                if(rotateChance < 0.05f){
                    transform.RotateAround(transform.position, Vector3.up, rotateAmount);
                }
            }
        }
        // Go to target
        if (currentTarget != null && !atTarget) {
            //Debug.Log("Has a Target" + currentTarget.transform.position + GetInstanceID());
            agent.SetDestination(currentTarget.transform.position);
        }
        // Slurp nectar from currentFlower (AKA currentTarget)
        if (atTarget && currentTarget.CompareTag("Flower") &&
            nectar < MAX_NECTAR && nectarCollectionAvalible) {
            StartCoroutine(nectarSuckTimer());
            if (currentTarget.GetComponent<FlowerBehavior>().suckNectar()) {
                nectar++;
                //Debug.Log("Nectar is: " + nectar);
            }else{
                Debug.Log("suckNectar() failed");
                isExploring = true;
                foundFlower = false;
                atTarget = false;
                goingHome = false;
                currentTarget = null;
                currentFlower = null;
                // if(isEffectPlaying == true) {
                //     effect.SetActive(false);
                //     isEffectPlaying = false;
                // }
            }
        }
        // Go home
        if (nectar >= MAX_NECTAR) {
            isExploring = false;
            atTarget = false;
            goingHome = true;
            currentTarget = hive;
        }
    }

    void FixedUpdate(){
        //float fixedDistance = 70;
        if(transform.position.x < -68f || transform.position.x > 80f ||
            transform.position.z < -75f || transform.position.z > 35f)
            recieveSignal(1);

        lifeSpan++;
        if(lifeSpan >= MAX_LIFESPAN){
            Debug.Log("Bee died of old age");
            Destroy(this.gameObject);
        }
    }

    // neutralState()
    // Reset all state values to neutral (AKA bee spawns in hive)
    public void neutralState() {
        foundFlower = false;
        isExploring = false;
        goingHome = false;
        atTarget = false;
        isEffectPlaying = false;
    }

    // recieveSignal()
    // Transition event handler
    // Pre:  int - signal code:
    //             0 for "go find flower"
    //             1 for "go home"
    // Post: bool - true if signal was recieved, else false
    public bool recieveSignal(int signal) {
        // 0: Exploring
        if (signal == 0) {
            // Maintain state
            foundFlower = false;
            isExploring = true;
            goingHome = false;
            atTarget = false;
            
            return true;
        }
        // 1: Go home
        else if (signal == 1) {
            // Maintain state
            goingHome = true;
            currentTarget = hive;
            atTarget = false;
            isExploring = false;

            return true;
        }
        return false;
    }

    // dropOffNectar()
    // Called by Hive
    // Pre:  none
    // Post: returns nectar in inventory
    // Hive doesnt need agency, function is depreciated
    public float dropOffNectar() {
        float temp = nectar;
        nectar = 0;
        return temp;
    }

    public void foundFlowerFunc(GameObject flower){
        //Debug.Log("foundFlowerFunc was called");
        if(!foundFlower){
            foundFlower = true;
            goingHome = false;
            atTarget = false;
            isExploring = false;
            currentFlower = flower;
            currentTarget = flower;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(currentTarget == null) return;
        Debug.Log("Bee hit something physically");
        if (other.gameObject.CompareTag("Hive") && currentTarget == hive) {
            Debug.Log("Touched hive");
            //GameObject child = transform.GetChild(0).gameObject;
            //child.SetActive(false);
            //gameObject.SetActive(false);
            hiveScript.storedBees++;
            Destroy(this.gameObject);
            goingHome = false;
            atTarget = true;
            isEffectPlaying = false;
        }
        if(other.gameObject.CompareTag("Flower") && currentTarget.CompareTag("Flower")) {
            isExploring = false;
            foundFlower = false;
            atTarget = true;
            Quaternion rotation = Quaternion.AngleAxis(0f, Vector3.up);
            effect = Instantiate(pickupEffect, transform.position, rotation);
            effect.SetActive(true);
            isEffectPlaying = true;
            //currentTarget = other.gameObject;
            //currentFlower = other.gameObject;
        }
    }
    private void OnCollisionExit(Collision other){
        if(other.gameObject.CompareTag("Flower")){
            atTarget = false;
            foundFlower = false;
            currentFlower = null;
            Destroy(effect);
            isEffectPlaying = false;
        }
    }

    IEnumerator nectarSuckTimer(){
        nectarCollectionAvalible = false;
        yield return new WaitForSeconds(1);
        // isEffectPlaying = true;
        nectarCollectionAvalible = true;
    }
}
