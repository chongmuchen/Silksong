using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.Video;

namespace TeamCherry.Cinematics
{

public abstract class EmbeddedCinematicVideoPlayer : CinematicVideoPlayer
{
	private bool skipFrameOnDrop;

	private VideoPlayer videoPlayer;

	private readonly Texture originalMainTexture;

	private const string TEXTURE_PROP_NAME = "_MainTex";

	private static readonly int _mainTexId = Shader.PropertyToID("_MainTex");

	private bool isPlayEnqueued;

	private float requestedTime;

	private bool isStopped;

	private float lastSeekedTime;

	private bool isSeeking;

	private bool isDisposed;

	private bool isPrepared;

	private long lastFrame;

	private float timeSinceLastFrameChange;

	private float frameDuration = 1f / 30f;

	public override bool SkipFrameOnDrop
	{
		get
		{
			return skipFrameOnDrop;
		}
		set
		{
			skipFrameOnDrop = value;
			if ((videoPlayer != null))
			{
				videoPlayer.skipOnDrop = value;
			}
		}
	}

	public override float CurrentTime
	{
		get
		{
			return (float)videoPlayer.time;
		}
		set
		{
			videoPlayer.time = value;
			requestedTime = value;
		}
	}

	public override float Volume
	{
		get
		{
			if (!((Object)(object)base.Config.AudioSource != (Object)null))
			{
				return 1f;
			}
			return base.Config.AudioSource.volume;
		}
		set
		{
			if ((Object)(object)base.Config.AudioSource != (Object)null)
			{
				base.Config.AudioSource.volume = value;
			}
		}
	}

	public override bool IsLoading
	{
		get
		{
			if ((Object)(object)videoPlayer != (Object)null && (!videoPlayer.isPrepared || isSeeking) && !isStopped)
			{
				return !isPrepared;
			}
			return false;
		}
	}

	public override bool IsLooping
	{
		get
		{
			if ((Object)(object)videoPlayer != (Object)null)
			{
				return videoPlayer.isLooping;
			}
			return false;
		}
		set
		{
			if ((Object)(object)videoPlayer != (Object)null)
			{
				videoPlayer.isLooping = value;
			}
		}
	}

	public override bool IsPlaying
	{
		get
		{
			if (isDisposed)
			{
				return false;
			}
			if ((Object)(object)videoPlayer != (Object)null && videoPlayer.isPrepared)
			{
				return videoPlayer.isPlaying;
			}
			return isPlayEnqueued;
		}
	}

	protected EmbeddedCinematicVideoPlayer(CinematicVideoPlayerConfig config)
		: base(config)
	{
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		try
		{
			originalMainTexture = ((Renderer)config.MeshRenderer).material.GetTexture(_mainTexId);
			videoPlayer = ((Component)config.MeshRenderer).gameObject.AddComponent<VideoPlayer>();
			videoPlayer.playOnAwake = false;
			skipFrameOnDrop = videoPlayer.skipOnDrop;
			videoPlayer.timeUpdateMode = (VideoTimeUpdateMode)0;
			if ((config.VideoReference.Audio != null))
			{
				videoPlayer.audioOutputMode = (VideoAudioOutputMode)0;
			}
			else
			{
				videoPlayer.audioOutputMode = (VideoAudioOutputMode)1;
				videoPlayer.SetTargetAudioSource((ushort)0, config.AudioSource);
			}
			videoPlayer.renderMode = (VideoRenderMode)3;
			videoPlayer.targetMaterialRenderer = (Renderer)(object)config.MeshRenderer;
			videoPlayer.targetMaterialProperty = "_MainTex";
			videoPlayer.waitForFirstFrame = true;
			string absolutePath = GetAbsolutePath();
			if (File.Exists(absolutePath))
			{
				videoPlayer.url = new Uri(absolutePath).AbsoluteUri;
			}
			else
			{
				VideoClip embeddedVideoClip = config.VideoReference.EmbeddedVideoClip;
				videoPlayer.clip = embeddedVideoClip;
			}
			videoPlayer.seekCompleted += OnSeekCompleted;
			videoPlayer.prepareCompleted += OnPrepareCompleted;
			videoPlayer.started += OnVideoStarted;
			videoPlayer.Prepare();
		}
		catch (Exception)
		{
		}
	}

