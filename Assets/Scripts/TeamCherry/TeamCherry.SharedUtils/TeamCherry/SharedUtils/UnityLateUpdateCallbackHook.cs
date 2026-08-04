using System;
using Unity.Profiling;

namespace TeamCherry.SharedUtils
{

public abstract class UnityLateUpdateCallbackHook<T> : ComponentSingleton<T> where T : UnityLateUpdateCallbackHook<T>
{
	private ProfilerMarker lateUpdateProfilerMarker;

	public event Action OnLateUpdate;

	private void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		lateUpdateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".LateUpdate()");
	}

	private void LateUpdate()
	{
		this.OnLateUpdate?.Invoke();
	}
}
public class UnityLateUpdateCallbackHook : UnityLateUpdateCallbackHook<UnityLateUpdateCallbackHook>
{
}
}
