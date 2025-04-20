using System;
using UnityEngine;

// Token: 0x02000604 RID: 1540
public class OwnerRig : MonoBehaviour, IVariable<VRRig>, IVariable, IRigAware
{
	// Token: 0x06002659 RID: 9817 RVA: 0x0004A188 File Offset: 0x00048388
	public void TryFindRig()
	{
		this._rig = base.GetComponentInParent<VRRig>();
		if (this._rig != null)
		{
			return;
		}
		this._rig = base.GetComponentInChildren<VRRig>();
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x0004A1B1 File Offset: 0x000483B1
	public VRRig Get()
	{
		return this._rig;
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x0004A1B9 File Offset: 0x000483B9
	public void Set(VRRig value)
	{
		this._rig = value;
	}

	// Token: 0x0600265C RID: 9820 RVA: 0x0004A1C2 File Offset: 0x000483C2
	public void Set(GameObject obj)
	{
		this._rig = ((obj != null) ? obj.GetComponentInParent<VRRig>() : null);
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x0004A1B9 File Offset: 0x000483B9
	void IRigAware.SetRig(VRRig rig)
	{
		this._rig = rig;
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x0004A1DC File Offset: 0x000483DC
	public static implicit operator bool(OwnerRig or)
	{
		return or != null && !(or == null) && or._rig != null && !(or._rig == null);
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x0004A209 File Offset: 0x00048409
	public static implicit operator VRRig(OwnerRig or)
	{
		if (!or)
		{
			return null;
		}
		return or._rig;
	}

	// Token: 0x04002A5C RID: 10844
	[SerializeField]
	private VRRig _rig;
}
