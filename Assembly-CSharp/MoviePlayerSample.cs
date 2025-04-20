using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x020002FF RID: 767
public class MoviePlayerSample : MonoBehaviour
{
	// Token: 0x17000219 RID: 537
	// (get) Token: 0x0600125F RID: 4703 RVA: 0x0003C9B2 File Offset: 0x0003ABB2
	// (set) Token: 0x06001260 RID: 4704 RVA: 0x0003C9BA File Offset: 0x0003ABBA
	public bool IsPlaying { get; private set; }

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06001261 RID: 4705 RVA: 0x0003C9C3 File Offset: 0x0003ABC3
	// (set) Token: 0x06001262 RID: 4706 RVA: 0x0003C9CB File Offset: 0x0003ABCB
	public long Duration { get; private set; }

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06001263 RID: 4707 RVA: 0x0003C9D4 File Offset: 0x0003ABD4
	// (set) Token: 0x06001264 RID: 4708 RVA: 0x0003C9DC File Offset: 0x0003ABDC
	public long PlaybackPosition { get; private set; }

	// Token: 0x06001265 RID: 4709 RVA: 0x000B07E8 File Offset: 0x000AE9E8
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

	// Token: 0x06001266 RID: 4710 RVA: 0x0003C9E5 File Offset: 0x0003ABE5
	private bool IsLocalVideo(string movieName)
	{
		return !movieName.Contains("://");
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x000B08B4 File Offset: 0x000AEAB4
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

	// Token: 0x06001268 RID: 4712 RVA: 0x0003C9F5 File Offset: 0x0003ABF5
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

	// Token: 0x06001269 RID: 4713 RVA: 0x000B0B30 File Offset: 0x000AED30
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

	// Token: 0x0600126A RID: 4714 RVA: 0x0003CA04 File Offset: 0x0003AC04
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

	// Token: 0x0600126B RID: 4715 RVA: 0x0003CA2C File Offset: 0x0003AC2C
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

	// Token: 0x0600126C RID: 4716 RVA: 0x000B0C18 File Offset: 0x000AEE18
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

	// Token: 0x0600126D RID: 4717 RVA: 0x000B0C64 File Offset: 0x000AEE64
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

	// Token: 0x0600126E RID: 4718 RVA: 0x0003CA54 File Offset: 0x0003AC54
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

	// Token: 0x0600126F RID: 4719 RVA: 0x0003CA83 File Offset: 0x0003AC83
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

	// Token: 0x06001270 RID: 4720 RVA: 0x000B0E24 File Offset: 0x000AF024
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

	// Token: 0x0400143E RID: 5182
	private bool videoPausedBeforeAppPause;

	// Token: 0x0400143F RID: 5183
	private VideoPlayer videoPlayer;

	// Token: 0x04001440 RID: 5184
	private OVROverlay overlay;

	// Token: 0x04001441 RID: 5185
	private Renderer mediaRenderer;

	// Token: 0x04001445 RID: 5189
	private RenderTexture copyTexture;

	// Token: 0x04001446 RID: 5190
	private Material externalTex2DMaterial;

	// Token: 0x04001447 RID: 5191
	public string MovieName;

	// Token: 0x04001448 RID: 5192
	public string DrmLicenseUrl;

	// Token: 0x04001449 RID: 5193
	public bool LoopVideo;

	// Token: 0x0400144A RID: 5194
	public MoviePlayerSample.VideoShape Shape;

	// Token: 0x0400144B RID: 5195
	public MoviePlayerSample.VideoStereo Stereo;

	// Token: 0x0400144C RID: 5196
	public bool AutoDetectStereoLayout;

	// Token: 0x0400144D RID: 5197
	public bool DisplayMono;

	// Token: 0x0400144E RID: 5198
	private MoviePlayerSample.VideoShape _LastShape = (MoviePlayerSample.VideoShape)(-1);

	// Token: 0x0400144F RID: 5199
	private MoviePlayerSample.VideoStereo _LastStereo = (MoviePlayerSample.VideoStereo)(-1);

	// Token: 0x04001450 RID: 5200
	private bool _LastDisplayMono;

	// Token: 0x02000300 RID: 768
	public enum VideoShape
	{
		// Token: 0x04001452 RID: 5202
		_360,
		// Token: 0x04001453 RID: 5203
		_180,
		// Token: 0x04001454 RID: 5204
		Quad
	}

	// Token: 0x02000301 RID: 769
	public enum VideoStereo
	{
		// Token: 0x04001456 RID: 5206
		Mono,
		// Token: 0x04001457 RID: 5207
		TopBottom,
		// Token: 0x04001458 RID: 5208
		LeftRight,
		// Token: 0x04001459 RID: 5209
		BottomTop
	}
}
