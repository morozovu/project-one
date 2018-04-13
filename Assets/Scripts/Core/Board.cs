using System.Collections;
using UnityEngine;


public class Board : MonoBehaviour
{
	public Transform emptySprite;
	public int height = 30;
	public int width = 10;
	public int header = 8;
	public int completedRows = 0;
	public ParticlePlayer[] m_rowGlowFx = new ParticlePlayer[4];

	Transform[,] _grid;


	public void StoreShapeInGrid(Shape shape)
	{
		if (shape == null)
		{
			return;
		}

		foreach (Transform child in shape.transform)
		{
			Vector2 pos = Vectorf.Round(child.position);
			_grid[(int)pos.x, (int)pos.y] = child;
		}
	}

	public IEnumerator ClearAllRows()
	{
		completedRows = 0;

		for (int y = 0; y < height; ++y)
		{
			if (IsComplete(y))
			{
				ClearRowFX(completedRows, y);
				completedRows++;
			}
		}

		yield return new WaitForSeconds(0.1f);

		for (int y = 0; y < height; ++y)
		{
			if (IsComplete(y))
			{
				ClearRow(y);
				ShiftRowsDown(y + 1);
				yield return new WaitForSeconds(0.1f);
				y--;
			}
		}
	}

	public bool IsOverLimit(Shape shape)
	{
		foreach (Transform child in shape.transform)
		{
			if (child.transform.position.y >= height - header)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsValidPosition(Shape shape)
	{
		foreach (Transform child in shape.transform)
		{
			Vector2 pos = Vectorf.Round(child.position);

			if (!IsWithinBoard((int)pos.x, (int)pos.y))
			{
				return false;
			}

			if (IsOccupied((int)pos.x, (int)pos.y, shape))
			{
				return false;
			}
		}
		return true;
	}


	void Awake()
	{
		_grid = new Transform[width, height];
	}

	void Start()
	{
		DrawEmptyCells();
	}


	bool IsWithinBoard(int x, int y)
	{
		return (x >= 0 && x < width && y >= 0);

	}

	bool IsOccupied(int x, int y, Shape shape)
	{

		return (_grid[x, y] != null && _grid[x, y].parent != shape.transform);
	}

	void DrawEmptyCells()
	{
		if (emptySprite)
		{
			for (int y = 0; y < height - header; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Transform clone;
					clone = Instantiate(emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;

					// names the empty squares for organizational purposes
					clone.name = "Board Space ( x = " + x.ToString() + " , y =" + y.ToString() + " )";

					// parents all of the empty squares to the Board object
					clone.transform.parent = transform;
				}
			}
		}
	}

	bool IsComplete(int y)
	{
		for (int x = 0; x < width; ++x)
		{
			if (_grid[x, y] == null)
			{
				return false;
			}

		}
		return true;
	}

	void ClearRow(int y)
	{
		for (int x = 0; x < width; ++x)
		{
			if (_grid[x, y] != null)
			{
				Destroy(_grid[x, y].gameObject);

			}

			_grid[x, y] = null;
		}
	}

	void ShiftOneRowDown(int y)
	{
		for (int x = 0; x < width; ++x)
		{
			if (_grid[x, y] != null)
			{
				_grid[x, y - 1] = _grid[x, y];
				_grid[x, y] = null;
				_grid[x, y - 1].position += new Vector3(0, -1, 0);
			}
		}
	}

	void ShiftRowsDown(int startY)
	{
		for (int i = startY; i < height; ++i)
		{
			ShiftOneRowDown(i);
		}
	}

	void ClearRowFX(int idx, int y)
	{
		if (m_rowGlowFx[idx])
		{
			m_rowGlowFx[idx].transform.position = new Vector3(0, y, -2f);
			m_rowGlowFx[idx].Play();
		}
	}
}
