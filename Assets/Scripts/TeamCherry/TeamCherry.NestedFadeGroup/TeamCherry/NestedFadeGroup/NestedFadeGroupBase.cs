using System;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

[ExecuteAlways]
public abstract class NestedFadeGroupBase : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	private float alpha = 1f;

	[SerializeField]
	[HideInInspector]
	[Obsolete]
	private bool exclude;

	[SerializeField]
	private OverrideNestedFadeGroup parentOverride = new OverrideNestedFadeGroup();

	private float previousAlpha = -1f;

	private bool previousParentOverrideEnabled;

	private NestedFadeGroupBase previousParentOverrideValue;

	private Action<bool> fadeCallback;

	private Coroutine fadeRoutine;

	private float fadeStartAlpha;

	private float fadeToAlpha;

	private bool hasParent;

	private float internalAlphaTotal = -1f;

	private bool isSubscribedToParent;

	[NonSerialized]
	private bool parentIsValid;

	protected bool started;

	public float AlphaSelf
	{
		get
		{
			return alpha;
		}
		set
		{
			alpha = value;
			previousAlpha = value;
			RefreshAlpha();
		}
	}

	protected virtual float ExtraAlpha => 1f;

	public OverrideNestedFadeGroup ParentOverride
	{
		get
		{
			return parentOverride;
		}
		set
		{
			parentOverride = value ?? new OverrideNestedFadeGroup();
			parentIsValid = false;
			UpdateParent();
		}
	}

	public float AlphaTotal { get; private set; }

	public NestedFadeGroup ParentGroup { get; private set; }

	protected static bool QueuedOnComponentAdded { get; set; }

	protected virtual void Start()
	{
		started = true;
		ComponentSingleton<NestedFadeGroupBaseCallbackHooks>.Instance.OnLateUpdate += OnLateUpdate;
		OnLateUpdate();
	}

	private void OnValidate()
	{
		if (exclude)
		{
			parentOverride = new OverrideNestedFadeGroup
			{
				IsEnabled = true,
				Value = null
			};
			exclude = false;
		}
	}

	protected virtual void Awake()
	{
		OnValidate();
		parentIsValid = false;
	}

	protected virtual void OnEnable()
	{
		if (started)
		{
			ComponentSingleton<NestedFadeGroupBaseCallbackHooks>.Instance.OnLateUpdate += OnLateUpdate;
		}
		previousAlpha = -1f;
		previousParentOverrideEnabled = false;
		previousParentOverrideValue = null;
		if (!UpdateParent())
		{
			RefreshAlpha();
		}
	}

	protected virtual void OnDisable()
	{
		ComponentSingleton<NestedFadeGroupBaseCallbackHooks>.Instance.OnLateUpdate -= OnLateUpdate;
	}

	protected virtual void OnDestroy()
	{
		UnsubscribeFromParent();
	}

	protected virtual void OnLateUpdate()
	{
		if (previousAlpha < 0f || Math.Abs(alpha - previousAlpha) > 0.0001f)
		{
			previousAlpha = alpha;
			AlphaSelf = alpha;
		}
		if (previousParentOverrideEnabled != parentOverride.IsEnabled || (Object)(object)previousParentOverrideValue != (Object)(object)parentOverride.Value)
		{
			previousParentOverrideEnabled = parentOverride.IsEnabled;
			previousParentOverrideValue = parentOverride.Value;
			ParentOverride = parentOverride;
		}
	}

	protected virtual void OnTransformParentChanged()
	{
		parentIsValid = false;
		UpdateParent();
	}

	public void UpdateAndRefresh(bool forced = false)
	{
		if (forced)
		{
			parentIsValid = false;
		}
		if (!UpdateParent())
		{
			RefreshAlpha();
		}
	}

	public bool UpdateParent()
	{
		if (!((Behaviour)this).enabled)
		{
			return false;
		}
		if (parentIsValid)
		{
			return false;
		}
		parentIsValid = true;
		NestedFadeGroup parentGroup = GetParentGroup(((Component)this).transform);
		return SetParent(parentGroup);
	}

	private NestedFadeGroup GetParentGroup(Transform currentTransform)
	{
		if (parentOverride.IsEnabled)
		{
			return parentOverride.Value;
		}
		while (true)
		{
			if ((Object)(object)currentTransform == (Object)null)
			{
				return null;
			}
			NestedFadeGroup component = ((Component)currentTransform).GetComponent<NestedFadeGroup>();
			if ((Object)(object)component != (Object)null && ((Behaviour)component).enabled && (Object)(object)component != (Object)(object)this)
			{
				return component;
			}
			Transform parent = currentTransform.parent;
			if (!Object.op_Implicit((Object)(object)parent))
			{
				break;
			}
			currentTransform = parent;
		}
		return null;
	}

	public bool SetParent(NestedFadeGroup parentGroup)
	{
		if ((Object)(object)ParentGroup != (Object)(object)parentGroup)
		{
			UnsubscribeFromParent();
			ParentGroup = parentGroup;
			hasParent = Object.op_Implicit((Object)(object)parentGroup);
			SubscribeToParent();
			RefreshAlpha();
			return true;
		}
		SubscribeToParent();
		return false;
	}

	private void SubscribeToParent()
	{
		if (!isSubscribedToParent && Object.op_Implicit((Object)(object)ParentGroup))
		{
			isSubscribedToParent = true;
			ParentGroup.AddChild(this);
		}
	}

	private void UnsubscribeFromParent()
	{
		if (isSubscribedToParent)
		{
			isSubscribedToParent = false;
			if (Object.op_Implicit((Object)(object)ParentGroup))
			{
				ParentGroup.RemoveChild(this);
			}
		}
	}

	protected void RefreshAlpha(bool forced = false)
	{
		UpdateAlpha(hasParent ? ParentGroup.AlphaTotal : 1f, forced);
	}

	public void UpdateAlpha(float parentAlpha, bool forced = false)
	{
		if (((Component)this).gameObject.activeSelf)
		{
			GetMissingReferences();
			if (QueuedOnComponentAdded)
			{
				QueuedOnComponentAdded = false;
				OnComponentAdded();
			}
			AlphaTotal = alpha * parentAlpha * ExtraAlpha;
			if (forced || internalAlphaTotal != AlphaTotal)
			{
				internalAlphaTotal = AlphaTotal;
				OnAlphaChanged(AlphaTotal);
			}
		}
	}

	public void FadeToZero(float time)
	{
		FadeTo(0f, time);
	}

	public void FadeToOne(float time)
	{
		FadeTo(1f, time);
	}

	public void FadeToZero(float time, Action<bool> callback)
	{
		FadeTo(0f, time, null, isRealtime: false, callback);
	}

	public void FadeToOne(float time, Action<bool> callback)
	{
		FadeTo(1f, time, null, isRealtime: false, callback);
	}

	private void RunFadeCallback(bool finished)
	{
		if (fadeCallback != null)
		{
			Action<bool> action = fadeCallback;
			fadeCallback = null;
			action(finished);
		}
	}

	public float FadeTo(float toAlpha, float fadeTime, AnimationCurve curve = null, bool isRealtime = false, Action<bool> callback = null)
	{
		if (fadeRoutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(fadeRoutine);
		}
		RunFadeCallback(finished: false);
		fadeCallback = callback;
		if (((Component)this).gameObject.activeInHierarchy && fadeTime > 0f)
		{
			fadeStartAlpha = AlphaSelf;
			fadeToAlpha = toAlpha;
			if (curve == null)
			{
				if (Mathf.Abs(fadeStartAlpha - fadeToAlpha) <= Mathf.Epsilon)
				{
					AlphaSelf = toAlpha;
					RunFadeCallback(finished: true);
					return 0f;
				}
				fadeRoutine = ((MonoBehaviour)(object)this).StartTimerRoutine(0f, fadeTime, delegate(float time)
				{
					AlphaSelf = Mathf.Lerp(fadeStartAlpha, fadeToAlpha, time);
				}, null, delegate
				{
					RunFadeCallback(finished: true);
				}, isRealtime);
			}
			else
			{
				fadeRoutine = ((MonoBehaviour)(object)this).StartTimerRoutine(0f, fadeTime, delegate(float time)
				{
					AlphaSelf = Mathf.Lerp(fadeStartAlpha, fadeToAlpha, curve.Evaluate(time));
				}, null, delegate
				{
					RunFadeCallback(finished: true);
				}, isRealtime);
			}
			return fadeTime;
		}
		AlphaSelf = toAlpha;
		RunFadeCallback(finished: true);
		return 0f;
	}

	protected virtual void GetMissingReferences()
	{
	}

	protected virtual void OnComponentAdded()
	{
	}

	protected abstract void OnAlphaChanged(float alpha);
}
