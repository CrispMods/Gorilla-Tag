using System;
using UnityEngine;

// Token: 0x020003AE RID: 942
public class RigDuplicationZone : MonoBehaviour
{
	// Token: 0x14000046 RID: 70
	// (add) Token: 0x06001612 RID: 5650 RVA: 0x000C19C4 File Offset: 0x000BFBC4
	// (remove) Token: 0x06001613 RID: 5651 RVA: 0x000C19F8 File Offset: 0x000BFBF8
	public static event RigDuplicationZone.RigDuplicationZoneAction OnEnabled;

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06001614 RID: 5652 RVA: 0x0003EEA4 File Offset: 0x0003D0A4
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x0003EEAC File Offset: 0x0003D0AC
	private void OnEnable()
	{
		RigDuplicationZone.OnEnabled += this.RigDuplicationZone_OnEnabled;
		if (RigDuplicationZone.OnEnabled != null)
		{
			RigDuplicationZone.OnEnabled(this);
		}
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x0003EED1 File Offset: 0x0003D0D1
	private void OnDisable()
	{
		RigDuplicationZone.OnEnabled -= this.RigDuplicationZone_OnEnabled;
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x0003EEE4 File Offset: 0x0003D0E4
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

	// Token: 0x06001618 RID: 5656 RVA: 0x0003EF12 File Offset: 0x0003D112
	private void setOtherZone(RigDuplicationZone z)
	{
		this.otherZone = z;
		this.offsetToOtherZone = z.transform.position - base.transform.position;
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x000C1A2C File Offset: 0x000BFC2C
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

	// Token: 0x0600161A RID: 5658 RVA: 0x000C1A64 File Offset: 0x000BFC64
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

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x0600161B RID: 5659 RVA: 0x0003EF3C File Offset: 0x0003D13C
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

	// Token: 0x0400183C RID: 6204
	private RigDuplicationZone otherZone;

	// Token: 0x0400183D RID: 6205
	[SerializeField]
	private string id;

	// Token: 0x0400183E RID: 6206
	private bool playerInZone;

	// Token: 0x0400183F RID: 6207
	private Vector3 offsetToOtherZone;

	// Token: 0x020003AF RID: 943
	// (Invoke) Token: 0x0600161E RID: 5662
	public delegate void RigDuplicationZoneAction(RigDuplicationZone z);
}
