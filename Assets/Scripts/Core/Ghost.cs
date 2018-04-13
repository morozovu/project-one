using UnityEngine;

public class Ghost : MonoBehaviour
{
	public Color color = new Color(1f, 1f, 1f, 0.2f);

	Shape _ghostShape = null;
	bool _hitBottom = false;


	public void DrawGhost(Shape originalShape, Board gameBoard)
	{
		if (!_ghostShape)
		{
			_ghostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation) as Shape;
			_ghostShape.gameObject.name = "GhostShape";

			SpriteRenderer[] allRenderers = _ghostShape.GetComponentsInChildren<SpriteRenderer>();

			foreach (SpriteRenderer r in allRenderers)
			{
				r.color = color;
			}

		}
		else
		{
			_ghostShape.transform.position = originalShape.transform.position;
			_ghostShape.transform.rotation = originalShape.transform.rotation;
			_ghostShape.transform.localScale = Vector3.one;

		}

		_hitBottom = false;

		while (!_hitBottom)
		{
			_ghostShape.MoveDown();
			if (!gameBoard.IsValidPosition(_ghostShape))
			{
				_ghostShape.MoveUp();
				_hitBottom = true;
			}
		}

	}

	public void Reset()
	{
		Destroy(_ghostShape.gameObject);
	}
}
