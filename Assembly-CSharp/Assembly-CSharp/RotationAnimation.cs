using System;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class RotationAnimation : MonoBehaviour, ITickSystemTick
{
	// Token: 0x17000261 RID: 609
	// (get) Token: 0x060015D8 RID: 5592 RVA: 0x00069CDC File Offset: 0x00067EDC
	// (set) Token: 0x060015D9 RID: 5593 RVA: 0x00069CE4 File Offset: 0x00067EE4
	public bool TickRunning { get; set; }

	// Token: 0x060015DA RID: 5594 RVA: 0x00069CF0 File Offset: 0x00067EF0
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

	// Token: 0x060015DB RID: 5595 RVA: 0x00069E0A File Offset: 0x0006800A
	private void Awake()
	{
		this.baseRotation = base.transform.localRotation;
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x00069E1D File Offset: 0x0006801D
	private void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
		this.releaseSet = false;
		this.baseTime = Time.time;
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x00069E37 File Offset: 0x00068037
	public void ReleaseToDisable()
	{
		this.releaseSet = true;
		this.releaseTime = Time.time;
	}

	// Token: 0x060015DE RID: 5598 RVA: 0x00069E4B File Offset: 0x0006804B
	public void CancelRelease()
	{
		this.releaseSet = false;
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x00069E54 File Offset: 0x00068054
	private void OnDisable()
	{
		base.transform.localRotation = this.baseRotation;
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x040017FA RID: 6138
	[SerializeField]
	private AnimationCurve x;

	// Token: 0x040017FB RID: 6139
	[SerializeField]
	private AnimationCurve y;

	// Token: 0x040017FC RID: 6140
	[SerializeField]
	private AnimationCurve z;

	// Token: 0x040017FD RID: 6141
	[SerializeField]
	private AnimationCurve attack;

	// Token: 0x040017FE RID: 6142
	[SerializeField]
	private AnimationCurve release;

	// Token: 0x040017FF RID: 6143
	[SerializeField]
	private Vector3 amplitude = Vector3.one;

	// Token: 0x04001800 RID: 6144
	[SerializeField]
	private Vector3 period = Vector3.one;

	// Token: 0x04001801 RID: 6145
	private Quaternion baseRotation;

	// Token: 0x04001802 RID: 6146
	private float baseTime;

	// Token: 0x04001803 RID: 6147
	private float releaseTime;

	// Token: 0x04001804 RID: 6148
	private bool releaseSet;
}
