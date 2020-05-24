using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Character Components
	CharacterController characterController;
	Rigidbody rb;
	Animator animator;
	public Text trickText;

	//Character Variables
	private float jumpForceStart = 1.8f;
	private float gravityForceStart = 1.0f;
	private float pushForceStart = 0.7f;
	private float maxSpeedStart = 25.0f;

	//Physics Variables
	private float pushForce = 3.0f;
	private float brakeForce = 0.0f;
	private float jumpForce = 0.5f;
	private float gravityForce = 0.8f;
	private float maxSpeed = 20.0f;
	private bool isGrounded = true;

	//Grind Variables
	private bool canGrind = false;
	public bool didGrind = false;
	private float grindableLength = 0.0f;
	private float grindableX = 0.0f;
	private float grindableY = 0.0f;
	private float grindableZ = 0.0f;

	public bool inCameraShiftZone = false;

	//Spawns
	private Vector3 origin;
	private Vector3 spawn;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.AddForce(0.1f, 0, 0);
        trickText.text = "";

        brakeForce = pushForce/3;
        origin = transform.position;
        spawn = transform.position;

        weirdStart();
    }

    // Update is called once per frame
    void Update()
    {
    	//Push
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
        	rb.AddForce(pushForce, 0, 0, ForceMode.Impulse);
        }

        //Brake/Reverse
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
        	rb.AddForce(-brakeForce, 0, 0, ForceMode.Impulse);
        }

        //Jump
        if (isGrounded)
        {

        	//Kickflip
        	if (Input.GetKeyDown(KeyCode.RightArrow))
	        {
	        	rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
	        	animator.SetTrigger("Kickflip");
	        	trickText.text = "Kickflip";
	        }
	        //Pop Shove It
	        if (Input.GetKeyDown(KeyCode.DownArrow))
	        {
	        	rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
	        	animator.SetTrigger("Pop Shove It");
	        	trickText.text = "Pop Shove It";
	        }
	        //Ollie
	        if (Input.GetKeyDown(KeyCode.S))
	        {
	        	rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
	        	animator.SetTrigger("Ollie");
	        	trickText.text = "Ollie";
	        }
	        //Impossible
	        if(Input.GetKeyDown(KeyCode.W))
	        {
	        	rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
	        	animator.SetTrigger("Impossible");
	        	trickText.text = "Impossible";
	        }

	        //Treflip
	        if(Input.GetKeyDown(KeyCode.UpArrow))
	        {
	        	rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
	        	animator.SetTrigger("Treflip");
	        	trickText.text = "Treflip";
	        }
        }

        //Grind
        if(Input.GetKeyDown(KeyCode.Space) && canGrind)
        {
            if (!didGrind)
            {
                didGrind = true;
                characterController.Move(new Vector3(transform.position.x - spawn.x, grindableY, grindableZ));
            }
        }
        
        //Manual
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("Manual", true);
            trickText.text = "Manual";
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool("Manual", false);
        }
        


        //Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
        	transform.position = spawn;
        	transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //Speed Cap
        if(rb.velocity.magnitude > maxSpeed)
         {
            rb.velocity = rb.velocity.normalized * maxSpeed;
         }

        //Gravity & Rotation Fix
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(0, -gravityForce, 0);
    }

    void OnTriggerEnter(Collider collider)
    {
    	if (collider.gameObject.layer == 8)
    	{
    		Debug.Log("Grounded");
    		isGrounded = true;
    	}
    	//In Grindable Area
    	else if (collider.gameObject.layer == 9)
    	{
    		origin = transform.position;
    		//parent position
    		grindableX = collider.transform.position.x + collider.transform.GetChild(0).gameObject.transform.position.x;
    		grindableY = collider.transform.position.y + collider.transform.GetChild(0).gameObject.transform.position.y;
    		grindableZ = collider.transform.position.z + collider.transform.GetChild(0).gameObject.transform.position.z;
    		grindableY = 0.843f;
    		grindableZ = 4.871f;
    		grindableLength = collider.bounds.size.x;
    		Debug.Log("Current X: " + transform.position.x);
    		//transform.position = new Vector3(-27.0f, grindableY, grindableZ);


    		canGrind = true;
    	//In CameraShift Area
    	} else if(collider.gameObject.layer == 10)
    	{
    		inCameraShiftZone = true;
    	}
    }
    void OnTriggerExit(Collider collider)
    {
    	Debug.Log("UnGrounded");
    	if (collider.gameObject.layer == 8)
    	{
    		isGrounded = false;
    	}
    	//In Grindable Area
    	else if (collider.gameObject.layer == 9)
    	{
    		canGrind = false;
    		if (didGrind)
    		{
    			Debug.Log("grindLength: " + grindableLength);
    			characterController.Move(new Vector3(transform.position.x + grindableLength + (grindableX - spawn.x), transform.position.y - spawn.y, spawn.z));
    			didGrind = false;
    		}
    	}
    }

    void weirdStart()
    {
    	jumpForce = jumpForceStart;
        gravityForce = gravityForceStart;
        pushForce = pushForceStart;
        maxSpeed = maxSpeedStart;
    }
}
