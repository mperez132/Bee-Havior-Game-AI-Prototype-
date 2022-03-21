using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctionScript : MonoBehaviour
{
    public GameObject hive;
    public HiveBehavior hiveScript;
    public GameObject slider;
    public GameObject pButton;
    // Start is called before the first frame update
    void Start()
    {
        hive = GameObject.Find ("PolyHive (1)");
        hiveScript = hive.GetComponent <HiveBehavior>();
        slider = GameObject.Find ("UISlider");
        pButton = GameObject.Find ("ProduceButton");
    }

    void Update(){
        
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void TogglePause() {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
    }

    public void deployButton() {
        float bees = hiveScript.storedBees;
        float percent = slider.GetComponent<Slider>().value;
        int totalValue = (int)(bees*percent);
        hiveScript.deployNBees(totalValue);
        Debug.Log("deployed");
    }

    public void createButton() {
        hiveScript.produceBee();
        Debug.Log("produce Bee");
    }

    public void produceButton() {
        hiveScript.toggleConversion();
        //Debug.Log("we good");
        if (pButton.GetComponentInChildren<Text>().text == "Make Honey") {
            pButton.GetComponentInChildren<Text>().text = "Stop Honey";
        }else{
            pButton.GetComponentInChildren<Text>().text = "Make Honey";
        }
    }

    public void giveButton() {
        hiveScript.Nectar += 20;
    }
}
