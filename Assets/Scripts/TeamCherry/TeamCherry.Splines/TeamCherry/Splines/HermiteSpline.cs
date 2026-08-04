using System.Collections.Generic;
using UnityEngine;

namespace TeamCherry.Splines;

[ExecuteAlways]
public class HermiteSpline : HermiteSplineBase
{
	[SerializeField]
	private bool normaliseDistances = true;

	public bool NormaliseDistances
	{
		get
		{
			return normaliseDistances;
		}
		set
		{
			normaliseDistances = value;
		}
	}

	protected override void Start()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (Application.isPlaying)
		{
			List<Transform> list = base.ControlPoints;
			if (normaliseDistances && list.Count >= 3)
			{
				Vector3 position = list[0].position;
				Vector3 position2 = list[list.Count - 1].position;
				for (int i = 1; i < list.Count - 1; i++)
				{
					list[i].position = Vector3.Lerp(position, position2, (float)i / (float)(list.Count - 1));
				}
			}
		}
		base.Start();
	}
}
