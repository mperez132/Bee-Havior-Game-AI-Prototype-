using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSBehavior : MonoBehaviour
{
    //The Speed of the camera
    private float panSpeed = 20f;
    //The amount from the border that the cursor can move the camera
    public float panBorderThickness = 10f;
    public float smoothSpeed = 100f;
    public float scrollSpeed = 500f;
    public float rotationAmount;
    public float movementSpeed;
    public Vector3 newPosition;
    public Quaternion newRotation;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        //Adding the left shift as a multiplier of 2 for the pan speed. This can be adjusted 
        if (Input.GetKey("left shift")) {
            panSpeed = 40f;
        }
        else {
            panSpeed = 20f;
        }
        //If the player moves the cursor on the border or presses a wasd key then the camera will move given the pan speed
        if(Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness) {
            position.z += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness) {
            position.z -= panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness) {
            position.x += panSpeed * Time.deltaTime;
        }
        if(Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness) {
            position.x -= panSpeed * Time.deltaTime;
        }
        //Setting the border that the camera will stop at so it doesn't go too far off the map. 
        //Hard set values since our map is not a perfect square. 
        position.x = Mathf.Clamp(position.x, -80, 88);
        position.z = Mathf.Clamp(position.z, -98, 35);
        transform.position = position;
    }
    void LateUpdate() {
        Vector3 position = transform.position;
        Vector3 tempPosition = transform.position;
        //RTS games have a scroll feature so we're able to zoom in or zoom out on the environment
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        tempPosition.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
        Vector3 desiredPosition = tempPosition;
        Vector3 smoothedPosition = Vector3.Lerp(position, desiredPosition, smoothSpeed);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, 6, 25);
        transform.position = smoothedPosition;
    }
}
