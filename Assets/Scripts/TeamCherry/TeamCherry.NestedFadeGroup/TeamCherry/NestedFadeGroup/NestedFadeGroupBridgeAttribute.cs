using System;

namespace TeamCherry.NestedFadeGroup
{

[AttributeUsage(AttributeTargets.Class)]
public class NestedFadeGroupBridgeAttribute : Attribute
{
	public Type[] TargetTypes { get; private set; }

	public NestedFadeGroupBridgeAttribute(params Type[] targetTypes)
	{
		TargetTypes = targetTypes;
	}
}
}
