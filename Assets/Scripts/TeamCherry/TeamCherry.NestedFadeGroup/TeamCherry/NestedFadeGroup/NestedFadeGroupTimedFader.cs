using TeamCherry.SharedUtils;
using UnityEngine;
using UnityEngine.Events;

namespace TeamCherry.NestedFadeGroup
{

public class NestedFadeGroupTimedFader : MonoBehaviour
{
	[SerializeField]
	private NestedFadeGroupBase fade;

	[SerializeField]
	private float startDelay;

	[SerializeField]
	private float duration;

	[SerializeField]
	private MinMaxFloat fadeAlpha;

	[SerializeField]
	private bool manualTrigger;

	[SerializeField]
	private bool disableOnEnd;

	[Space]
	public UnityEvent OnFadeStart;

	private float delayLeft;

	private float fadeTimeLeft;

	private void OnEnable()
	{
		if ((fade != null))
		{
			if (!manualTrigger)
			{
				Fade();
			}
			else
			{
				fade.AlphaSelf = fadeAlpha.Start;
			}
		}
	}

	private void Update()
	{
		if (delayLeft > 0f)
		{
			delayLeft -= Time.deltaTime;
			if (delayLeft <= 0f)
			{
				StartFade();
			}
		}
		if (fadeTimeLeft > 0f)
		{
			fadeTimeLeft -= Time.deltaTime;
			if (fadeTimeLeft <= 0f && disableOnEnd)
			{
				((Component)this).gameObject.SetActive(false);
			}
		}
	}

	public void Fade()
	{
		if (startDelay <= 0f)
		{
			StartFade();
			return;
		}
		fade.AlphaSelf = fadeAlpha.Start;
		delayLeft = startDelay;
	}

	private void StartFade()
	{
		fade.AlphaSelf = fadeAlpha.Start;
		fadeTimeLeft = fade.FadeTo(fadeAlpha.End, duration);
		OnFadeStart.Invoke();
	}
}
}
