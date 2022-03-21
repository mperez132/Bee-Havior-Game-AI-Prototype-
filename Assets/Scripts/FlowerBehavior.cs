using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBehavior : MonoBehaviour
{
    // NOTES: 11/23/2021
    // bool to maintain if flower is depleted
    // change the state of collider based on depleted or not
    // timer for replenish Nectar
    public int Nectar;
    private bool depleted, hasBee;
    SphereCollider objCollider;
    int replenishTime;
    float timer = 0;
    BeeBehavior beeScript;
    TestNavMesh beeTest;
    Renderer changeColor;
    public Material witheredColor;
    public Material ripeColor;

    // Start is called before the first frame update
    void Start()
    {
        // consider frame rate and how fast bee consumes 1 nectar
        //Nectar = Random.Range(1, 5);//amount of nectar
        Nectar = Random.Range(25, 50);
        replenishTime = Random.Range(1, 3);//num of minutes till replenish
        objCollider = GetComponent<SphereCollider>();
        changeColor = GetComponent<Renderer>();
        timer = 0;
        hasBee = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Flower has no nectar left
        //disable its collider because its inactive
        //start incrementing timer
        if(Nectar<=0){
            objCollider.enabled = false;
            timer += Time.deltaTime;
        }
        //if  timer has reached the replenishTime minutes
        //enable the collider and reset variables such
        //as nectar, timer, depleted
        if(timer >= replenishTime*60){
            changeColor.material = ripeColor;
            objCollider.enabled = true;
            hasBee = false;
            Nectar = Random.Range(25, 50);
            timer = 0;
        }
    }
    //in suckNectar set hasBee to false if a bee is
    //allowed to leave a flower before flower is empty
    public bool suckNectar() {
        if (Nectar <= 0) {
            changeColor.material = witheredColor;
            return false;
        } else {
            Nectar--;
            return true;
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Bee") && !hasBee){
            Debug.Log("Trigger went off");
            hasBee = true;
            //send signal to bee
             beeScript = other.gameObject.GetComponent<BeeBehavior>();
             beeScript.foundFlowerFunc(gameObject);
        }
    }
    private void OnCollisionExit(Collision other){
        hasBee = false;
    }
}