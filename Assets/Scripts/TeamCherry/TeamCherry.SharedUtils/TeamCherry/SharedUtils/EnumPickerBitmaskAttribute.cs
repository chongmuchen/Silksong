using System;
using UnityEngine;

namespace TeamCherry.SharedUtils;

[AttributeUsage(AttributeTargets.Field)]
public class EnumPickerBitmaskAttribute : PropertyAttribute
{
	public Type EnumType { get; }

	public EnumPickerBitmaskAttribute()
	{
		EnumType = null;
	}

	public EnumPickerBitmaskAttribute(Type enumType)
	{
		EnumType = enumType;
	}
}
