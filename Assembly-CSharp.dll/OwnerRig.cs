using System;
using UnityEngine;

// Token: 0x02000626 RID: 1574
public class OwnerRig : MonoBehaviour, IVariable<VRRig>, IVariable, IRigAware
{
	// Token: 0x06002736 RID: 10038 RVA: 0x00049BF3 File Offset: 0x00047DF3
	public void TryFindRig()
	{
		this._rig = base.GetComponentInParent<VRRig>();
		if (this._rig != null)
		{
			return;
		}
		this._rig = base.GetComponentInChildren<VRRig>();
	}

	// Token: 0x06002737 RID: 10039 RVA: 0x00049C1C File Offset: 0x00047E1C
	public VRRig Get()
	{
		return this._rig;
	}

	// Token: 0x06002738 RID: 10040 RVA: 0x00049C24 File Offset: 0x00047E24
	public void Set(VRRig value)
	{
		this._rig = value;
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x00049C2D File Offset: 0x00047E2D
	public void Set(GameObject obj)
	{
		this._rig = ((obj != null) ? obj.GetComponentInParent<VRRig>() : null);
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x00049C24 File Offset: 0x00047E24
	void IRigAware.SetRig(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x0600273B RID: 10043 RVA: 0x00049C47 File Offset: 0x00047E47
	public static implicit operator bool(OwnerRig or)
	{
		return or != null && !(or == null) && or._rig != null && !(or._rig == null);
	}

	// Token: 0x0600273C RID: 10044 RVA: 0x00049C74 File Offset: 0x00047E74
	public static implicit operator VRRig(OwnerRig or)
	{
		if (!or)
		{
			return null;
		}
		return or._rig;
	}

	// Token: 0x04002AFC RID: 11004
	[SerializeField]
	private VRRig _rig;
}
