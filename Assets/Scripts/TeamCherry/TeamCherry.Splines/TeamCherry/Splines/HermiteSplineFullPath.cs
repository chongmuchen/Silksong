using System;
using System.Collections.Generic;
using TeamCherry.SharedUtils;
using UnityEngine;

namespace TeamCherry.Splines;

public class HermiteSplineFullPath : HermiteSplineBase, IHermiteSplinePath
{
	[Flags]
	private enum FadeEnds
	{
		None = 0,
		Start = 1,
		End = 2
	}

	[Header("Full Path")]
	[SerializeField]
	[EnumPickerBitmask]
	private FadeEnds fadeEnds;

	[SerializeField]
	private int endFadePoints;

	public int ControlPointCount => base.ControlPoints.Count;

	public void Reverse()
	{
		base.ControlPoints.Reverse();
		UpdateSpline();
	}

	public Vector3 GetControlPoint(int i)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		List<Transform> list = base.ControlPoints;
		if (list == null || i >= list.Count)
		{
			return Vector3.zero;
		}
		return list[i].position;
	}

	public bool CanDeleteControlPoint()
	{
		return base.ControlPoints.Count > 3;
	}

	public void DeleteControlPoint(int i)
	{
		if (CanDeleteControlPoint())
		{
			List<Transform> list = base.ControlPoints;
			Object.DestroyImmediate((Object)(object)((Component)list[i]).gameObject);
			list.RemoveAt(i);
			UpdateSpline();
		}
	}

	public void InsertControlPoint(int activePointIndex, Vector3 mouseWorldPos, out int newPointIndex)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		List<Transform> list = base.ControlPoints;
		Vector3 newControlPointPos = HermiteSplinePath.GetNewControlPointPos(((Component)this).transform, list.Count, (int i) => base.ControlPoints[i].localPosition, activePointIndex, mouseWorldPos, out newPointIndex);
		Transform val = list[activePointIndex];
		Transform transform = new GameObject("point").transform;
		((Component)transform).transform.SetParent(val.parent);
		transform.SetSiblingIndex(newPointIndex);
		((Component)transform).transform.localPosition = newControlPointPos;
		list.Insert(newPointIndex, transform);
		UpdateSpline();
	}

	public void SetControlPoint(int i, Vector3 mouseWorldPos)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		List<Transform> list = base.ControlPoints;
		if (list != null && i < list.Count)
		{
			list[i].position = mouseWorldPos;
			UpdateSpline();
		}
	}

	protected override void UpdateMeshInternal(bool forceNewMesh = false)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		if (endFadePoints <= 0 || fadeEnds == FadeEnds.None)
		{
			base.UpdateMeshInternal(forceNewMesh);
			return;
		}
		Point[] internalPoints = InternalPoints;
		int num = Mathf.Min(endFadePoints, internalPoints.Length);
		if ((fadeEnds & FadeEnds.Start) == FadeEnds.Start)
		{
			for (int i = 0; i < num; i++)
			{
				Point point = internalPoints[i];
				ref Color color = ref point.Color;
				color *= Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)i / (float)num);
				internalPoints[i] = point;
			}
		}
		if ((fadeEnds & FadeEnds.End) == FadeEnds.End)
		{
			for (int j = 0; j < num; j++)
			{
				int num2 = internalPoints.Length - j - 1;
				Point point2 = internalPoints[num2];
				ref Color color2 = ref point2.Color;
				color2 *= Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)j / (float)num);
				internalPoints[num2] = point2;
			}
		}
		base.UpdateMeshInternal(forceNewMesh);
	}
}
