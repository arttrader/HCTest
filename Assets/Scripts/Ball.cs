using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	public float ballInitialVelocity = 400f;

	private Rigidbody rb;
	private bool ballInPlay;
	
	public void StartBall()
	{
		transform.parent = null;
		ballInPlay = true;
		rb.isKinematic = false;
		rb.AddForce(new Vector3(ballInitialVelocity, ballInitialVelocity, 0));
	}

	// Use this for initialization
	void Awake ()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

}
