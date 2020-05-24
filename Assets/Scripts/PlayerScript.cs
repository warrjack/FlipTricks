using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

	float RotationSpeed = 600f;
	int animation = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	if (Input.GetKeyDown(KeyCode.RightArrow))
    	{
    		animation = 1;
    	}

    	if(animation == 1)
    	{
    		transform.Rotate(Vector3.right*(RotationSpeed * Time.deltaTime));
    	}
    }
}
