using System;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class RotationAnimation : MonoBehaviour, ITickSystemTick
{
	// Token: 0x17000261 RID: 609
	// (get) Token: 0x060015D5 RID: 5589 RVA: 0x00069958 File Offset: 0x00067B58
	// (set) Token: 0x060015D6 RID: 5590 RVA: 0x00069960 File Offset: 0x00067B60
	public bool TickRunning { get; set; }

	// Token: 0x060015D7 RID: 5591 RVA: 0x0006996C File Offset: 0x00067B6C
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

	// Token: 0x060015D8 RID: 5592 RVA: 0x00069A86 File Offset: 0x00067C86
	private void Awake()
	{
		this.baseRotation = base.transform.localRotation;
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x00069A99 File Offset: 0x00067C99
	private void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
		this.releaseSet = false;
		this.baseTime = Time.time;
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x00069AB3 File Offset: 0x00067CB3
	public void ReleaseToDisable()
	{
		this.releaseSet = true;
		this.releaseTime = Time.time;
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x00069AC7 File Offset: 0x00067CC7
	public void CancelRelease()
	{
		this.releaseSet = false;
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x00069AD0 File Offset: 0x00067CD0
	private void OnDisable()
	{
		base.transform.localRotation = this.baseRotation;
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x040017F9 RID: 6137
	[SerializeField]
	private AnimationCurve x;

	// Token: 0x040017FA RID: 6138
	[SerializeField]
	private AnimationCurve y;

	// Token: 0x040017FB RID: 6139
	[SerializeField]
	private AnimationCurve z;

	// Token: 0x040017FC RID: 6140
	[SerializeField]
	private AnimationCurve attack;

	// Token: 0x040017FD RID: 6141
	[SerializeField]
	private AnimationCurve release;

	// Token: 0x040017FE RID: 6142
	[SerializeField]
	private Vector3 amplitude = Vector3.one;

	// Token: 0x040017FF RID: 6143
	[SerializeField]
	private Vector3 period = Vector3.one;

	// Token: 0x04001800 RID: 6144
	private Quaternion baseRotation;

	// Token: 0x04001801 RID: 6145
	private float baseTime;

	// Token: 0x04001802 RID: 6146
	private float releaseTime;

	// Token: 0x04001803 RID: 6147
	private bool releaseSet;
}
