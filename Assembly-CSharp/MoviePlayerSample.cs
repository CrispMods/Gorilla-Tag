using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x020002F4 RID: 756
public class MoviePlayerSample : MonoBehaviour
{
	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06001213 RID: 4627 RVA: 0x00056072 File Offset: 0x00054272
	// (set) Token: 0x06001214 RID: 4628 RVA: 0x0005607A File Offset: 0x0005427A
	public bool IsPlaying { get; private set; }

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06001215 RID: 4629 RVA: 0x00056083 File Offset: 0x00054283
	// (set) Token: 0x06001216 RID: 4630 RVA: 0x0005608B File Offset: 0x0005428B
	public long Duration { get; private set; }

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06001217 RID: 4631 RVA: 0x00056094 File Offset: 0x00054294
	// (set) Token: 0x06001218 RID: 4632 RVA: 0x0005609C File Offset: 0x0005429C
	public long PlaybackPosition { get; private set; }

	// Token: 0x06001219 RID: 4633 RVA: 0x000560A8 File Offset: 0x000542A8
	private void Awake()
	{
		Debug.Log("MovieSample Awake");
		this.mediaRenderer = base.GetComponent<Renderer>();
		this.videoPlayer = base.GetComponent<VideoPlayer>();
		if (this.videoPlayer == null)
		{
			this.videoPlayer = base.gameObject.AddComponent<VideoPlayer>();
		}
		this.videoPlayer.isLooping = this.LoopVideo;
		this.overlay = base.GetComponent<OVROverlay>();
		if (this.overlay == null)
		{
			this.overlay = base.gameObject.AddComponent<OVROverlay>();
		}
		this.overlay.enabled = false;
		this.overlay.isExternalSurface = NativeVideoPlayer.IsAvailable;
		this.overlay.enabled = (this.overlay.currentOverlayShape != OVROverlay.OverlayShape.Equirect || Application.platform == RuntimePlatform.Android);
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x00056173 File Offset: 0x00054373
	private bool IsLocalVideo(string movieName)
	{
		return !movieName.Contains("://");
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x00056184 File Offset: 0x00054384
	private void UpdateShapeAndStereo()
	{
		if (this.AutoDetectStereoLayout && this.overlay.isExternalSurface)
		{
			int videoWidth = NativeVideoPlayer.VideoWidth;
			int videoHeight = NativeVideoPlayer.VideoHeight;
			switch (NativeVideoPlayer.VideoStereoMode)
			{
			case NativeVideoPlayer.StereoMode.Unknown:
				if (videoWidth > videoHeight)
				{
					this.Stereo = MoviePlayerSample.VideoStereo.LeftRight;
				}
				else
				{
					this.Stereo = MoviePlayerSample.VideoStereo.TopBottom;
				}
				break;
			case NativeVideoPlayer.StereoMode.Mono:
				this.Stereo = MoviePlayerSample.VideoStereo.Mono;
				break;
			case NativeVideoPlayer.StereoMode.TopBottom:
				this.Stereo = MoviePlayerSample.VideoStereo.TopBottom;
				break;
			case NativeVideoPlayer.StereoMode.LeftRight:
				this.Stereo = MoviePlayerSample.VideoStereo.LeftRight;
				break;
			}
		}
		if (this.Shape != this._LastShape || this.Stereo != this._LastStereo || this.DisplayMono != this._LastDisplayMono)
		{
			Rect rect = new Rect(0f, 0f, 1f, 1f);
			switch (this.Shape)
			{
			case MoviePlayerSample.VideoShape._360:
				this.overlay.currentOverlayShape = OVROverlay.OverlayShape.Equirect;
				goto IL_118;
			case MoviePlayerSample.VideoShape._180:
				this.overlay.currentOverlayShape = OVROverlay.OverlayShape.Equirect;
				rect = new Rect(0.25f, 0f, 0.5f, 1f);
				goto IL_118;
			}
			this.overlay.currentOverlayShape = OVROverlay.OverlayShape.Quad;
			IL_118:
			this.overlay.overrideTextureRectMatrix = true;
			this.overlay.invertTextureRects = false;
			Rect rect2 = new Rect(0f, 0f, 1f, 1f);
			Rect rect3 = new Rect(0f, 0f, 1f, 1f);
			switch (this.Stereo)
			{
			case MoviePlayerSample.VideoStereo.TopBottom:
				rect2 = new Rect(0f, 0.5f, 1f, 0.5f);
				rect3 = new Rect(0f, 0f, 1f, 0.5f);
				break;
			case MoviePlayerSample.VideoStereo.LeftRight:
				rect2 = new Rect(0f, 0f, 0.5f, 1f);
				rect3 = new Rect(0.5f, 0f, 0.5f, 1f);
				break;
			case MoviePlayerSample.VideoStereo.BottomTop:
				rect2 = new Rect(0f, 0f, 1f, 0.5f);
				rect3 = new Rect(0f, 0.5f, 1f, 0.5f);
				break;
			}
			this.overlay.SetSrcDestRects(rect2, this.DisplayMono ? rect2 : rect3, rect, rect);
			this._LastDisplayMono = this.DisplayMono;
			this._LastStereo = this.Stereo;
			this._LastShape = this.Shape;
		}
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x00056400 File Offset: 0x00054600
	private IEnumerator Start()
	{
		if (this.mediaRenderer.material == null)
		{
			Debug.LogError("No material for movie surface");
			yield break;
		}
		yield return new WaitForSeconds(1f);
		if (!string.IsNullOrEmpty(this.MovieName))
		{
			if (this.IsLocalVideo(this.MovieName))
			{
				this.Play(Application.streamingAssetsPath + "/" + this.MovieName, null);
			}
			else
			{
				this.Play(this.MovieName, this.DrmLicenseUrl);
			}
		}
		yield break;
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x00056410 File Offset: 0x00054610
	public void Play(string moviePath, string drmLicencesUrl)
	{
		if (moviePath != string.Empty)
		{
			Debug.Log("Playing Video: " + moviePath);
			if (this.overlay.isExternalSurface)
			{
				OVROverlay.ExternalSurfaceObjectCreated externalSurfaceObjectCreated = delegate()
				{
					Debug.Log("Playing ExoPlayer with SurfaceObject");
					NativeVideoPlayer.PlayVideo(moviePath, drmLicencesUrl, this.overlay.externalSurfaceObject);
					NativeVideoPlayer.SetLooping(this.LoopVideo);
				};
				if (this.overlay.externalSurfaceObject == IntPtr.Zero)
				{
					this.overlay.externalSurfaceObjectCreated = externalSurfaceObjectCreated;
				}
				else
				{
					externalSurfaceObjectCreated();
				}
			}
			else
			{
				Debug.Log("Playing Unity VideoPlayer");
				this.videoPlayer.url = moviePath;
				this.videoPlayer.Prepare();
				this.videoPlayer.Play();
			}
			Debug.Log("MovieSample Start");
			this.IsPlaying = true;
			return;
		}
		Debug.LogError("No media file name provided");
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x000564F6 File Offset: 0x000546F6
	public void Play()
	{
		if (this.overlay.isExternalSurface)
		{
			NativeVideoPlayer.Play();
		}
		else
		{
			this.videoPlayer.Play();
		}
		this.IsPlaying = true;
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x0005651E File Offset: 0x0005471E
	public void Pause()
	{
		if (this.overlay.isExternalSurface)
		{
			NativeVideoPlayer.Pause();
		}
		else
		{
			this.videoPlayer.Pause();
		}
		this.IsPlaying = false;
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x00056548 File Offset: 0x00054748
	public void SeekTo(long position)
	{
		long num = Math.Max(0L, Math.Min(this.Duration, position));
		if (this.overlay.isExternalSurface)
		{
			NativeVideoPlayer.PlaybackPosition = num;
			return;
		}
		this.videoPlayer.time = (double)num / 1000.0;
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x00056594 File Offset: 0x00054794
	private void Update()
	{
		this.UpdateShapeAndStereo();
		if (!this.overlay.isExternalSurface)
		{
			Texture texture = (this.videoPlayer.texture != null) ? this.videoPlayer.texture : Texture2D.blackTexture;
			if (this.overlay.enabled)
			{
				if (this.overlay.textures[0] != texture)
				{
					this.overlay.enabled = false;
					this.overlay.textures[0] = texture;
					this.overlay.enabled = true;
				}
			}
			else
			{
				this.mediaRenderer.material.mainTexture = texture;
				this.mediaRenderer.material.SetVector("_SrcRectLeft", this.overlay.srcRectLeft.ToVector());
				this.mediaRenderer.material.SetVector("_SrcRectRight", this.overlay.srcRectRight.ToVector());
			}
			this.IsPlaying = this.videoPlayer.isPlaying;
			this.PlaybackPosition = (long)(this.videoPlayer.time * 1000.0);
			this.Duration = (long)(this.videoPlayer.length * 1000.0);
			return;
		}
		NativeVideoPlayer.SetListenerRotation(Camera.main.transform.rotation);
		this.IsPlaying = NativeVideoPlayer.IsPlaying;
		this.PlaybackPosition = NativeVideoPlayer.PlaybackPosition;
		this.Duration = NativeVideoPlayer.Duration;
		if (this.IsPlaying && (int)OVRManager.display.displayFrequency != 60)
		{
			OVRManager.display.displayFrequency = 60f;
			return;
		}
		if (!this.IsPlaying && (int)OVRManager.display.displayFrequency != 72)
		{
			OVRManager.display.displayFrequency = 72f;
		}
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x00056751 File Offset: 0x00054951
	public void SetPlaybackSpeed(float speed)
	{
		speed = Mathf.Max(0f, speed);
		if (this.overlay.isExternalSurface)
		{
			NativeVideoPlayer.SetPlaybackSpeed(speed);
			return;
		}
		this.videoPlayer.playbackSpeed = speed;
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x00056780 File Offset: 0x00054980
	public void Stop()
	{
		if (this.overlay.isExternalSurface)
		{
			NativeVideoPlayer.Stop();
		}
		else
		{
			this.videoPlayer.Stop();
		}
		this.IsPlaying = false;
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x000567A8 File Offset: 0x000549A8
	private void OnApplicationPause(bool appWasPaused)
	{
		Debug.Log("OnApplicationPause: " + appWasPaused.ToString());
		if (appWasPaused)
		{
			this.videoPausedBeforeAppPause = !this.IsPlaying;
		}
		if (!this.videoPausedBeforeAppPause)
		{
			if (appWasPaused)
			{
				this.Pause();
				return;
			}
			this.Play();
		}
	}

	// Token: 0x040013F6 RID: 5110
	private bool videoPausedBeforeAppPause;

	// Token: 0x040013F7 RID: 5111
	private VideoPlayer videoPlayer;

	// Token: 0x040013F8 RID: 5112
	private OVROverlay overlay;

	// Token: 0x040013F9 RID: 5113
	private Renderer mediaRenderer;

	// Token: 0x040013FD RID: 5117
	private RenderTexture copyTexture;

	// Token: 0x040013FE RID: 5118
	private Material externalTex2DMaterial;

	// Token: 0x040013FF RID: 5119
	public string MovieName;

	// Token: 0x04001400 RID: 5120
	public string DrmLicenseUrl;

	// Token: 0x04001401 RID: 5121
	public bool LoopVideo;

	// Token: 0x04001402 RID: 5122
	public MoviePlayerSample.VideoShape Shape;

	// Token: 0x04001403 RID: 5123
	public MoviePlayerSample.VideoStereo Stereo;

	// Token: 0x04001404 RID: 5124
	public bool AutoDetectStereoLayout;

	// Token: 0x04001405 RID: 5125
	public bool DisplayMono;

	// Token: 0x04001406 RID: 5126
	private MoviePlayerSample.VideoShape _LastShape = (MoviePlayerSample.VideoShape)(-1);

	// Token: 0x04001407 RID: 5127
	private MoviePlayerSample.VideoStereo _LastStereo = (MoviePlayerSample.VideoStereo)(-1);

	// Token: 0x04001408 RID: 5128
	private bool _LastDisplayMono;

	// Token: 0x020002F5 RID: 757
	public enum VideoShape
	{
		// Token: 0x0400140A RID: 5130
		_360,
		// Token: 0x0400140B RID: 5131
		_180,
		// Token: 0x0400140C RID: 5132
		Quad
	}

	// Token: 0x020002F6 RID: 758
	public enum VideoStereo
	{
		// Token: 0x0400140E RID: 5134
		Mono,
		// Token: 0x0400140F RID: 5135
		TopBottom,
		// Token: 0x04001410 RID: 5136
		LeftRight,
		// Token: 0x04001411 RID: 5137
		BottomTop
	}
}
