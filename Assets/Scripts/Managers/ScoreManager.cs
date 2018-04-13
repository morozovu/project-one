using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public int level = 1;
	public int linesPerLevel = 5;
	public Text linesText;
	public Text levelText;
	public Text scoreText;
	public bool didLevelUp = false;
	public ParticlePlayer levelUpFx;
	public GameData gameData;

	const int minLines = 1;
	const int maxLines = 4;

	int _score = 0;
	int _linesToNextLevel;
	int _totalLines = 0;


	public void ScoreLines(int n)
	{
		gameData.AddLines(n);

		// flag to GameController that we leveled up
		didLevelUp = false;

		// clamp this between 1 and 4 lines
		n = Mathf.Clamp(n, minLines, maxLines);

		// adds to our score depending on how many lines we clear
		switch (n)
		{
			case 1:
				_score += 40 * level;
				break;
			case 2:
				_score += 100 * level;
				break;
			case 3:
				_score += 300 * level;
				break;
			case 4:
				_score += 1200 * level;
				break;
		}

		// reduce our current number of lines needed for the next level
		_linesToNextLevel -= n;

		_totalLines += n;

		// if we finished our lines, then level up
		if (_linesToNextLevel <= 0)
		{
			LevelUp();
		}

		// update the user interface
		UpdateUIText();
	}

	public void Reset()
	{
		gameData = new GameData();
		level = 1;
		_linesToNextLevel = linesPerLevel * level;

		UpdateUIText();
	}

	public void LevelUp()
	{
		level++;
		_linesToNextLevel = linesPerLevel * level;
		didLevelUp = true;

		if (levelUpFx)
		{
			levelUpFx.Play();
		}
	}

	public GameData GetCurrentData()
	{
		gameData.score = _score;
		gameData.level = level;

		return gameData;
	}


	void Start()
	{
		Reset();
	}

	string PadZero(int n, int padDigits)
	{
		string nStr = n.ToString();

		while (nStr.Length < padDigits)
		{
			nStr = "0" + nStr;
		}
		return nStr;
	}

	void UpdateUIText()
	{
		if (levelText)
		{
			levelText.text = level.ToString();
		}

		if (scoreText)
		{
			scoreText.text = PadZero(_score, 5);
		}

		if (linesText)
		{
			linesText.text = PadZero(_totalLines, 3);
		}
	}
}
