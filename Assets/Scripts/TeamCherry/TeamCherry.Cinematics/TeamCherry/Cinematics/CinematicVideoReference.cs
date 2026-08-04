using UnityEngine;
using UnityEngine.Video;

namespace TeamCherry.Cinematics
{

[CreateAssetMenu(menuName = "Team Cherry/Cinematic Video Reference", fileName = "CinematicVideoReference", order = 1000)]
public class CinematicVideoReference : ScriptableObject
{
	[SerializeField]
	private VideoClip embeddedVideoClip;

	[SerializeField]
	private AudioClip audio;

	[SerializeField]
	[HideInInspector]
	private float videoFileLength;

	[SerializeField]
	[HideInInspector]
	private float videoFileFrameRate;

	[SerializeField]
	[HideInInspector]
	private int videoFileWidth;

	[SerializeField]
	[HideInInspector]
	private int videoFileHeight;

	public VideoClip EmbeddedVideoClip => embeddedVideoClip;

	public string VideoFileName => ((Object)this).name;

	public AudioClip Audio => audio;

	public float VideoFileLength => videoFileLength;

	public float VideoFileFrameRate => videoFileFrameRate;

	public int VideoFileWidth => videoFileWidth;

	public int VideoFileHeight => videoFileHeight;
}
}
