using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public OnDealDamage OnDealDamage;
	[SerializeField] private Material screenVignetteMat;
	private Coroutine vignetteCoroutine;

	private void Awake()
	{
		OnDealDamage += triggerVignette;
	}

	private void triggerVignette()
	{
		vignetteCoroutine ??= StartCoroutine(flashVignette());
	}

	private IEnumerator flashVignette()
	{
		float _timer = 0;
		const float _duration = 0.1f;
		while (_timer < _duration)
		{
			_timer += Time.deltaTime;
			yield return null;
			screenVignetteMat.SetFloat("_Alpha", _timer / _duration);
		}
		_timer = 0;

		screenVignetteMat.SetFloat("_Alpha", 1);
		yield return new WaitForSeconds(0.1f);
		vignetteCoroutine = null;
	}
}

public delegate void OnDealDamage();