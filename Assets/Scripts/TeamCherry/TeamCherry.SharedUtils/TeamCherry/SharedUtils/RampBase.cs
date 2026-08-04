using System;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.Events;

namespace TeamCherry.SharedUtils
{

public abstract class RampBase : MonoBehaviour
{
	[Serializable]
	public class RampEvents
	{
		public UnityEvent OnRampStart;

		public UnityEvent OnRampEnd;

		public void CallStart()
		{
			UnityEvent onRampStart = OnRampStart;
			if (onRampStart != null)
			{
				onRampStart.Invoke();
			}
		}

		public void CallEnd()
		{
			UnityEvent onRampEnd = OnRampEnd;
			if (onRampEnd != null)
			{
				onRampEnd.Invoke();
			}
		}
	}

	[SerializeField]
	private AnimationCurve rampCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	private float duration;

	[SerializeField]
	private bool resetOnEnd;

	[SerializeField]
	private bool startOnEnable;

	[SerializeField]
	private bool resumeOnEnable;

	public RampEvents Events;

	private Coroutine rampRoutine;

	protected bool started;

	public AnimationCurve RampCurve
	{
		get
		{
			return rampCurve;
		}
		set
		{
			rampCurve = value;
		}
	}

	public float Duration
	{
		get
		{
			return duration;
		}
		set
		{
			duration = value;
		}
	}

	public bool ResetOnEnd
	{
		get
		{
			return resetOnEnd;
		}
		set
		{
			resetOnEnd = value;
		}
	}

	private void OnEnable()
	{
		if (startOnEnable || (resumeOnEnable && started))
		{
			StartRamp();
		}
	}

	[ContextMenu("Start", true)]
	[ContextMenu("Stop", true)]
	public bool CanDoRamp()
	{
		return Application.isPlaying;
	}

	[ContextMenu("Start")]
	public void StartRamp()
	{
		if (!((Component)this).gameObject.activeInHierarchy)
		{
			started = true;
			return;
		}
		StopRamp();
		started = true;
		Events.CallStart();
		rampRoutine = ((MonoBehaviour)(object)this).StartTimerRoutine(0f, duration, delegate(float time)
		{
			UpdateValues(rampCurve.Evaluate(time));
		}, null, delegate
		{
			rampRoutine = null;
			Events.CallEnd();
			if (resetOnEnd)
			{
				ResetValues();
			}
			started = false;
		});
	}

	[ContextMenu("Stop")]
	public void StopRamp()
	{
		if (rampRoutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(rampRoutine);
			Events.CallEnd();
		}
		if (resetOnEnd)
		{
			ResetValues();
		}
		started = false;
	}

	protected abstract void UpdateValues(float multiplier);

	protected abstract void ResetValues();
}
}
