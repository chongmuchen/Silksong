using System;
using UnityEngine;

namespace TeamCherry.Cinematics;

public abstract class CinematicVideoPlayer : IDisposable
{
	protected CinematicVideoPlayerConfig Config { get; }

	public abstract bool IsLoading { get; }

	public abstract bool IsPlaying { get; }

	public abstract bool IsLooping { get; set; }

	public abstract float Volume { get; set; }

	public abstract float CurrentTime { get; set; }

	public virtual bool SkipFrameOnDrop
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual int SkippedStartFrames { get; set; }

	protected CinematicVideoPlayer(CinematicVideoPlayerConfig config)
	{
		Config = config;
		if (Object.op_Implicit((Object)(object)config.AudioSource))
		{
			config.AudioSource.clip = config.VideoReference.Audio;
			config.AudioSource.playOnAwake = false;
			config.AudioSource.Stop();
			if ((Object)(object)config.VideoReference.Audio != (Object)null)
			{
				config.VideoReference.Audio.LoadAudioData();
			}
		}
	}

	public virtual void Dispose()
	{
	}

	public abstract void Play();

	public abstract void Stop();

	public virtual void Update()
	{
	}

	public static CinematicVideoPlayer Create(CinematicVideoPlayerConfig config)
	{
		return new DesktopCinematicVideoPlayer(config);
	}

	protected void PlayAudio(float time)
	{
		if (Object.op_Implicit((Object)(object)Config.AudioSource))
		{
			Config.AudioSource.Stop();
			Config.AudioSource.time = time;
			Config.AudioSource.Play();
		}
	}

	protected void StopAudio()
	{
		if (Object.op_Implicit((Object)(object)Config.AudioSource))
		{
			Config.AudioSource.Stop();
		}
	}
}
