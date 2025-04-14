using System;
using UnityEngine;

// Token: 0x020008B2 RID: 2226
public class TimeOfDayEvent : TimeEvent
{
	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x060035DA RID: 13786 RVA: 0x000FF661 File Offset: 0x000FD861
	public float currentTime
	{
		get
		{
			return this._currentTime;
		}
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x060035DB RID: 13787 RVA: 0x000FF669 File Offset: 0x000FD869
	// (set) Token: 0x060035DC RID: 13788 RVA: 0x000FF671 File Offset: 0x000FD871
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

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x060035DD RID: 13789 RVA: 0x000FF67F File Offset: 0x000FD87F
	// (set) Token: 0x060035DE RID: 13790 RVA: 0x000FF687 File Offset: 0x000FD887
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

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x060035DF RID: 13791 RVA: 0x000FF695 File Offset: 0x000FD895
	public bool isOngoing
	{
		get
		{
			return this._ongoing;
		}
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x000FF6A0 File Offset: 0x000FD8A0
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

	// Token: 0x060035E1 RID: 13793 RVA: 0x000FF722 File Offset: 0x000FD922
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

	// Token: 0x060035E2 RID: 13794 RVA: 0x000FF758 File Offset: 0x000FD958
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

	// Token: 0x060035E3 RID: 13795 RVA: 0x000FF7EF File Offset: 0x000FD9EF
	public static implicit operator bool(TimeOfDayEvent ev)
	{
		return ev && ev.isOngoing;
	}

	// Token: 0x04003814 RID: 14356
	[SerializeField]
	[Range(0f, 1f)]
	private float _timeStart;

	// Token: 0x04003815 RID: 14357
	[SerializeField]
	[Range(0f, 1f)]
	private float _timeEnd = 1f;

	// Token: 0x04003816 RID: 14358
	[SerializeField]
	private float _currentTime = -1f;

	// Token: 0x04003817 RID: 14359
	[Space]
	[SerializeField]
	private double _currentSeconds = -1.0;

	// Token: 0x04003818 RID: 14360
	[SerializeField]
	private double _totalSecondsInRange = -1.0;

	// Token: 0x04003819 RID: 14361
	[NonSerialized]
	private float _elapsed = -1f;

	// Token: 0x0400381A RID: 14362
	[SerializeField]
	private BetterDayNightManager _dayNightManager;
}
