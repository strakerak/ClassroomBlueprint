using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


using Debug = UnityEngine.Debug;

[RequireComponent(typeof(CharacterController))]
public class FPCameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;
    public bool first = false;

    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    //public float jumpPower = 7f;
    public float gravity = 10f;


    public float lookSpeed = 2f;
    public float lookXLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    CharacterController characterController;

    public bool canMove = false;
    // Update is called once per frame


    void Start()
    {
        canMove = false;
        characterController = GetComponent<CharacterController>();

    }
    void Update()
    {

        if(gameObject.transform.position.y>0.05f)
        {
            gameObject.transform.position = new Vector3(transform.position.x, 0.03f, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.F) && GameObject.Find("FPVParent(Clone)") != null)
        {
            /*Debug.Log("Camera work");
            GameObject go = GameObject.Find("FPVParent(Clone)");
            GameObject mc = GameObject.Find("GeneralObjects");
            if (first == false)
            {
                Debug.Log("Cam is off");
                go.transform.GetChild(0).gameObject.SetActive(true);
                mc.transform.GetChild(5).gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                first = true;
                canMove = true;
            }
            else
            {
                Debug.Log("Cam on");
                go.transform.GetChild(0).gameObject.SetActive(false);
                mc.transform.GetChild(5).gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                first = false;
                canMove = false;
            }*/
            Debug.Log("Nuh uh uh!");
        }

        if (first == true)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);


            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }



        }
    }
}
