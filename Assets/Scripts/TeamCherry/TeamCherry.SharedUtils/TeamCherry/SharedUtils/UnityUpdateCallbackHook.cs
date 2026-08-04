using System;
using Unity.Profiling;

namespace TeamCherry.SharedUtils;

public abstract class UnityUpdateCallbackHook<T> : ComponentSingleton<T> where T : UnityUpdateCallbackHook<T>
{
	private ProfilerMarker updateProfilerMarker;

	public event Action OnUpdate;

	private void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		updateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".Update()");
	}

	private void Update()
	{
		this.OnUpdate?.Invoke();
	}
}
public class UnityUpdateCallbackHook : UnityUpdateCallbackHook<UnityUpdateCallbackHook>
{
}
