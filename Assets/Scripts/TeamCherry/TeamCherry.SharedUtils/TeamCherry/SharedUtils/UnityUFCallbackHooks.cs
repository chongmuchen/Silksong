using System;
using Unity.Profiling;

namespace TeamCherry.SharedUtils
{

public abstract class UnityUFCallbackHooks<T> : ComponentSingleton<T> where T : UnityUFCallbackHooks<T>
{
	private ProfilerMarker updateProfilerMarker;

	private ProfilerMarker fixedUpdateProfilerMarker;

	public event Action OnUpdate;

	public event Action OnFixedUpdate;

	private void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		updateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".Update()");
		fixedUpdateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".FixedUpdate()");
	}

	private void Update()
	{
		this.OnUpdate?.Invoke();
	}

	private void FixedUpdate()
	{
		this.OnFixedUpdate?.Invoke();
	}
}
public class UnityUFCallbackHooks : UnityUFCallbackHooks<UnityUFCallbackHooks>
{
}
}
