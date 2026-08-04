using System;
using UnityEngine;

namespace TeamCherry.NestedFadeGroup
{

[ExecuteAlways]
[NestedFadeGroupBridge(new Type[] { typeof(ParticleSystem) })]
[RequireComponent(typeof(ParticleSystem))]
public class NestedFadeGroupParticleSystem : NestedFadeGroupBase
{
	private ParticleSystem system;

	private ParticleSystem.Particle[] particles;

	private bool hasSystem;

	private bool hasParticles;

	protected override void GetMissingReferences()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		if (hasSystem)
		{
			return;
		}
		hasSystem = ((system = ((Component)this).GetComponent<ParticleSystem>()) != null);
		ParticleSystem.MainModule main = system.main;
		int num = main.maxParticles;
		if (!main.loop)
		{
			ParticleSystem.EmissionModule emission = system.emission;
			if (IsCurveZero(emission.rateOverDistance))
			{
				float num2 = 0f;
				ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
				ParticleSystemCurveMode mode = rateOverTime.mode;
				if ((int)mode != 0)
				{
					if ((int)mode == 3)
					{
						rateOverTime = emission.rateOverTime;
						num2 = rateOverTime.constantMax;
					}
				}
				else
				{
					rateOverTime = emission.rateOverTime;
					num2 = rateOverTime.constant;
				}
				if (num2 > 0f)
				{
					num = Mathf.CeilToInt(num2 * main.duration);
				}
			}
		}
		if (num <= 1000)
		{
			particles = new ParticleSystem.Particle[num];
		}
		else
		{
			particles = null;
		}
		hasParticles = particles != null;
	}

	public void UpdateParticlesArraySize()
	{
		hasSystem = false;
		hasParticles = false;
		GetMissingReferences();
	}

	private bool IsCurveZero(ParticleSystem.MinMaxCurve curve)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected I4, but got Unknown
		ParticleSystemCurveMode mode = curve.mode;
		switch ((int)mode)
		{
		case 0:
			return curve.constant <= 0f;
		case 1:
			return IsCurveZero(curve.curve);
		case 3:
			if (curve.constantMin <= 0f)
			{
				return curve.constantMax <= 0f;
			}
			return false;
		case 2:
			if (IsCurveZero(curve.curveMin))
			{
				return IsCurveZero(curve.curveMax);
			}
			return false;
		default:
			return false;
		}
	}

	private bool IsCurveZero(AnimationCurve curve)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if (curve == null || curve.length == 0)
		{
			return true;
		}
		Keyframe[] keys = curve.keys;
		for (int i = 0; i < keys.Length; i++)
		{
			Keyframe val = keys[i];
			if (val.value > 0f)
			{
				return false;
			}
		}
		return true;
	}

	protected override void OnAlphaChanged(float alpha)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (!hasParticles || !hasSystem)
		{
			return;
		}
		ParticleSystem.MainModule main = system.main;
		ParticleSystem.MinMaxGradient startColor = main.startColor;
		Color color = startColor.color;
		color.a = alpha;
		startColor.color = color;
		main.startColor = startColor;
		Color32 startColor2 = (Color32)(color);
		int num = system.GetParticles(particles);
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				particles[i].startColor = startColor2;
			}
		}
		system.SetParticles(particles, num);
	}
}
}
