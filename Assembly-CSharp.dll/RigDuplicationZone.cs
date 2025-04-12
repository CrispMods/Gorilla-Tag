using System;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class RigDuplicationZone : MonoBehaviour
{
	// Token: 0x14000045 RID: 69
	// (add) Token: 0x060015C9 RID: 5577 RVA: 0x000BF19C File Offset: 0x000BD39C
	// (remove) Token: 0x060015CA RID: 5578 RVA: 0x000BF1D0 File Offset: 0x000BD3D0
	public static event RigDuplicationZone.RigDuplicationZoneAction OnEnabled;

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x060015CB RID: 5579 RVA: 0x0003DBE4 File Offset: 0x0003BDE4
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x0003DBEC File Offset: 0x0003BDEC
	private void OnEnable()
	{
		RigDuplicationZone.OnEnabled += this.RigDuplicationZone_OnEnabled;
		if (RigDuplicationZone.OnEnabled != null)
		{
			RigDuplicationZone.OnEnabled(this);
		}
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x0003DC11 File Offset: 0x0003BE11
	private void OnDisable()
	{
		RigDuplicationZone.OnEnabled -= this.RigDuplicationZone_OnEnabled;
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x0003DC24 File Offset: 0x0003BE24
	private void RigDuplicationZone_OnEnabled(RigDuplicationZone z)
	{
		if (z == this)
		{
			return;
		}
		if (z.id != this.id)
		{
			return;
		}
		this.setOtherZone(z);
		z.setOtherZone(this);
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x0003DC52 File Offset: 0x0003BE52
	private void setOtherZone(RigDuplicationZone z)
	{
		this.otherZone = z;
		this.offsetToOtherZone = z.transform.position - base.transform.position;
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x000BF204 File Offset: 0x000BD404
	private void OnTriggerEnter(Collider other)
	{
		VRRig component = other.GetComponent<VRRig>();
		if (component == null)
		{
			return;
		}
		if (component.isLocal)
		{
			this.playerInZone = true;
			return;
		}
		component.SetDuplicationZone(this);
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x000BF23C File Offset: 0x000BD43C
	private void OnTriggerExit(Collider other)
	{
		VRRig component = other.GetComponent<VRRig>();
		if (component == null)
		{
			return;
		}
		if (component.isLocal)
		{
			this.playerInZone = false;
			return;
		}
		component.ClearDuplicationZone(this);
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x060015D2 RID: 5586 RVA: 0x0003DC7C File Offset: 0x0003BE7C
	public Vector3 VisualOffsetForRigs
	{
		get
		{
			if (!this.otherZone.playerInZone)
			{
				return Vector3.zero;
			}
			return this.offsetToOtherZone;
		}
	}

	// Token: 0x040017F6 RID: 6134
	private RigDuplicationZone otherZone;

	// Token: 0x040017F7 RID: 6135
	[SerializeField]
	private string id;

	// Token: 0x040017F8 RID: 6136
	private bool playerInZone;

	// Token: 0x040017F9 RID: 6137
	private Vector3 offsetToOtherZone;

	// Token: 0x020003A4 RID: 932
	// (Invoke) Token: 0x060015D5 RID: 5589
	public delegate void RigDuplicationZoneAction(RigDuplicationZone z);
}
