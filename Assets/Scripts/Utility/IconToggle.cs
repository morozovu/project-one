using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{
	public Sprite iconTrue;
	public Sprite iconFalse;
	public bool defaultIconState = true;

	Image _image;


	public void ToggleIcon(bool state)
	{
		if (!_image || !iconTrue || !iconFalse)
		{
			Debug.LogWarning("ICONTOGGLE Undefined iconTrue and/or iconFalse!");
			return;
		}

		_image.sprite = (state) ? iconTrue : iconFalse;
	}


	void Start()
	{
		_image = GetComponent<Image>();
		_image.sprite = (defaultIconState) ? iconTrue : iconFalse;
	}
}
