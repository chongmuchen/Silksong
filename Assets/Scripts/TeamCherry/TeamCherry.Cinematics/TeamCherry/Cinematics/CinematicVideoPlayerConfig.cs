using UnityEngine;

namespace TeamCherry.Cinematics
{

public class CinematicVideoPlayerConfig
{
	public CinematicVideoReference VideoReference { get; }

	public MeshRenderer MeshRenderer { get; }

	public AudioSource AudioSource { get; }

	public CinematicVideoFaderStyles FaderStyle { get; }

	public float ImplicitVolume { get; }

	public CinematicVideoPlayerConfig(CinematicVideoReference videoReference, MeshRenderer meshRenderer, AudioSource audioSource, CinematicVideoFaderStyles faderStyle, float implicitVolume)
	{
		VideoReference = videoReference;
		MeshRenderer = meshRenderer;
		AudioSource = audioSource;
		FaderStyle = faderStyle;
		ImplicitVolume = implicitVolume;
	}
}
}
