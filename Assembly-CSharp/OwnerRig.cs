using System;
using UnityEngine;

// Token: 0x02000625 RID: 1573
public class OwnerRig : MonoBehaviour, IVariable<VRRig>, IVariable, IRigAware
{
	// Token: 0x0600272E RID: 10030 RVA: 0x000C0A0B File Offset: 0x000BEC0B
	public void TryFindRig()
	{
		this._rig = base.GetComponentInParent<VRRig>();
		if (this._rig != null)
		{
			return;
		}
		this._rig = base.GetComponentInChildren<VRRig>();
	}

	// Token: 0x0600272F RID: 10031 RVA: 0x000C0A34 File Offset: 0x000BEC34
	public VRRig Get()
	{
		return this._rig;
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x000C0A3C File Offset: 0x000BEC3C
	public void Set(VRRig value)
	{
		this._rig = value;
	}

	// Token: 0x06002731 RID: 10033 RVA: 0x000C0A45 File Offset: 0x000BEC45
	public void Set(GameObject obj)
	{
		this._rig = ((obj != null) ? obj.GetComponentInParent<VRRig>() : null);
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x000C0A3C File Offset: 0x000BEC3C
	void IRigAware.SetRig(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x06002733 RID: 10035 RVA: 0x000C0A5F File Offset: 0x000BEC5F
	public static implicit operator bool(OwnerRig or)
	{
		return or != null && !(or == null) && or._rig != null && !(or._rig == null);
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x000C0A8C File Offset: 0x000BEC8C
	public static implicit operator VRRig(OwnerRig or)
	{
		if (!or)
		{
			return null;
		}
		return or._rig;
	}

	// Token: 0x04002AF6 RID: 10998
	[SerializeField]
	private VRRig _rig;
}
