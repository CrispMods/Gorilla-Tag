using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000439 RID: 1081
public class HotPepperEvents : MonoBehaviour
{
	// Token: 0x06001A9F RID: 6815 RVA: 0x0008330F File Offset: 0x0008150F
	private void OnEnable()
	{
		this._pepper.onBiteWorld.AddListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._pepper.onBiteView.AddListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x00083349 File Offset: 0x00081549
	private void OnDisable()
	{
		this._pepper.onBiteWorld.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._pepper.onBiteView.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x00083383 File Offset: 0x00081583
	public void OnBiteView(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, true);
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x0008338E File Offset: 0x0008158E
	public void OnBiteWorld(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, false);
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x00083399 File Offset: 0x00081599
	public void OnBite(VRRig rig, int nextState, bool isViewRig)
	{
		if (nextState != 8)
		{
			return;
		}
		rig.transform.Find("RigAnchor/rig/body/head/gorillaface/spicy").gameObject.GetComponent<HotPepperFace>().PlayFX(1f);
	}

	// Token: 0x04001D69 RID: 7529
	[SerializeField]
	private EdibleHoldable _pepper;

	// Token: 0x0200043A RID: 1082
	public enum EdibleState
	{
		// Token: 0x04001D6B RID: 7531
		A = 1,
		// Token: 0x04001D6C RID: 7532
		B,
		// Token: 0x04001D6D RID: 7533
		C = 4,
		// Token: 0x04001D6E RID: 7534
		D = 8
	}
}
