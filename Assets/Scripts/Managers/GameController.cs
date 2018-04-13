using System.Collections;
using LevelManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[Range(0.02f, 1f)]
	public float keyRepeatRateLeftRight = 0.25f;
	[Range(0.01f, 0.5f)]
	public float keyRepeatRateDown = 0.01f;
	public float dropInterval = 0.1f;

	public bool isPaused = false;
	public GameObject pausePanel;
	public ParticlePlayer gameOverFx;
	public GameObject gameOverPanel;
	public IconToggle rotIconToggle;


	Board _gameBoard;
	Spawner _spawner;
	SoundManager _soundManager;
	ScoreManager _scoreManager;
	Shape _activeShape;
	Ghost _ghost;
	Holder _holder;


	float _dropIntervalModded; // drop interval modified by level	
	float _timeToDrop; // the next time that the active shape can drop	
	float _timeToNextKeyLeftRight;
	float _timeToNextKeyDown;
	bool _gameOver = false;
	bool _clockwise = true;


	public void ToggleRotDirection()
	{
		_clockwise = !_clockwise;
		if (rotIconToggle)
		{
			rotIconToggle.ToggleIcon(_clockwise);
		}
	}

	public void TogglePause()
	{
		isPaused = !isPaused;

		if (isPaused)
		{
			PauseMenu.Open();
		}
		else
		{
			GameMenu.Open();
		}

		if (_soundManager)
		{
			_soundManager.musicSource.volume = (isPaused) ? _soundManager.musicVolume * 0.25f : _soundManager.musicVolume;
		}
	}

	public void Hold()
	{
		// if the holder is empty...
		if (!_holder.heldShape)
		{
			// catch the current active Shape
			_holder.Catch(_activeShape);

			// spawn a new Shape
			_activeShape = _spawner.SpawnShape();

			// play a sound
			PlaySound(_soundManager.holdSound);

		}
		// if the holder is not empty and can release…
		else if (_holder.canRelease)
		{
			// set our active Shape to a temporary Shape
			Shape shape = _activeShape;

			// release the currently heldShape 
			_activeShape = _holder.Release();

			// move the released Shape back to the spawner position
			_activeShape.transform.position = _spawner.transform.position;

			// catch the temporary Shape
			_holder.Catch(shape);

			// play a sound 
			PlaySound(_soundManager.holdSound);

		}
		// the holder is not empty but cannot release yet
		else
		{
			//Debug.LogWarning("HOLDER WARNING:  Wait for cool down");

			// play an error sound
			PlaySound(_soundManager.errorSound);

		}

		// reset the Ghost every time we tap the Hold button
		if (_ghost)
		{
			_ghost.Reset();
		}

	}

	public void Restart()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Game");
	}


	void Awake()
	{
		GameMenu.Open();
	}

	void Start()
	{
		_gameBoard = GameObject.FindObjectOfType<Board>();
		_spawner = GameObject.FindObjectOfType<Spawner>();
		_soundManager = GameObject.FindObjectOfType<SoundManager>();
		_scoreManager = GameObject.FindObjectOfType<ScoreManager>();
		_ghost = GameObject.FindObjectOfType<Ghost>();
		_holder = GameObject.FindObjectOfType<Holder>();


		_timeToNextKeyDown = Time.time + keyRepeatRateDown;
		_timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

		if (!_gameBoard)
		{
			Debug.LogWarning("WARNING!  There is no game board defined!");
		}

		if (!_soundManager)
		{
			Debug.LogWarning("WARNING!  There is no sound manager defined!");
		}

		if (!_scoreManager)
		{
			Debug.LogWarning("WARNING!  There is no score manager defined!");
		}

		if (!_spawner)
		{
			Debug.LogWarning("WARNING!  There is no spawner defined!");
		}
		else
		{
			_spawner.transform.position = Vectorf.Round(_spawner.transform.position);

			if (!_activeShape)
			{
				_activeShape = _spawner.SpawnShape();
			}
		}

		if (gameOverPanel)
		{
			gameOverPanel.SetActive(false);
		}

		if (pausePanel)
		{
			pausePanel.SetActive(false);
		}

		_dropIntervalModded = Mathf.Clamp(dropInterval - ((float)_scoreManager.level * 0.1f), 0.05f, 1f);
	}

	void Update()
	{
		if (!_spawner || !_gameBoard || !_activeShape || _gameOver || !_soundManager || !_scoreManager)
		{
			return;
		}

		PlayerInput();
	}

	void LateUpdate()
	{
		if (_ghost)
		{
			_ghost.DrawGhost(_activeShape, _gameBoard);
		}
	}


	void PlayerInput()
	{
		if ((Input.GetButton("MoveRight") && (Time.time > _timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveRight"))
		{
			_activeShape.MoveRight();
			_timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

			if (!_gameBoard.IsValidPosition(_activeShape))
			{
				_activeShape.MoveLeft();
				PlaySound(_soundManager.errorSound, 0.5f);
			}
			else
			{
				PlaySound(_soundManager.moveSound, 0.5f);
			}
		}
		else if ((Input.GetButton("MoveLeft") && (Time.time > _timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveLeft"))
		{
			_activeShape.MoveLeft();
			_timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

			if (!_gameBoard.IsValidPosition(_activeShape))
			{
				_activeShape.MoveRight();
				PlaySound(_soundManager.errorSound, 0.5f);
			}
			else
			{
				PlaySound(_soundManager.moveSound, 0.5f);
			}
		}
		else if (Input.GetButtonDown("Rotate"))
		{
			_activeShape.RotateClockwise(_clockwise);

			if (!_gameBoard.IsValidPosition(_activeShape))
			{
				_activeShape.RotateClockwise(!_clockwise);

				PlaySound(_soundManager.errorSound, 0.5f);
			}
			else
			{
				PlaySound(_soundManager.moveSound, 0.5f);
			}
		}
		else if ((Input.GetButton("MoveDown") && (Time.time > _timeToNextKeyDown)) || (Time.time > _timeToDrop))
		{
			_timeToDrop = Time.time + _dropIntervalModded;

			_timeToNextKeyDown = Time.time + keyRepeatRateDown;

			_activeShape.MoveDown();

			if (!_gameBoard.IsValidPosition(_activeShape))
			{
				if (_gameBoard.IsOverLimit(_activeShape))
				{
					GameOver();
				}
				else
				{
					LandShape();
				}
			}

		}
		else if (Input.GetButtonDown("ToggleRot"))
		{
			ToggleRotDirection();
		}
		else if (Input.GetButtonDown("Pause"))
		{
			TogglePause();
		}
		else if (Input.GetButtonDown("Hold"))
		{
			Hold();
		}
	}

	void LandShape()
	{
		if (_activeShape)
		{
			_scoreManager.gameData.AddShape(_activeShape);

			// move the shape up, store it in the Board's grid array
			_activeShape.MoveUp();
			_gameBoard.StoreShapeInGrid(_activeShape);

			_activeShape.LandShapeFX();

			if (_ghost)
			{
				_ghost.Reset();
			}

			if (_holder)
			{
				_holder.canRelease = true;
			}
			// spawn a new shape
			_activeShape = _spawner.SpawnShape();

			// set all of the timeToNextKey variables to current time, so no input delay for the next spawned shape
			_timeToNextKeyLeftRight = Time.time;
			_timeToNextKeyDown = Time.time;

			// remove completed rows from the board if we have any 
			_gameBoard.StartCoroutine("ClearAllRows");

			PlaySound(_soundManager.dropSound);

			if (_gameBoard.completedRows > 0)
			{
				_scoreManager.ScoreLines(_gameBoard.completedRows);

				if (_scoreManager.didLevelUp)
				{
					_dropIntervalModded = Mathf.Clamp(dropInterval - ((float)_scoreManager.level * 0.05f), 0.05f, 1f);
					PlaySound(_soundManager.levelUpVocalClip);
				}
				else
				{
					if (_gameBoard.completedRows > 1)
					{
						AudioClip randomVocal = _soundManager.GetRandomClip(_soundManager.vocalClips);
						PlaySound(randomVocal);
					}
				}

				PlaySound(_soundManager.clearRowSound);
			}
		}
	}

	void GameOver()
	{
		// move the shape one row up
		_activeShape.MoveUp();

		StartCoroutine(GameOverRoutine());

		// play the failure sound effect
		PlaySound(_soundManager.gameOverSound, 5f);

		// play "game over" vocal
		PlaySound(_soundManager.gameOverVocalClip, 5f);

		// set the game over condition to true
		_gameOver = true;

		var saveManager = new SaveManager<LeaderboardScore>();
		var leaderboardScore = new LeaderboardScore
		{
			name = "Yurii",
			score = _scoreManager.GetCurrentData()
		};
		saveManager.Save(new[] { leaderboardScore }, "leaderbord");
	}

	IEnumerator GameOverRoutine()
	{
		if (gameOverFx)
		{
			gameOverFx.Play();
		}

		yield return new WaitForSeconds(0.3f);

		// turn on the Game Over Panel
		//if (gameOverPanel)
		//{
		//	gameOverPanel.SetActive(true);
		//}
		GameOverMenu.Open();
	}

	void PlaySound(AudioClip clip, float volMultiplier = 1.0f)
	{
		if (_soundManager.fxEnabled && clip)
		{
			AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(_soundManager.fxVolume * volMultiplier, 0.05f, 1f));
		}
	}
}
