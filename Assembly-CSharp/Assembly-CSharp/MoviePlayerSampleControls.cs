using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020002F9 RID: 761
public class MoviePlayerSampleControls : MonoBehaviour
{
	// Token: 0x06001231 RID: 4657 RVA: 0x00056C9C File Offset: 0x00054E9C
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

	// Token: 0x06001232 RID: 4658 RVA: 0x00056D38 File Offset: 0x00054F38
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

	// Token: 0x06001233 RID: 4659 RVA: 0x00056E14 File Offset: 0x00055014
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

	// Token: 0x06001234 RID: 4660 RVA: 0x00056EC4 File Offset: 0x000550C4
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

	// Token: 0x06001235 RID: 4661 RVA: 0x00056F74 File Offset: 0x00055174
	private void OnSeekBarMoved(float value)
	{
		long num = (long)(value * (float)this.Player.Duration);
		if (Mathf.Abs((float)(num - this.Player.PlaybackPosition)) > 200f)
		{
			this.Seek(num);
		}
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x00056FB2 File Offset: 0x000551B2
	private void Seek(long pos)
	{
		this._didSeek = true;
		this._seekPreviousPosition = this.Player.PlaybackPosition;
		this.Player.SeekTo(pos);
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x00056FD8 File Offset: 0x000551D8
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

	// Token: 0x06001238 RID: 4664 RVA: 0x00057198 File Offset: 0x00055398
	private void SetVisible(bool visible)
	{
		this.Canvas.enabled = visible;
		this._isVisible = visible;
		this.Player.DisplayMono = visible;
		this.LeftHand.SetActive(visible);
		this.RightHand.SetActive(visible);
		Debug.Log("Controls Visible: " + visible.ToString());
	}

	// Token: 0x04001419 RID: 5145
	public MoviePlayerSample Player;

	// Token: 0x0400141A RID: 5146
	public OVRInputModule InputModule;

	// Token: 0x0400141B RID: 5147
	public OVRGazePointer GazePointer;

	// Token: 0x0400141C RID: 5148
	public GameObject LeftHand;

	// Token: 0x0400141D RID: 5149
	public GameObject RightHand;

	// Token: 0x0400141E RID: 5150
	public Canvas Canvas;

	// Token: 0x0400141F RID: 5151
	public ButtonDownListener PlayPause;

	// Token: 0x04001420 RID: 5152
	public MediaPlayerImage PlayPauseImage;

	// Token: 0x04001421 RID: 5153
	public Slider ProgressBar;

	// Token: 0x04001422 RID: 5154
	public ButtonDownListener FastForward;

	// Token: 0x04001423 RID: 5155
	public MediaPlayerImage FastForwardImage;

	// Token: 0x04001424 RID: 5156
	public ButtonDownListener Rewind;

	// Token: 0x04001425 RID: 5157
	public MediaPlayerImage RewindImage;

	// Token: 0x04001426 RID: 5158
	public float TimeoutTime = 10f;

	// Token: 0x04001427 RID: 5159
	private bool _isVisible;

	// Token: 0x04001428 RID: 5160
	private float _lastButtonTime;

	// Token: 0x04001429 RID: 5161
	private bool _didSeek;

	// Token: 0x0400142A RID: 5162
	private long _seekPreviousPosition;

	// Token: 0x0400142B RID: 5163
	private long _rewindStartPosition;

	// Token: 0x0400142C RID: 5164
	private float _rewindStartTime;

	// Token: 0x0400142D RID: 5165
	private MoviePlayerSampleControls.PlaybackState _state;

	// Token: 0x020002FA RID: 762
	private enum PlaybackState
	{
		// Token: 0x0400142F RID: 5167
		Playing,
		// Token: 0x04001430 RID: 5168
		Paused,
		// Token: 0x04001431 RID: 5169
		Rewinding,
		// Token: 0x04001432 RID: 5170
		FastForwarding
	}
}
