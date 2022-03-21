// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class UIValueScript : MonoBehaviour
// {
//     public int totalValue = 0;
//     private GameObject slider;
//     private GameObject hive;
//     private HiveBehavior hiveScript;
//     private float sliderValue;
//     private float queueLength;
//     // Start is called before the first frame update
//     void Start(){
//         slider = GameObject.Find ("UISlider");
//         hive = GameObject.Find ("Hive");
//         hiveScript = hive.GetComponent <HiveBehavior>();
//     }

//     // Update is called once per frame
//     void Update(){
//         sliderValue = slider.GetComponent <Slider>().value;
//         queueLength = hiveScript.BeeQueue.Count;
//         totalValue = (int)(sliderValue * queueLength);
//     }
// }
