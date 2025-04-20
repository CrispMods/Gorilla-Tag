using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000609 RID: 1545
public class RadialBoundsTrigger : MonoBehaviour
{
	// Token: 0x06002673 RID: 9843 RVA: 0x0004A37B File Offset: 0x0004857B
	public void TestOverlap()
	{
		this.TestOverlap(this._raiseEvents);
	}

	// Token: 0x06002674 RID: 9844 RVA: 0x001087C4 File Offset: 0x001069C4
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

	// Token: 0x06002675 RID: 9845 RVA: 0x0004A389 File Offset: 0x00048589
	private void FixedUpdate()
	{
		this.TestOverlap();
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x001089B0 File Offset: 0x00106BB0
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

	// Token: 0x04002A6F RID: 10863
	[SerializeField]
	private Id32 _triggerID;

	// Token: 0x04002A70 RID: 10864
	[Space]
	public RadialBounds object1 = new RadialBounds();

	// Token: 0x04002A71 RID: 10865
	[Space]
	public RadialBounds object2 = new RadialBounds();

	// Token: 0x04002A72 RID: 10866
	[Space]
	public float hysteresis = 0.5f;

	// Token: 0x04002A73 RID: 10867
	[SerializeField]
	private bool _raiseEvents = true;

	// Token: 0x04002A74 RID: 10868
	[Space]
	private bool _overlapping;

	// Token: 0x04002A75 RID: 10869
	private float _timeSpentInOverlap;

	// Token: 0x04002A76 RID: 10870
	[Space]
	private float _timeOverlapStarted;

	// Token: 0x04002A77 RID: 10871
	private float _timeOverlapStopped;
}
