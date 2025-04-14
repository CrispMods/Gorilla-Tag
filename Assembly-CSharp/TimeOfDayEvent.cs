using System;
using UnityEngine;

// Token: 0x020008AF RID: 2223
public class TimeOfDayEvent : TimeEvent
{
	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x060035CE RID: 13774 RVA: 0x000FF099 File Offset: 0x000FD299
	public float currentTime
	{
		get
		{
			return this._currentTime;
		}
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x060035CF RID: 13775 RVA: 0x000FF0A1 File Offset: 0x000FD2A1
	// (set) Token: 0x060035D0 RID: 13776 RVA: 0x000FF0A9 File Offset: 0x000FD2A9
	public float timeStart
	{
		get
		{
			return this._timeStart;
		}
		set
		{
			this._timeStart = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x060035D1 RID: 13777 RVA: 0x000FF0B7 File Offset: 0x000FD2B7
	// (set) Token: 0x060035D2 RID: 13778 RVA: 0x000FF0BF File Offset: 0x000FD2BF
	public float timeEnd
	{
		get
		{
			return this._timeEnd;
		}
		set
		{
			this._timeEnd = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x060035D3 RID: 13779 RVA: 0x000FF0CD File Offset: 0x000FD2CD
	public bool isOngoing
	{
		get
		{
			return this._ongoing;
		}
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x000FF0D8 File Offset: 0x000FD2D8
	private void Start()
	{
		if (!this._dayNightManager)
		{
			this._dayNightManager = BetterDayNightManager.instance;
		}
		if (!this._dayNightManager)
		{
			return;
		}
		for (int i = 0; i < this._dayNightManager.timeOfDayRange.Length; i++)
		{
			this._totalSecondsInRange += this._dayNightManager.timeOfDayRange[i] * 3600.0;
		}
		this._totalSecondsInRange = Math.Floor(this._totalSecondsInRange);
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x000FF15A File Offset: 0x000FD35A
	private void Update()
	{
		this._elapsed += Time.deltaTime;
		if (this._elapsed < 1f)
		{
			return;
		}
		this._elapsed = 0f;
		this.UpdateTime();
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x000FF190 File Offset: 0x000FD390
	private void UpdateTime()
	{
		this._currentSeconds = ((ITimeOfDaySystem)this._dayNightManager).currentTimeInSeconds;
		this._currentSeconds = Math.Floor(this._currentSeconds);
		this._currentTime = (float)(this._currentSeconds / this._totalSecondsInRange);
		bool flag = this._currentTime >= 0f && this._currentTime >= this._timeStart && this._currentTime <= this._timeEnd;
		if (!this._ongoing && flag)
		{
			base.StartEvent();
		}
		if (this._ongoing && !flag)
		{
			base.StopEvent();
		}
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x000FF227 File Offset: 0x000FD427
	public static implicit operator bool(TimeOfDayEvent ev)
	{
		return ev && ev.isOngoing;
	}

	// Token: 0x04003802 RID: 14338
	[SerializeField]
	[Range(0f, 1f)]
	private float _timeStart;

	// Token: 0x04003803 RID: 14339
	[SerializeField]
	[Range(0f, 1f)]
	private float _timeEnd = 1f;

	// Token: 0x04003804 RID: 14340
	[SerializeField]
	private float _currentTime = -1f;

	// Token: 0x04003805 RID: 14341
	[Space]
	[SerializeField]
	private double _currentSeconds = -1.0;

	// Token: 0x04003806 RID: 14342
	[SerializeField]
	private double _totalSecondsInRange = -1.0;

	// Token: 0x04003807 RID: 14343
	[NonSerialized]
	private float _elapsed = -1f;

	// Token: 0x04003808 RID: 14344
	[SerializeField]
	private BetterDayNightManager _dayNightManager;
}
