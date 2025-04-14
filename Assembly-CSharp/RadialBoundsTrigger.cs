using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200062A RID: 1578
public class RadialBoundsTrigger : MonoBehaviour
{
	// Token: 0x06002748 RID: 10056 RVA: 0x000C0C90 File Offset: 0x000BEE90
	public void TestOverlap()
	{
		this.TestOverlap(this._raiseEvents);
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x000C0CA0 File Offset: 0x000BEEA0
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

	// Token: 0x0600274A RID: 10058 RVA: 0x000C0E8C File Offset: 0x000BF08C
	private void FixedUpdate()
	{
		this.TestOverlap();
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x000C0E94 File Offset: 0x000BF094
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

	// Token: 0x04002B09 RID: 11017
	[SerializeField]
	private Id32 _triggerID;

	// Token: 0x04002B0A RID: 11018
	[Space]
	public RadialBounds object1 = new RadialBounds();

	// Token: 0x04002B0B RID: 11019
	[Space]
	public RadialBounds object2 = new RadialBounds();

	// Token: 0x04002B0C RID: 11020
	[Space]
	public float hysteresis = 0.5f;

	// Token: 0x04002B0D RID: 11021
	[SerializeField]
	private bool _raiseEvents = true;

	// Token: 0x04002B0E RID: 11022
	[Space]
	private bool _overlapping;

	// Token: 0x04002B0F RID: 11023
	private float _timeSpentInOverlap;

	// Token: 0x04002B10 RID: 11024
	[Space]
	private float _timeOverlapStarted;

	// Token: 0x04002B11 RID: 11025
	private float _timeOverlapStopped;
}
