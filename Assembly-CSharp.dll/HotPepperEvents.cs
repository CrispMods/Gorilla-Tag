using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000439 RID: 1081
public class HotPepperEvents : MonoBehaviour
{
	// Token: 0x06001AA2 RID: 6818 RVA: 0x00041115 File Offset: 0x0003F315
	private void OnEnable()
	{
		this._pepper.onBiteWorld.AddListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._pepper.onBiteView.AddListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x0004114F File Offset: 0x0003F34F
	private void OnDisable()
	{
		this._pepper.onBiteWorld.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._pepper.onBiteView.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x00041189 File Offset: 0x0003F389
	public void OnBiteView(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, true);
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x00041194 File Offset: 0x0003F394
	public void OnBiteWorld(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, false);
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x0004119F File Offset: 0x0003F39F
	public void OnBite(VRRig rig, int nextState, bool isViewRig)
	{
		if (nextState != 8)
		{
			return;
		}
		rig.transform.Find("RigAnchor/rig/body/head/gorillaface/spicy").gameObject.GetComponent<HotPepperFace>().PlayFX(1f);
	}

	// Token: 0x04001D6A RID: 7530
	[SerializeField]
	private EdibleHoldable _pepper;

	// Token: 0x0200043A RID: 1082
	public enum EdibleState
	{
		// Token: 0x04001D6C RID: 7532
		A = 1,
		// Token: 0x04001D6D RID: 7533
		B,
		// Token: 0x04001D6E RID: 7534
		C = 4,
		// Token: 0x04001D6F RID: 7535
		D = 8
	}
}
