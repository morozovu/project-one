using Assets.Scripts.Model;
using UnityEngine;

public class Shape : MonoBehaviour
{
	public bool canRotate = true;
	public Vector3 queueOffset;
	public string glowSquareTag = "LandShapeFx";
	public ShapeType shapeType;

	GameObject[] _glowSquareFx;


	public void LandShapeFX()
	{
		int i = 0;

		foreach (Transform child in gameObject.transform)
		{
			if (_glowSquareFx[i])
			{
				_glowSquareFx[i].transform.position = new Vector3(child.position.x, child.position.y, -2f);
				ParticlePlayer particlePlayer = _glowSquareFx[i].GetComponent<ParticlePlayer>();

				if (particlePlayer)
				{
					particlePlayer.Play();
				}
			}

			i++;
		}
	}

	public void MoveLeft()
	{
		Move(new Vector3(-1, 0, 0));
	}

	public void MoveRight()
	{
		Move(new Vector3(1, 0, 0));
	}

	public void MoveUp()
	{
		Move(new Vector3(0, 1, 0));
	}

	public void MoveDown()
	{
		Move(new Vector3(0, -1, 0));
	}

	public void RotateRight()
	{
		if (canRotate)
			transform.Rotate(0, 0, -90);
	}

	public void RotateLeft()
	{
		if (canRotate)
			transform.Rotate(0, 0, 90);
	}

	public void RotateClockwise(bool clockwise)
	{
		if (clockwise)
		{
			RotateRight();
		}
		else
		{
			RotateLeft();
		}
	}


	void Start()
	{
		if (glowSquareTag != "")
		{
			_glowSquareFx = GameObject.FindGameObjectsWithTag(glowSquareTag);
		}
	}


	void Move(Vector3 moveDirection)
	{
		transform.position += moveDirection;
	}
}
