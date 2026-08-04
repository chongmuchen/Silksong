using System;
using Unity.Profiling;

namespace TeamCherry.SharedUtils
{

public abstract class UnityCallbackHooks<T> : ComponentSingleton<T> where T : UnityCallbackHooks<T>
{
	private ProfilerMarker updateProfilerMarker;

	private ProfilerMarker lateUpdateProfilerMarker;

	private ProfilerMarker fixedUpdateProfilerMarker;

	public event Action OnUpdate;

	public event Action OnLateUpdate;

	public event Action OnFixedUpdate;

	private void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		updateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".Update()");
		lateUpdateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".LateUpdate()");
		fixedUpdateProfilerMarker = new ProfilerMarker(((object)this).GetType().Name + ".FixedUpdate()");
	}

	private void Update()
	{
		this.OnUpdate?.Invoke();
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
public class UnityCallbackHooks : UnityCallbackHooks<UnityCallbackHooks>
{
}
}
