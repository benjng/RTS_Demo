using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseZoomDuration = 0.2f;
    // private float leftEdge = 0f;
    // private float bottomEdge = 10f;
    private float rightEdge = Screen.width - 10;
    private float TopEdge = Screen.height - 10;


    void Start()
    {
        // Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(CameraLerp());
    }

    void Update()
    {
        float horizontalInput, verticalInput;

        // Check if the mouse position is on the left edge of the screen
        if (/*Input.mousePosition.x <= leftEdge || */Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        } else if (/*Input.mousePosition.x >= rightEdge || */Input.GetKey(KeyCode.D)) {
            horizontalInput = 1f;
        } else {
            horizontalInput = 0;
        }

        if (/*Input.mousePosition.y <= bottomEdge || */Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        } else if (/*Input.mousePosition.y >= TopEdge || */Input.GetKey(KeyCode.W)) {
            verticalInput = 1f;
        } else {
            verticalInput = 0;
        }

        // Calculate the movement direction based on the camera's orientation
        Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);      
        Vector3 cameraRight = cameraTransform.right;

        // The direction in which the character should move based on the user's input
        Vector3 movementDir = verticalInput * cameraForward + horizontalInput * cameraRight;
        movementDir.Normalize();

        transform.position += movementDir * Time.deltaTime * moveSpeed;
    }

    IEnumerator CameraLerp(){
        while (true){
            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheelInput == 0) yield return null;

            float currentFOV = cameraTransform.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
            float targetFOV = currentFOV;
            if (scrollWheelInput > 0f){ // Scroll wheel is scrolling up
                targetFOV -= 5;
            } else if (scrollWheelInput < 0f){ // Scroll wheel is scrolling down
                targetFOV += 5;
            }


            if (targetFOV != currentFOV) {
                float t = 0;
                float _smoothness = 10f;
                float timeStarted = Time.time;
                for (int i=0; i<_smoothness; i++){
                    cameraTransform.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = Mathf.Lerp(currentFOV, targetFOV, t);
                    t += 1f/_smoothness;
                    yield return new WaitForSeconds(mouseZoomDuration/_smoothness);
                }
                Debug.Log(Time.time - timeStarted);
            }
            yield return null;
        }
    }

    // void OnDrawGizmos() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(transform.position, 2.0f);
    // }
}
