using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{

    const int clampUpwardAngleY = -85;
    const int clampDownwardAngleY = 85;
    private float movementSpeed = 5f;
    private bool menu = false;
    Vector3 currentEulerAngles;
    public Transform camera;
    // Update is called once per frame
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = transform.GetChild(0);
    }
    void Update()
    {
        //get the Input from X axis
        float horizontalInput = Input.GetAxis("Horizontal");
        //get the Input from Z axis
        float verticalInput = Input.GetAxis("Vertical");
        //get the Input from Y axis
        float acendInput = Input.GetAxis("Jump");

        float mouseVert = Input.GetAxis("Mouse Y");
        float mouseHoriz= Input.GetAxis("Mouse X");

        if (Input.GetKeyDown(KeyCode.Escape)){
            menu = !menu;
        }
        if(menu==true){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(menu==false){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        //Camera Movement
        currentEulerAngles += new Vector3(-mouseVert, 0, 0);
        if(!(currentEulerAngles.x<=-85) && !(currentEulerAngles.x >= 85)){
            camera.localEulerAngles = currentEulerAngles;
        }else{
            if(currentEulerAngles.x<0)
                currentEulerAngles.x = -85;
            else
                currentEulerAngles.x = 85;
        }
        
        //Debug.Log(currentEulerAngles);
        transform.RotateAround(transform.position, Vector3.up, mouseHoriz);



        //update the position
        if(horizontalInput!=0){
            if(horizontalInput>0)
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
            else
                transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
            
        }
        if(verticalInput!=0){
            if(verticalInput>0)
                transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            else
                transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
        }
        if(acendInput!=0){
            if(acendInput>0)
                transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
            else
                transform.Translate(Vector3.down * Time.deltaTime * movementSpeed);
        }
    }
}