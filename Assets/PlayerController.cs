﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float force;
	public float jumpHeight;
    private float lockPos;
    private float initSpeed;
	public bool finishedJump;
	public bool canMove;
    private float scalD;
    private float currD;
    private bool isAlive;
    private float xSpawn;
    private float ySpawn;

    private float tTime;

	private Rigidbody2D rb;

	void Start ()
	{
        isAlive = true;
		canMove = true;
		finishedJump = true;
		rb = GetComponent<Rigidbody2D>();
        lockPos = 0f;
        initSpeed = 0.6f;
        scalD = -0.012f;
        currD = 0f;
        xSpawn = gameObject.transform.position.x;
        ySpawn = gameObject.transform.position.y;

        tTime = 0;
	}

	void FixedUpdate ()
	{
        if(isAlive) transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockPos, lockPos);
        if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && canMove) {
                if (finishedJump) rb.AddForce(new Vector2(1.8f*force, 0));
                else rb.AddForce(new Vector2(force, 0));
                //This line checks to see if the speed in the x direction is below a certain value. If it is, it sets the velocity.
                //This helps to make movement a bit more responsive but still smooth.
                if (checkV()) rb.velocity = new Vector2(initSpeed, rb.velocity.y);
            }

			if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && canMove) {
                if (finishedJump) rb.AddForce(new Vector2 (1.8f * -force, 0));
                else rb.AddForce(new Vector2(-force, 0));
                if (checkV()) rb.velocity = new Vector2(-initSpeed, rb.velocity.y);
            }

			if (Input.GetKey (KeyCode.Space) && finishedJump) {
				rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
				finishedJump = false;
			}
		}
		else {
			if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && canMove) {
                if (finishedJump) rb.AddForce(new Vector2 (force, 0));
                else rb.AddForce(new Vector2(0.6f * force, 0));
                if (checkV()) rb.velocity = new Vector2(initSpeed, rb.velocity.y);
            }

			if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && canMove) {
                if (finishedJump) rb.AddForce(new Vector2 (-force, 0));
                else rb.AddForce(new Vector2(0.6f * -force, 0));
                if (checkV()) rb.velocity = new Vector2(-initSpeed, rb.velocity.y);
            }

			if (Input.GetKey (KeyCode.Space) && finishedJump) {
				rb.velocity = new Vector2 (rb.velocity.x, jumpHeight);
				finishedJump = false;
			}
		}

        if (!finishedJump)
        {
            currD += scalD;
            rb.velocity += new Vector2(0, currD);
        }

        if (!isAlive)
            tdTimer();
	}

        void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.CompareTag("Floor") && finishedJump == false && isAlive)
		{
			finishedJump = true;
			canMove = true;
            currD = 0;
		}
		if(coll.gameObject.CompareTag("Wall") && isAlive)
		{
			if (!finishedJump) {
				canMove = false;
			} 
//			else {
//				;
//			}
		}
        if (coll.gameObject.CompareTag("Hazard")) die();
	}
    
    private bool checkV()
    {
        if (System.Math.Abs(rb.velocity.x) < 0.4f) return true;
        else return false;
    }
    private void die()
    {
        isAlive = false;
        canMove = false;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    private void respawn()
    {
        gameObject.transform.position = new Vector2(xSpawn,ySpawn);
        isAlive = true;
        canMove = true;
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    private void tdTimer()
    {
        if (tTime < 3)
            tTime += Time.fixedDeltaTime;
        else
        {
            tTime = 0;
            respawn();
        }
    }

    public void updateCheck(Vector3 cp)
    {
        xSpawn = cp.x;
        ySpawn = cp.y;
    }
}
