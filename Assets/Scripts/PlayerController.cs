using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float jumpHeight;
    //public float jumpSpeed;

    private float ROTATIONANGLE = 90f;
    //private float currentHeight;
    //private bool jumping, falling;

	// Use this for initialization
	void Start () {
        //jumping = false;
        //falling = false;
        //currentHeight = 0;
	}
	
	// Update is called once per frame
	void Update () {

        //if(Input.GetButton("Jump") && !jumping && !falling){
        //    jumping = true;
        //}

        //if(currentHeight > jumpHeight) {
        //    jumping = false;
        //    falling = true;
        //}

        //if(jumping) {
        //    currentHeight += jumpSpeed * Time.deltaTime;
        //}

        //if(falling) {
        //    currentHeight -= jumpSpeed * Time.deltaTime;
        //}

        //if (currentHeight<0) {
        //    falling = false;
        //}

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Quaternion newRotation = Quaternion.Euler(
            new Vector3(
                verticalInput * ROTATIONANGLE,
                0, 
                horizontalInput * -ROTATIONANGLE)
        );
        Vector3 newPosition = new Vector3(
            horizontalInput,
            1, //1 + currentHeight,
            verticalInput
        );

        GetComponent<Rigidbody>().MoveRotation(newRotation);
        GetComponent<Rigidbody>().MovePosition(newPosition);

	}

}
