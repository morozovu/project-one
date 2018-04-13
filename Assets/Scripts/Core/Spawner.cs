using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Shape[] allShapes;
	public Transform[] queuedXforms = new Transform[1];
	public float queueScale = 0.5f;
	public ParticlePlayer spawnFx;

	Shape[] _queuedShapes = new Shape[1];


	public Shape SpawnShape()
	{
		Shape shape = null;

		// use the Queue
		shape = GetQueuedShape();
		shape.transform.position = transform.position;

		StartCoroutine(GrowShape(shape, transform.position, 0.25f));

		if (spawnFx)
		{
			spawnFx.Play();
		}

		if (shape)
		{
			return shape;
		}
		else
		{
			Debug.LogWarning("WARNING! Invalid shape in spawner!");
			return null;
		}
	}


	void Awake()
	{
		InitQueue();
	}


	Shape GetRandomShape()
	{
		int i = Random.Range(0, allShapes.Length);
		if (allShapes[i])
		{
			return allShapes[i];
		}
		else
		{
			Debug.LogWarning("WARNING! Invalid shape in spawner!");
			return null;
		}
	}

	void InitQueue()
	{
		for (int i = 0; i < _queuedShapes.Length; i++)
		{
			_queuedShapes[i] = null;
		}
		FillQueue();
	}

	void FillQueue()
	{
		for (int i = 0; i < _queuedShapes.Length; i++)
		{
			if (!_queuedShapes[i])
			{
				_queuedShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
				_queuedShapes[i].transform.position = queuedXforms[i].position + _queuedShapes[i].queueOffset;

				_queuedShapes[i].transform.localScale = new Vector3(queueScale, queueScale, queueScale);
			}
		}
	}

	Shape GetQueuedShape()
	{
		Shape firstShape = null;

		// designate the 0 index Shape in the queue 
		if (_queuedShapes[0])
		{
			firstShape = _queuedShapes[0];
		}

		// set Shapes1,2... to 0,1,... and move their positions forward in the queue
		for (int i = 1; i < _queuedShapes.Length; i++)
		{
			_queuedShapes[i - 1] = _queuedShapes[i];
			_queuedShapes[i - 1].transform.position = queuedXforms[i - 1].position + _queuedShapes[i].queueOffset;
		}

		// set the last space to null
		_queuedShapes[_queuedShapes.Length - 1] = null;

		// fill the empty resulting space after shifting
		FillQueue();

		// returns either the first Shape (or null if the queue is empty)
		return firstShape;
	}

	IEnumerator GrowShape(Shape shape, Vector3 position, float growTime = 0.5f)
	{
		float size = 0f;
		growTime = Mathf.Clamp(growTime, 0.1f, 2f);
		float sizeDelta = Time.deltaTime / growTime;

		while (size < 1f)
		{
			shape.transform.localScale = new Vector3(size, size, size);
			size += sizeDelta;
			shape.transform.position = position;
			yield return null;
		}

		shape.transform.localScale = Vector3.one;
	}
}
