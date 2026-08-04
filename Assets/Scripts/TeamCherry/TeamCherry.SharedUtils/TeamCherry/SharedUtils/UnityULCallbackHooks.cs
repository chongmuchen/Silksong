using System;
using Unity.Profiling;

namespace TeamCherry.SharedUtils
{

public abstract class UnityULCallbackHooks<T> : ComponentSingleton<T> where T : UnityULCallbackHooks<T>
{
	private ProfilerMarker updateProfilerMarker;

	private ProfilerMarker lateUpdateProfilerMarker;

	public event Action OnUpdate;

	public event Action OnLateUpdate;

	private void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		updateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".Update()");
		lateUpdateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".LateUpdate()");
	}

	private void Update()
	{
		this.OnUpdate?.Invoke();
	}

	private void LateUpdate()
	{
		this.OnLateUpdate?.Invoke();
	}
}
public class UnityULCallbackHooks : UnityULCallbackHooks<UnityULCallbackHooks>
{
}
}
