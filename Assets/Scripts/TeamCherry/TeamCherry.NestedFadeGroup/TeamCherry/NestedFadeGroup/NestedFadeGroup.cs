using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup;

[ExecuteAlways]
[DisallowMultipleComponent]
public class NestedFadeGroup : NestedFadeGroupBase
{
	private struct Bridge
	{
		public Type SourceType;

		public Type DestinationType;
	}

	private readonly HashSet<NestedFadeGroupBase> children = new HashSet<NestedFadeGroupBase>();

	private readonly List<NestedFadeGroupBase> childrenTemp = new List<NestedFadeGroupBase>();

	private readonly List<NestedFadeGroupBase> runningList = new List<NestedFadeGroupBase>();

	private static List<Bridge> _bridges;

	private bool enabledState;

	private bool childrenValid;

	protected override void OnAlphaChanged(float alpha)
	{
		childrenTemp.AddRange(children);
		foreach (NestedFadeGroupBase item in childrenTemp)
		{
			item.UpdateAlpha(alpha);
		}
		childrenTemp.Clear();
	}

	protected override void OnEnable()
	{
		if (!Object.op_Implicit((Object)(object)base.ParentGroup))
		{
			AddMissingBridgeComponents();
		}
		base.OnEnable();
		if (((Behaviour)this).enabled == enabledState || childrenValid)
		{
			return;
		}
		childrenValid = true;
		GetChildrenUntilNextFadeGroup(((Component)this).transform);
		foreach (NestedFadeGroupBase running in runningList)
		{
			running.UpdateAndRefresh(forced: true);
		}
		runningList.Clear();
	}

	private void GetChildrenUntilNextFadeGroup(Transform parent)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (Transform item in parent)
		{
			Transform val = item;
			NestedFadeGroupBase component = ((Component)val).GetComponent<NestedFadeGroupBase>();
			if (Object.op_Implicit((Object)(object)component))
			{
				runningList.Add(component);
				if (component is NestedFadeGroup && ((Behaviour)component).enabled)
				{
					continue;
				}
			}
			GetChildrenUntilNextFadeGroup(val);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (((Behaviour)this).enabled)
		{
			return;
		}
		enabledState = false;
		if (!((Component)this).gameObject.activeSelf)
		{
			return;
		}
		childrenValid = false;
		childrenTemp.AddRange(children);
		foreach (NestedFadeGroupBase item in childrenTemp)
		{
			item.SetParent(base.ParentGroup);
		}
		childrenTemp.Clear();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		childrenTemp.AddRange(children);
		foreach (NestedFadeGroupBase item in childrenTemp)
		{
			item.SetParent(base.ParentGroup);
		}
		childrenTemp.Clear();
	}

	private void OnTransformChildrenChanged()
	{
		AddMissingBridgeComponents();
	}

	public void AddMissingBridgeComponents()
	{
		if (_bridges == null)
		{
			_bridges = new List<Bridge>();
			foreach (Type item in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly assembly) => assembly.GetTypes()))
			{
				object[] customAttributes = item.GetCustomAttributes(typeof(NestedFadeGroupBridgeAttribute), inherit: true);
				for (int num = 0; num < customAttributes.Length; num++)
				{
					Type[] targetTypes = ((NestedFadeGroupBridgeAttribute)customAttributes[num]).TargetTypes;
					foreach (Type sourceType in targetTypes)
					{
						_bridges.Add(new Bridge
						{
							SourceType = sourceType,
							DestinationType = item
						});
					}
				}
			}
		}
		foreach (Bridge bridge in _bridges)
		{
			AddMissingBridgeComponents(bridge.SourceType, bridge.DestinationType);
		}
	}

	private void AddMissingBridgeComponents(Type sourceType, Type destinationType)
	{
		Component[] componentsInChildren = ((Component)this).GetComponentsInChildren(sourceType, true);
		foreach (Component val in componentsInChildren)
		{
			if (!Object.op_Implicit((Object)(object)val.GetComponent(destinationType)))
			{
				NestedFadeGroupBase.QueuedOnComponentAdded = true;
				val.gameObject.AddComponent(destinationType);
				NestedFadeGroupBase.QueuedOnComponentAdded = false;
			}
		}
	}

	public void AddChild(NestedFadeGroupBase child)
	{
		children.Add(child);
	}

	public void RemoveChild(NestedFadeGroupBase child)
	{
		children.Remove(child);
	}
}
