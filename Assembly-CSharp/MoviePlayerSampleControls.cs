using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000304 RID: 772
public class MoviePlayerSampleControls : MonoBehaviour
{
	// Token: 0x0600127A RID: 4730 RVA: 0x000B0F2C File Offset: 0x000AF12C
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

	// Token: 0x0600127B RID: 4731 RVA: 0x000B0FC8 File Offset: 0x000AF1C8
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

	// Token: 0x0600127C RID: 4732 RVA: 0x000B10A4 File Offset: 0x000AF2A4
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

	// Token: 0x0600127D RID: 4733 RVA: 0x000B1154 File Offset: 0x000AF354
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

	// Token: 0x0600127E RID: 4734 RVA: 0x000B1204 File Offset: 0x000AF404
	private void OnSeekBarMoved(float value)
	{
		long num = (long)(value * (float)this.Player.Duration);
		if (Mathf.Abs((float)(num - this.Player.PlaybackPosition)) > 200f)
		{
			this.Seek(num);
		}
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x0003CB15 File Offset: 0x0003AD15
	private void Seek(long pos)
	{
		this._didSeek = true;
		this._seekPreviousPosition = this.Player.PlaybackPosition;
		this.Player.SeekTo(pos);
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x000B1244 File Offset: 0x000AF444
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

	// Token: 0x06001281 RID: 4737 RVA: 0x000B1404 File Offset: 0x000AF604
	private void SetVisible(bool visible)
	{
		this.Canvas.enabled = visible;
		this._isVisible = visible;
		this.Player.DisplayMono = visible;
		this.LeftHand.SetActive(visible);
		this.RightHand.SetActive(visible);
		Debug.Log("Controls Visible: " + visible.ToString());
	}

	// Token: 0x04001460 RID: 5216
	public MoviePlayerSample Player;

	// Token: 0x04001461 RID: 5217
	public OVRInputModule InputModule;

	// Token: 0x04001462 RID: 5218
	public OVRGazePointer GazePointer;

	// Token: 0x04001463 RID: 5219
	public GameObject LeftHand;

	// Token: 0x04001464 RID: 5220
	public GameObject RightHand;

	// Token: 0x04001465 RID: 5221
	public Canvas Canvas;

	// Token: 0x04001466 RID: 5222
	public ButtonDownListener PlayPause;

	// Token: 0x04001467 RID: 5223
	public MediaPlayerImage PlayPauseImage;

	// Token: 0x04001468 RID: 5224
	public Slider ProgressBar;

	// Token: 0x04001469 RID: 5225
	public ButtonDownListener FastForward;

	// Token: 0x0400146A RID: 5226
	public MediaPlayerImage FastForwardImage;

	// Token: 0x0400146B RID: 5227
	public ButtonDownListener Rewind;

	// Token: 0x0400146C RID: 5228
	public MediaPlayerImage RewindImage;

	// Token: 0x0400146D RID: 5229
	public float TimeoutTime = 10f;

	// Token: 0x0400146E RID: 5230
	private bool _isVisible;

	// Token: 0x0400146F RID: 5231
	private float _lastButtonTime;

	// Token: 0x04001470 RID: 5232
	private bool _didSeek;

	// Token: 0x04001471 RID: 5233
	private long _seekPreviousPosition;

	// Token: 0x04001472 RID: 5234
	private long _rewindStartPosition;

	// Token: 0x04001473 RID: 5235
	private float _rewindStartTime;

	// Token: 0x04001474 RID: 5236
	private MoviePlayerSampleControls.PlaybackState _state;

	// Token: 0x02000305 RID: 773
	private enum PlaybackState
	{
		// Token: 0x04001476 RID: 5238
		Playing,
		// Token: 0x04001477 RID: 5239
		Paused,
		// Token: 0x04001478 RID: 5240
		Rewinding,
		// Token: 0x04001479 RID: 5241
		FastForwarding
	}
}
