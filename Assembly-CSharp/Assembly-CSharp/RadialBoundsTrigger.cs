using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200062B RID: 1579
public class RadialBoundsTrigger : MonoBehaviour
{
	// Token: 0x06002750 RID: 10064 RVA: 0x000C1110 File Offset: 0x000BF310
	public void TestOverlap()
	{
		this.TestOverlap(this._raiseEvents);
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x000C1120 File Offset: 0x000BF320
	public void TestOverlap(bool raiseEvents)
	{
		if (!this.object1 || !this.object2)
		{
			this._overlapping = false;
			this._timeOverlapStarted = -1f;
			this._timeOverlapStopped = -1f;
			this._timeSpentInOverlap = 0f;
			return;
		}
		float time = Time.time;
		float num = this.object1.radius + this.object2.radius;
		bool flag = (this.object2.center - this.object1.center).sqrMagnitude <= num * num;
		if (this._overlapping && flag)
		{
			this._overlapping = true;
			this._timeSpentInOverlap = time - this._timeOverlapStarted;
			if (raiseEvents)
			{
				UnityEvent<RadialBounds, float> onOverlapStay = this.object1.onOverlapStay;
				if (onOverlapStay != null)
				{
					onOverlapStay.Invoke(this.object2, this._timeSpentInOverlap);
				}
				UnityEvent<RadialBounds, float> onOverlapStay2 = this.object2.onOverlapStay;
				if (onOverlapStay2 == null)
				{
					return;
				}
				onOverlapStay2.Invoke(this.object1, this._timeSpentInOverlap);
				return;
			}
		}
		else if (!this._overlapping && flag)
		{
			if (time - this._timeOverlapStopped < this.hysteresis)
			{
				return;
			}
			this._overlapping = true;
			this._timeOverlapStarted = time;
			this._timeOverlapStopped = -1f;
			this._timeSpentInOverlap = 0f;
			if (raiseEvents)
			{
				UnityEvent<RadialBounds> onOverlapEnter = this.object1.onOverlapEnter;
				if (onOverlapEnter != null)
				{
					onOverlapEnter.Invoke(this.object2);
				}
				UnityEvent<RadialBounds> onOverlapEnter2 = this.object2.onOverlapEnter;
				if (onOverlapEnter2 == null)
				{
					return;
				}
				onOverlapEnter2.Invoke(this.object1);
				return;
			}
		}
		else if (!flag && this._overlapping)
		{
			this._overlapping = false;
			this._timeOverlapStarted = -1f;
			this._timeOverlapStopped = time;
			this._timeSpentInOverlap = 0f;
			if (raiseEvents)
			{
				UnityEvent<RadialBounds> onOverlapExit = this.object1.onOverlapExit;
				if (onOverlapExit != null)
				{
					onOverlapExit.Invoke(this.object2);
				}
				UnityEvent<RadialBounds> onOverlapExit2 = this.object2.onOverlapExit;
				if (onOverlapExit2 == null)
				{
					return;
				}
				onOverlapExit2.Invoke(this.object1);
			}
		}
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x000C130C File Offset: 0x000BF50C
	private void FixedUpdate()
	{
		this.TestOverlap();
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x000C1314 File Offset: 0x000BF514
	private void OnDisable()
	{
		if (this._raiseEvents && this.object1 && this.object2 && this._overlapping)
		{
			UnityEvent<RadialBounds> onOverlapExit = this.object1.onOverlapExit;
			if (onOverlapExit != null)
			{
				onOverlapExit.Invoke(this.object2);
			}
			UnityEvent<RadialBounds> onOverlapExit2 = this.object2.onOverlapExit;
			if (onOverlapExit2 != null)
			{
				onOverlapExit2.Invoke(this.object1);
			}
		}
		this._timeOverlapStarted = -1f;
		this._timeSpentInOverlap = 0f;
		this._overlapping = false;
	}

	// Token: 0x04002B0F RID: 11023
	[SerializeField]
	private Id32 _triggerID;

	// Token: 0x04002B10 RID: 11024
	[Space]
	public RadialBounds object1 = new RadialBounds();

	// Token: 0x04002B11 RID: 11025
	[Space]
	public RadialBounds object2 = new RadialBounds();

	// Token: 0x04002B12 RID: 11026
	[Space]
	public float hysteresis = 0.5f;

	// Token: 0x04002B13 RID: 11027
	[SerializeField]
	private bool _raiseEvents = true;

	// Token: 0x04002B14 RID: 11028
	[Space]
	private bool _overlapping;

	// Token: 0x04002B15 RID: 11029
	private float _timeSpentInOverlap;

	// Token: 0x04002B16 RID: 11030
	[Space]
	private float _timeOverlapStarted;

	// Token: 0x04002B17 RID: 11031
	private float _timeOverlapStopped;
}
