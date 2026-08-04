using System.IO;
using UnityEngine;

namespace TeamCherry.Cinematics;

public class DesktopCinematicVideoPlayer : EmbeddedCinematicVideoPlayer
{
	public DesktopCinematicVideoPlayer(CinematicVideoPlayerConfig config)
		: base(config)
	{
	}

	protected override string GetAbsolutePath()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		RuntimePlatform platform = Application.platform;
		CinematicFormats format = (((int)platform != 13 && (int)platform != 16) ? CinematicFormats.MP4_H264_1080_Any_AAC_48000 : CinematicFormats.WEBM_VP8_1080_Any_Vorbis_48000);
		return Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, base.Config.VideoReference.VideoFileName + CinematicFormatUtils.GetExtension(format)));
	}
}
