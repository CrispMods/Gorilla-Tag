using System;
using UnityEngine;

// Token: 0x020003B0 RID: 944
public class RotationAnimation : MonoBehaviour, ITickSystemTick
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06001621 RID: 5665 RVA: 0x0003EF57 File Offset: 0x0003D157
	// (set) Token: 0x06001622 RID: 5666 RVA: 0x0003EF5F File Offset: 0x0003D15F
	public bool TickRunning { get; set; }

	// Token: 0x06001623 RID: 5667 RVA: 0x000C1A9C File Offset: 0x000BFC9C
	public void Tick()
	{
		Vector3 vector = Vector3.zero;
		vector.x = this.amplitude.x * this.x.Evaluate((Time.time - this.baseTime) * this.period.x % 1f);
		vector.y = this.amplitude.y * this.y.Evaluate((Time.time - this.baseTime) * this.period.y % 1f);
		vector.z = this.amplitude.z * this.z.Evaluate((Time.time - this.baseTime) * this.period.z % 1f);
		if (this.releaseSet)
		{
			float num = this.release.Evaluate(Time.time - this.releaseTime);
			vector *= num;
			if (num < Mathf.Epsilon)
			{
				base.enabled = false;
			}
		}
		base.transform.localRotation = Quaternion.Euler(vector) * this.baseRotation;
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x0003EF68 File Offset: 0x0003D168
	private void Awake()
	{
		this.baseRotation = base.transform.localRotation;
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x0003EF7B File Offset: 0x0003D17B
	private void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
		this.releaseSet = false;
		this.baseTime = Time.time;
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x0003EF95 File Offset: 0x0003D195
	public void ReleaseToDisable()
	{
		this.releaseSet = true;
		this.releaseTime = Time.time;
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x0003EFA9 File Offset: 0x0003D1A9
	public void CancelRelease()
	{
		this.releaseSet = false;
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x0003EFB2 File Offset: 0x0003D1B2
	private void OnDisable()
	{
		base.transform.localRotation = this.baseRotation;
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x04001840 RID: 6208
	[SerializeField]
	private AnimationCurve x;

	// Token: 0x04001841 RID: 6209
	[SerializeField]
	private AnimationCurve y;

	// Token: 0x04001842 RID: 6210
	[SerializeField]
	private AnimationCurve z;

	// Token: 0x04001843 RID: 6211
	[SerializeField]
	private AnimationCurve attack;

	// Token: 0x04001844 RID: 6212
	[SerializeField]
	private AnimationCurve release;

	// Token: 0x04001845 RID: 6213
	[SerializeField]
	private Vector3 amplitude = Vector3.one;

	// Token: 0x04001846 RID: 6214
	[SerializeField]
	private Vector3 period = Vector3.one;

	// Token: 0x04001847 RID: 6215
	private Quaternion baseRotation;

	// Token: 0x04001848 RID: 6216
	private float baseTime;

	// Token: 0x04001849 RID: 6217
	private float releaseTime;

	// Token: 0x0400184A RID: 6218
	private bool releaseSet;
}
