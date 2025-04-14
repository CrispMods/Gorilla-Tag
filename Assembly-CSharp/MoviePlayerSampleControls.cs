using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020002F9 RID: 761
public class MoviePlayerSampleControls : MonoBehaviour
{
	// Token: 0x0600122E RID: 4654 RVA: 0x00056918 File Offset: 0x00054B18
	private void Start()
	{
		this.PlayPause.onButtonDown += this.OnPlayPauseClicked;
		this.FastForward.onButtonDown += this.OnFastForwardClicked;
		this.Rewind.onButtonDown += this.OnRewindClicked;
		this.ProgressBar.onValueChanged.AddListener(new UnityAction<float>(this.OnSeekBarMoved));
		this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Pause;
		this.FastForwardImage.buttonType = MediaPlayerImage.ButtonType.SkipForward;
		this.RewindImage.buttonType = MediaPlayerImage.ButtonType.SkipBack;
		this.SetVisible(false);
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x000569B4 File Offset: 0x00054BB4
	private void OnPlayPauseClicked()
	{
		switch (this._state)
		{
		case MoviePlayerSampleControls.PlaybackState.Playing:
			this.Player.Pause();
			this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Play;
			this.FastForwardImage.buttonType = MediaPlayerImage.ButtonType.SkipForward;
			this.RewindImage.buttonType = MediaPlayerImage.ButtonType.SkipBack;
			this._state = MoviePlayerSampleControls.PlaybackState.Paused;
			return;
		case MoviePlayerSampleControls.PlaybackState.Paused:
			this.Player.Play();
			this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Pause;
			this.FastForwardImage.buttonType = MediaPlayerImage.ButtonType.FastForward;
			this.RewindImage.buttonType = MediaPlayerImage.ButtonType.Rewind;
			this._state = MoviePlayerSampleControls.PlaybackState.Playing;
			return;
		case MoviePlayerSampleControls.PlaybackState.Rewinding:
			this.Player.Play();
			this._state = MoviePlayerSampleControls.PlaybackState.Playing;
			this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Pause;
			return;
		case MoviePlayerSampleControls.PlaybackState.FastForwarding:
			this.Player.SetPlaybackSpeed(1f);
			this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Pause;
			this._state = MoviePlayerSampleControls.PlaybackState.Playing;
			return;
		default:
			return;
		}
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x00056A90 File Offset: 0x00054C90
	private void OnFastForwardClicked()
	{
		switch (this._state)
		{
		case MoviePlayerSampleControls.PlaybackState.Playing:
			this.Player.SetPlaybackSpeed(2f);
			this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Play;
			this._state = MoviePlayerSampleControls.PlaybackState.FastForwarding;
			return;
		case MoviePlayerSampleControls.PlaybackState.Paused:
			this.Seek(this.Player.PlaybackPosition + 15000L);
			return;
		case MoviePlayerSampleControls.PlaybackState.Rewinding:
			this.Player.Play();
			this.Player.SetPlaybackSpeed(2f);
			this._state = MoviePlayerSampleControls.PlaybackState.FastForwarding;
			return;
		case MoviePlayerSampleControls.PlaybackState.FastForwarding:
			this.Player.SetPlaybackSpeed(1f);
			this._state = MoviePlayerSampleControls.PlaybackState.Playing;
			this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Pause;
			return;
		default:
			return;
		}
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00056B40 File Offset: 0x00054D40
	private void OnRewindClicked()
	{
		switch (this._state)
		{
		case MoviePlayerSampleControls.PlaybackState.Playing:
		case MoviePlayerSampleControls.PlaybackState.FastForwarding:
			this.Player.SetPlaybackSpeed(1f);
			this.Player.Pause();
			this._rewindStartPosition = this.Player.PlaybackPosition;
			this._rewindStartTime = Time.time;
			this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Play;
			this._state = MoviePlayerSampleControls.PlaybackState.Rewinding;
			return;
		case MoviePlayerSampleControls.PlaybackState.Paused:
			this.Seek(this.Player.PlaybackPosition - 15000L);
			return;
		case MoviePlayerSampleControls.PlaybackState.Rewinding:
			this.Player.Play();
			this.PlayPauseImage.buttonType = MediaPlayerImage.ButtonType.Pause;
			this._state = MoviePlayerSampleControls.PlaybackState.Playing;
			return;
		default:
			return;
		}
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00056BF0 File Offset: 0x00054DF0
	private void OnSeekBarMoved(float value)
	{
		long num = (long)(value * (float)this.Player.Duration);
		if (Mathf.Abs((float)(num - this.Player.PlaybackPosition)) > 200f)
		{
			this.Seek(num);
		}
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x00056C2E File Offset: 0x00054E2E
	private void Seek(long pos)
	{
		this._didSeek = true;
		this._seekPreviousPosition = this.Player.PlaybackPosition;
		this.Player.SeekTo(pos);
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x00056C54 File Offset: 0x00054E54
	private void Update()
	{
		if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Active) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Active) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Active))
		{
			this._lastButtonTime = Time.time;
			if (!this._isVisible)
			{
				this.SetVisible(true);
			}
		}
		if (OVRInput.GetActiveController() == OVRInput.Controller.LTouch)
		{
			this.InputModule.rayTransform = this.LeftHand.transform;
			this.GazePointer.rayTransform = this.LeftHand.transform;
		}
		else
		{
			this.InputModule.rayTransform = this.RightHand.transform;
			this.GazePointer.rayTransform = this.RightHand.transform;
		}
		if (OVRInput.Get(OVRInput.Button.Back, OVRInput.Controller.Active) && this._isVisible)
		{
			this.SetVisible(false);
		}
		if (this._state == MoviePlayerSampleControls.PlaybackState.Rewinding)
		{
			this.ProgressBar.value = Mathf.Clamp01(((float)this._rewindStartPosition - 1000f * (Time.time - this._rewindStartTime)) / (float)this.Player.Duration);
		}
		if (this._isVisible && this._state == MoviePlayerSampleControls.PlaybackState.Playing && Time.time - this._lastButtonTime > this.TimeoutTime)
		{
			this.SetVisible(false);
		}
		if (this._isVisible && (!this._didSeek || Mathf.Abs((float)(this._seekPreviousPosition - this.Player.PlaybackPosition)) > 50f))
		{
			this._didSeek = false;
			if (this.Player.Duration > 0L)
			{
				this.ProgressBar.value = (float)((double)this.Player.PlaybackPosition / (double)this.Player.Duration);
				return;
			}
			this.ProgressBar.value = 0f;
		}
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x00056E14 File Offset: 0x00055014
	private void SetVisible(bool visible)
	{
		this.Canvas.enabled = visible;
		this._isVisible = visible;
		this.Player.DisplayMono = visible;
		this.LeftHand.SetActive(visible);
		this.RightHand.SetActive(visible);
		Debug.Log("Controls Visible: " + visible.ToString());
	}

	// Token: 0x04001418 RID: 5144
	public MoviePlayerSample Player;

	// Token: 0x04001419 RID: 5145
	public OVRInputModule InputModule;

	// Token: 0x0400141A RID: 5146
	public OVRGazePointer GazePointer;

	// Token: 0x0400141B RID: 5147
	public GameObject LeftHand;

	// Token: 0x0400141C RID: 5148
	public GameObject RightHand;

	// Token: 0x0400141D RID: 5149
	public Canvas Canvas;

	// Token: 0x0400141E RID: 5150
	public ButtonDownListener PlayPause;

	// Token: 0x0400141F RID: 5151
	public MediaPlayerImage PlayPauseImage;

	// Token: 0x04001420 RID: 5152
	public Slider ProgressBar;

	// Token: 0x04001421 RID: 5153
	public ButtonDownListener FastForward;

	// Token: 0x04001422 RID: 5154
	public MediaPlayerImage FastForwardImage;

	// Token: 0x04001423 RID: 5155
	public ButtonDownListener Rewind;

	// Token: 0x04001424 RID: 5156
	public MediaPlayerImage RewindImage;

	// Token: 0x04001425 RID: 5157
	public float TimeoutTime = 10f;

	// Token: 0x04001426 RID: 5158
	private bool _isVisible;

	// Token: 0x04001427 RID: 5159
	private float _lastButtonTime;

	// Token: 0x04001428 RID: 5160
	private bool _didSeek;

	// Token: 0x04001429 RID: 5161
	private long _seekPreviousPosition;

	// Token: 0x0400142A RID: 5162
	private long _rewindStartPosition;

	// Token: 0x0400142B RID: 5163
	private float _rewindStartTime;

	// Token: 0x0400142C RID: 5164
	private MoviePlayerSampleControls.PlaybackState _state;

	// Token: 0x020002FA RID: 762
	private enum PlaybackState
	{
		// Token: 0x0400142E RID: 5166
		Playing,
		// Token: 0x0400142F RID: 5167
		Paused,
		// Token: 0x04001430 RID: 5168
		Rewinding,
		// Token: 0x04001431 RID: 5169
		FastForwarding
	}
}
