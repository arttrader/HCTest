using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
	public int lives = 3;
	public int bricks = 24;
	public float resetDelay = 1f;
	public Text livesText;
	public GameObject gameOver;
	public GameObject youWon;
	public GameObject bricksPrefab;
	public GameObject paddle;
	public GameObject ball;
	public GameObject deathParticles;
	public static GM instance = null;
	public Vector3 defaultPaddlePosition, defaultBallPosition;

	private Ball ballScript;
	private bool gameStarted;

	public void StartGame()
	{
		if (!gameStarted)
		{
			gameStarted = true;
			ballScript.StartBall();
		}
	}

	// Use this for initialization
	void Start()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		ballScript = ball.GetComponent<Ball>();
		defaultPaddlePosition = paddle.transform.position;
		defaultBallPosition = ball.transform.position;
		Setup();
	}

	public void Setup()
	{
		Instantiate(bricksPrefab, transform.position, Quaternion.identity);
		SetupPaddle();
	}

	void CheckGameOver()
	{
		if (bricks < 1)
		{
			youWon.SetActive(true);
			Time.timeScale = .25f;
			Invoke("Reset", resetDelay);
		}

		if (lives < 1)
		{
			gameOver.SetActive(true);
			Time.timeScale = .25f;
			Invoke("Reset", resetDelay);
		}
	}

	void Reset()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("TestScene");
	}

	public void LoseLife()
	{
		paddle.SetActive(false);
		lives--;
		livesText.text = "Lives: " + lives;
		Instantiate(deathParticles, paddle.transform.position, Quaternion.identity);
		Invoke("SetupPaddle", resetDelay);
		CheckGameOver();
	}

	void SetupPaddle()
	{
		paddle.transform.position = defaultPaddlePosition;
		ball.transform.position = defaultBallPosition;
		paddle.SetActive(true);
	}

	public void DestroyBrick()
	{
		bricks--;
		CheckGameOver();
	}
}
