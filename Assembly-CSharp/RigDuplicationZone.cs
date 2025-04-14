using System;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class RigDuplicationZone : MonoBehaviour
{
	// Token: 0x14000045 RID: 69
	// (add) Token: 0x060015C6 RID: 5574 RVA: 0x000697D0 File Offset: 0x000679D0
	// (remove) Token: 0x060015C7 RID: 5575 RVA: 0x00069804 File Offset: 0x00067A04
	public static event RigDuplicationZone.RigDuplicationZoneAction OnEnabled;

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x060015C8 RID: 5576 RVA: 0x00069837 File Offset: 0x00067A37
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x0006983F File Offset: 0x00067A3F
	private void OnEnable()
	{
		RigDuplicationZone.OnEnabled += this.RigDuplicationZone_OnEnabled;
		if (RigDuplicationZone.OnEnabled != null)
		{
			RigDuplicationZone.OnEnabled(this);
		}
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x00069864 File Offset: 0x00067A64
	private void OnDisable()
	{
		RigDuplicationZone.OnEnabled -= this.RigDuplicationZone_OnEnabled;
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x00069877 File Offset: 0x00067A77
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

	// Token: 0x060015CC RID: 5580 RVA: 0x000698A5 File Offset: 0x00067AA5
	private void setOtherZone(RigDuplicationZone z)
	{
		this.otherZone = z;
		this.offsetToOtherZone = z.transform.position - base.transform.position;
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x000698D0 File Offset: 0x00067AD0
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

	// Token: 0x060015CE RID: 5582 RVA: 0x00069908 File Offset: 0x00067B08
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
	// (get) Token: 0x060015CF RID: 5583 RVA: 0x0006993D File Offset: 0x00067B3D
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

	// Token: 0x040017F5 RID: 6133
	private RigDuplicationZone otherZone;

	// Token: 0x040017F6 RID: 6134
	[SerializeField]
	private string id;

	// Token: 0x040017F7 RID: 6135
	private bool playerInZone;

	// Token: 0x040017F8 RID: 6136
	private Vector3 offsetToOtherZone;

	// Token: 0x020003A4 RID: 932
	// (Invoke) Token: 0x060015D2 RID: 5586
	public delegate void RigDuplicationZoneAction(RigDuplicationZone z);
}