	protected abstract string GetAbsolutePath();

	public override void Dispose()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		if (isDisposed)
		{
			return;
		}
		isDisposed = true;
		base.Dispose();
		if (!((Object)(object)videoPlayer == (Object)null))
		{
			videoPlayer.seekCompleted -= OnSeekCompleted;
			videoPlayer.prepareCompleted -= OnPrepareCompleted;
			videoPlayer.started -= OnVideoStarted;
			videoPlayer.Stop();
			Object.Destroy((Object)(object)videoPlayer);
			videoPlayer = null;
			MeshRenderer meshRenderer = base.Config.MeshRenderer;
			if ((Object)(object)meshRenderer != (Object)null)
			{
				((Renderer)meshRenderer).material.SetTexture(_mainTexId, originalMainTexture);
			}
		}
	}

	public override void Play()
	{
		isStopped = false;
		if ((Object)(object)videoPlayer != (Object)null && videoPlayer.isPrepared)
		{
			PlayVideo();
		}
		isPlayEnqueued = true;
	}

	public override void Update()
	{
		_ = isDisposed;
		if (!isPrepared || isStopped || videoPlayer.isLooping || videoPlayer.isPaused)
		{
			return;
		}
		if (videoPlayer.isPlaying)
		{
			isPlayEnqueued = false;
			long frame = videoPlayer.frame;
			double time = videoPlayer.time;
			double length = videoPlayer.length;
			double num = length - time;
			double num2 = time / length;
			if (frame == lastFrame)
			{
				timeSinceLastFrameChange += Time.deltaTime;
				if ((num2 >= 0.99 || num <= 0.25) && timeSinceLastFrameChange >= frameDuration * 4f)
				{
					videoPlayer.Stop();
				}
			}
			else
			{
				timeSinceLastFrameChange = 0f;
			}
			lastFrame = frame;
		}
		else
		{
			timeSinceLastFrameChange = 0f;
		}
	}

	private void OnSeekCompleted(VideoPlayer source)
	{
		isSeeking = false;
		if (isPlayEnqueued && (Object)(object)videoPlayer != (Object)null && videoPlayer.isPrepared)
		{
			PlayVideo();
		}
	}

	private void Seek(float time)
	{
		if (!Mathf.Approximately(lastSeekedTime, time) && (Object)(object)videoPlayer != (Object)null && videoPlayer.canSetTime)
		{
			isSeeking = true;
			lastSeekedTime = time;
			videoPlayer.time = time;
		}
	}

	private void PlayVideo()
	{
		Seek(requestedTime);
		if (!isSeeking)
		{
			isPrepared = true;
			videoPlayer.Play();
			PlayAudio(requestedTime);
		}
		else
		{
			isPlayEnqueued = true;
		}
	}

	public override void Stop()
	{
		isStopped = true;
		if ((Object)(object)videoPlayer != (Object)null)
		{
			videoPlayer.Stop();
			if (!(base.Config.VideoReference.Audio != null))
			{
				StopAudio();
			}
		}
		isPlayEnqueued = false;
	}

	private void OnPrepareCompleted(VideoPlayer source)
	{
		if (!((Object)(object)source != (Object)(object)videoPlayer) && !((Object)(object)videoPlayer == (Object)null))
		{
			videoPlayer.skipOnDrop = skipFrameOnDrop;
			isPrepared = true;
			float frameRate = videoPlayer.frameRate;
			if (frameRate > 30f)
			{
				frameDuration = 1f / frameRate;
			}
			if (isPlayEnqueued)
			{
				PlayVideo();
			}
		}
	}

	private void OnVideoStarted(VideoPlayer source)
	{
		isPlayEnqueued = false;
	}
}
}
