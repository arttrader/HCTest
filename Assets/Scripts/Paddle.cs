using System;
using UnityEngine;
using UnityEngine.XR;

public class Paddle : MonoBehaviour
{
	public float initialPosition = -9.5f;
	public float paddleSpeed = 0.0001f;
//	public float deltaX = 0f;
	public InputController input;

	// Use this for initialization
	void Awake () {
		input.paddle = this;
	}

	void Start()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
//		swipe.DetectSwipe();
/*		if (Math.Abs(deltaX) > 0) {
			float x = Mathf.Clamp(transform.position.x + deltaX * paddleSpeed, -10f, 10f);
			Debug.Log("Moving to: " + x);
			transform.position = new Vector3(x, -9.5f, 0f);
		}*/
	}

	public void Move(float deltaX)
	{
		float x = Mathf.Clamp(transform.position.x + deltaX * paddleSpeed, -10f, 10f);
//		Debug.Log("Setting delta x to: " + x);
		transform.position = new Vector3(x, -9.5f, 0f);
	}
}
