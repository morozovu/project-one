using UnityEngine;

public class Holder : MonoBehaviour
{
	public Transform holdXform;
	public Shape heldShape = null;
	public bool canRelease = false;

	float _scale = 0.5f;


	public void Catch(Shape shape)
	{
		if (!shape)
		{
			Debug.LogWarning("HOLDER WARNING! " + shape.name + " is invalid!");
			return;
		}

		if (!holdXform)
		{
			Debug.LogWarning("HOLDER WARNING! Missing Holder transform!");
			return;
		}

		if (heldShape)
		{
			Debug.LogWarning("HOLDER WARNING!  Release a shape before trying to hold.");
			return;
		}
		else
		{
			shape.transform.position = holdXform.position + shape.queueOffset;
			shape.transform.localScale = new Vector3(_scale, _scale, _scale);
			heldShape = shape;
			shape.transform.rotation = Quaternion.identity;
		}
	}

	public Shape Release()
	{
		if (heldShape)
		{
			heldShape.transform.localScale = Vector3.one;
			Shape shape = heldShape;
			heldShape = null;
			canRelease = false;
			return shape;

		}
		Debug.LogWarning("HOLDER WARNING!  Holder contains no shape!");
		return null;
	}
}
