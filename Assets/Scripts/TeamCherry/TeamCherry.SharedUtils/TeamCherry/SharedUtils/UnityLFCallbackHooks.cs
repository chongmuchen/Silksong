using System;
using Unity.Profiling;

namespace TeamCherry.SharedUtils;

public abstract class UnityLFCallbackHooks<T> : ComponentSingleton<T> where T : UnityLFCallbackHooks<T>
{
	private ProfilerMarker lateUpdateProfilerMarker;

	private ProfilerMarker fixedUpdateProfilerMarker;

	public event Action OnLateUpdate;

	public event Action OnFixedUpdate;

	private void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		lateUpdateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".LateUpdate()");
		fixedUpdateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".FixedUpdate()");
	}

	private void LateUpdate()
	{
		this.OnLateUpdate?.Invoke();
	}

	private void FixedUpdate()
	{
		this.OnFixedUpdate?.Invoke();
	}
}
public class UnityLFCallbackHooks : UnityLFCallbackHooks<UnityLFCallbackHooks>
{
}
