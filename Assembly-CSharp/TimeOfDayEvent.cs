using System;
using UnityEngine;

// Token: 0x020008CB RID: 2251
public class TimeOfDayEvent : TimeEvent
{
	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06003696 RID: 13974 RVA: 0x00053FFE File Offset: 0x000521FE
	public float currentTime
	{
		get
		{
			return this._currentTime;
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06003697 RID: 13975 RVA: 0x00054006 File Offset: 0x00052206
	// (set) Token: 0x06003698 RID: 13976 RVA: 0x0005400E File Offset: 0x0005220E
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

	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06003699 RID: 13977 RVA: 0x0005401C File Offset: 0x0005221C
	// (set) Token: 0x0600369A RID: 13978 RVA: 0x00054024 File Offset: 0x00052224
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

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x0600369B RID: 13979 RVA: 0x00054032 File Offset: 0x00052232
	public bool isOngoing
	{
		get
		{
			return this._ongoing;
		}
	}

	// Token: 0x0600369C RID: 13980 RVA: 0x00144DE8 File Offset: 0x00142FE8
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

	// Token: 0x0600369D RID: 13981 RVA: 0x0005403A File Offset: 0x0005223A
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

	// Token: 0x0600369E RID: 13982 RVA: 0x00144E6C File Offset: 0x0014306C
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

	// Token: 0x0600369F RID: 13983 RVA: 0x0005406D File Offset: 0x0005226D
	public static implicit operator bool(TimeOfDayEvent ev)
	{
		return ev && ev.isOngoing;
	}

	// Token: 0x040038C3 RID: 14531
	[SerializeField]
	[Range(0f, 1f)]
	private float _timeStart;

	// Token: 0x040038C4 RID: 14532
	[SerializeField]
	[Range(0f, 1f)]
	private float _timeEnd = 1f;

	// Token: 0x040038C5 RID: 14533
	[SerializeField]
	private float _currentTime = -1f;

	// Token: 0x040038C6 RID: 14534
	[Space]
	[SerializeField]
	private double _currentSeconds = -1.0;

	// Token: 0x040038C7 RID: 14535
	[SerializeField]
	private double _totalSecondsInRange = -1.0;

	// Token: 0x040038C8 RID: 14536
	[NonSerialized]
	private float _elapsed = -1f;

	// Token: 0x040038C9 RID: 14537
	[SerializeField]
	private BetterDayNightManager _dayNightManager;
}
