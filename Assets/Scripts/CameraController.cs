using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private const float Y_MIN = 0.0f;
	private const float Y_MAX = 45.0f;

	public Transform lookAtObject;
	public Transform cameraTransform;
    public Vector3 cameraDistance;

	private Camera camera;
    private bool inCameraShiftZone = true;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform;
        camera = Camera.main;
        cameraDistance = new Vector3(transform.position.x - lookAtObject.transform.position.x, transform.position.y - lookAtObject.transform.position.y, transform.position.z);
        //Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if(inCameraShiftZone)
        {
            transform.position = new Vector3(lookAtObject.transform.position.x + cameraDistance.x, lookAtObject.transform.position.y + cameraDistance.y, transform.position.z);

        }else{
            transform.position = new Vector3(lookAtObject.transform.position.x + cameraDistance.x, transform.position.y, transform.position.z);
        }

        if(lookAtObject.GetComponent<PlayerController>().didGrind)
        {
            inCameraShiftZone = false;
        }else{
            inCameraShiftZone = true;
        }
    }
}
