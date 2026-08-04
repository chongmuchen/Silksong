using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TeamCherry.SharedUtils;

public abstract class FloatCurveAnimator : BaseAnimator
{
	[SerializeField]
	private MinMaxFloat range = new MinMaxFloat(0f, 1f);

	[SerializeField]
	private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	private float duration = 1f;

	[SerializeField]
	[HideInInspector]
	[Obsolete]
	private float delay;

	[SerializeField]
	private MinMaxFloat delayRange;

	[SerializeField]
	private bool isRealtime;

	[SerializeField]
	private bool playOnEnable;

	[SerializeField]
	private bool loop;

	[SerializeField]
	private bool resetOnPlay = true;

	[SerializeField]
	private float framerate;

	[Space]
	public UnityEvent OnStart;

	public UnityEvent OnStop;

	private double nextUpdateTime;

	private float? initialValue;

	private Coroutine animationRoutine;

	private Action<float> setLocalPosition;

	protected abstract float Value { get; set; }

	private void OnValidate()
	{
		if (delay != 0f)
		{
			delayRange = new MinMaxFloat(delay, delay);
			delay = 0f;
		}
	}

	private void Awake()
	{
		OnValidate();
	}

	private void OnEnable()
	{
		if (playOnEnable)
		{
			StartAnimation();
		}
	}

	private void OnDisable()
	{
		if (animationRoutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(animationRoutine);
		}
	}

	public override void StartAnimation()
	{
		if (!resetOnPlay || !initialValue.HasValue)
		{
			initialValue = Value;
		}
		if (animationRoutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(animationRoutine);
		}
		animationRoutine = ((MonoBehaviour)this).StartCoroutine(AnimationRoutine());
	}

	public void ForceStop()
	{
		Stop(setAtEnd: true);
	}

	public void StopAtCurrentPoint()
	{
		Stop(setAtEnd: false);
	}

	private void Stop(bool setAtEnd)
	{
		if (setLocalPosition != null)
		{
			if (animationRoutine != null)
			{
				((MonoBehaviour)this).StopCoroutine(animationRoutine);
				animationRoutine = null;
			}
			if (setAtEnd)
			{
				setLocalPosition(1f);
			}
			setLocalPosition = null;
		}
	}

	private IEnumerator AnimationRoutine()
	{
		setLocalPosition = delegate(float num)
		{
			num = curve.Evaluate(num);
			Value = range.GetLerpedValue(num);
		};
		setLocalPosition(0f);
		float randomValue = delayRange.GetRandomValue();
		if (randomValue > 0f)
		{
			yield return (object)new WaitForSeconds(randomValue);
		}
		OnStart.Invoke();
		float elapsed = 0f;
		while (loop || elapsed < duration)
		{
			if (duration <= 0f)
			{
				Debug.LogError((object)"Duration can not be less than or equal to 0!", (Object)(object)this);
				if (!loop)
				{
					break;
				}
				yield return null;
				continue;
			}
			bool flag = true;
			double time = GetTime();
			if (framerate > 0f)
			{
				if (time >= nextUpdateTime)
				{
					nextUpdateTime = time + (double)(1f / framerate);
				}
				else
				{
					flag = false;
				}
			}
			if (elapsed > duration)
			{
				elapsed %= duration;
			}
			if (flag)
			{
				setLocalPosition(elapsed / duration);
			}
			yield return null;
			elapsed += (isRealtime ? Time.unscaledDeltaTime : Time.deltaTime);
		}
		setLocalPosition(1f);
		setLocalPosition = null;
		OnStop.Invoke();
	}

	private double GetTime()
	{
		if (!isRealtime)
		{
			return Time.timeAsDouble;
		}
		return Time.unscaledTimeAsDouble;
	}
}
