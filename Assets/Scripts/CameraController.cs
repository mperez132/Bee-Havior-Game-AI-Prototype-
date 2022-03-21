using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotationAmount;
    public float movementSpeed;
    public float movementTime;
    public Vector3 newPosition;
    public Quaternion newRotation;
    public float normalSpeed;
    public float fastSpeed;
    public float smoothSpeed = 100f;
    public float panBorderThickness = 10f;
    public Vector3 zoomAmount;
    public Vector3 newZoom;
    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public float maxZoom, minZoom;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;
    // Start is called before the first frame update
    void Start()
    {
        //Set the current position, rotation and zoom
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false){
            //Adding the a cleaner zomo feature for the mouse scroll wheel. 
            //It has two locked heights for it can't go too high or below the ground. 
            if(Input.mouseScrollDelta.y != 0) {
                if(Input.mouseScrollDelta.y > 0 && newZoom.z < maxZoom) {
                    newZoom += Input.mouseScrollDelta.y * zoomAmount;
                }
                if(Input.mouseScrollDelta.y < 0 && newZoom.z > minZoom) {
                    newZoom += Input.mouseScrollDelta.y * zoomAmount;
                }
            }
            //Adding mouse input for the middle mouse or left click to drag and move the camera
            if(Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(0)) {
                Plane tempPlane = new Plane(Vector3.up, Vector3.zero);
                Ray tempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entryPoint;
                if(tempPlane.Raycast(tempRay, out entryPoint)) {
                    dragStartPosition = tempRay.GetPoint(entryPoint);
                }
            }
            if(Input.GetMouseButton(2)|| Input.GetMouseButton(0)) {
                Plane tempPlane = new Plane(Vector3.up, Vector3.zero);
                Ray tempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entryPoint;
                if(tempPlane.Raycast(tempRay, out entryPoint)) {
                    dragCurrentPosition = tempRay.GetPoint(entryPoint);
                    newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                }
            }
            //Adding mouse input for the right click to drag and rotate the camera. 
            if(Input.GetMouseButtonDown(1)) {
                rotateStartPosition = Input.mousePosition;
            }
            if(Input.GetMouseButton(1)) {
                rotateCurrentPosition = Input.mousePosition;
                Vector3 tempDiff = rotateStartPosition - rotateCurrentPosition;
                rotateStartPosition = rotateCurrentPosition;
                newRotation *= Quaternion.Euler(Vector3.up * (-tempDiff.x/5f));
            }
            //Adding the left shift as a multiplier of 2 for the pan speed. This can be adjusted 
            if (Input.GetKey("left shift")) {
                movementSpeed = fastSpeed;
            }
            else {
                movementSpeed = normalSpeed;
            }
        }
        //If the player moves the cursor on the border or presses a wasd key then the camera will move given the pan speed
        //removed from if statement: || Input.mousePosition.y >= Screen.height - panBorderThickness
        if(Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) {
            newPosition += (transform.forward * movementSpeed);
        }
        //removed from if statement: || Input.mousePosition.y <= panBorderThickness
        if(Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) {
            newPosition += (transform.forward * -movementSpeed);
        }
        //removed from if statement: || Input.mousePosition.x >= Screen.width - panBorderThickness
        if(Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) {
            newPosition += (transform.right * movementSpeed);
        }
        //removed from if statement: || Input.mousePosition.x <= panBorderThickness
        if(Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) {
            newPosition += (transform.right * -movementSpeed);
        }
        if(Input.GetKey("q")) {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if(Input.GetKey("e")) {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        newPosition.x = Mathf.Clamp(newPosition.x, -60, 90);
        newPosition.z = Mathf.Clamp(newPosition.z, -80, 40);
        //Apply the changes to the position, rotation, and the to the cameras position. 
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, smoothSpeed);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
    
}
