using System;
using Unity.Profiling;

namespace TeamCherry.SharedUtils
{

public abstract class UnityFixedUpdateCallbackHook<T> : ComponentSingleton<T> where T : UnityFixedUpdateCallbackHook<T>
{
	private ProfilerMarker fixedUpdateProfilerMarker;

	public event Action OnFixedUpdate;

	private void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		fixedUpdateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".FixedUpdate()");
	}

	private void FixedUpdate()
	{
		this.OnFixedUpdate?.Invoke();
	}
}
public class UnityFixedUpdateCallbackHook : UnityFixedUpdateCallbackHook<UnityFixedUpdateCallbackHook>
{
}
}
