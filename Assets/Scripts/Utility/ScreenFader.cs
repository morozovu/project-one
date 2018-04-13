using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{

	// our starting alpha value
	public float startAlpha = 1f;

	// our ending alpha value
	public float targetAlpha = 0f;

	// how long in seconds before we start the fade effect
	public float delay = 0f;

	// how long it takes to change from start to target alpha values
	public float timeToFade = 1f;


	// increment applied to our alpha per frame
	float _inc;

	// the current value of the alpha
	float _currentAlpha;

	// reference to our MaskableGraphic component
	MaskableGraphic _graphic;

	// the graphic's original color
	Color _originalColor;


	void Start()
	{
		// cache the Maskable Graphic
		_graphic = GetComponent<MaskableGraphic>();

		// cache our graphic's original color
		_originalColor = _graphic.color;

		// set our current alpha to the starting value
		_currentAlpha = startAlpha;

		// set the color of the graphic, based on the original rgb and current alpha value
		Color tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
		_graphic.color = tempColor;

		// calculate how much we increment the alpha based on our transition time; this rate is per FRAME, note Time.deltaTime
		_inc = ((targetAlpha - startAlpha) / timeToFade) * Time.deltaTime;

		// start our coroutine
		StartCoroutine(FadeRoutine());
	}


	IEnumerator FadeRoutine()
	{
		// wait to begin the fade effect
		yield return new WaitForSeconds(delay);

		while (Mathf.Abs(targetAlpha - _currentAlpha) > 0.01f)
		{
			yield return new WaitForEndOfFrame();

			// increment our alpha value
			_currentAlpha = _currentAlpha + _inc;

			// set the color of the graphic, based on the original rgb and current alpha value
			Color tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
			_graphic.color = tempColor;

		}
	}
}
