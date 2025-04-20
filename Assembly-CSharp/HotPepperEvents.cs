using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000445 RID: 1093
public class HotPepperEvents : MonoBehaviour
{
	// Token: 0x06001AF3 RID: 6899 RVA: 0x0004244E File Offset: 0x0004064E
	private void OnEnable()
	{
		this._pepper.onBiteWorld.AddListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._pepper.onBiteView.AddListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x00042488 File Offset: 0x00040688
	private void OnDisable()
	{
		this._pepper.onBiteWorld.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._pepper.onBiteView.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x000424C2 File Offset: 0x000406C2
	public void OnBiteView(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, true);
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000424CD File Offset: 0x000406CD
	public void OnBiteWorld(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, false);
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000424D8 File Offset: 0x000406D8
	public void OnBite(VRRig rig, int nextState, bool isViewRig)
	{
		if (nextState != 8)
		{
			return;
		}
		rig.transform.Find("RigAnchor/rig/body/head/gorillaface/spicy").gameObject.GetComponent<HotPepperFace>().PlayFX(1f);
	}

	// Token: 0x04001DB8 RID: 7608
	[SerializeField]
	private EdibleHoldable _pepper;

	// Token: 0x02000446 RID: 1094
	public enum EdibleState
	{
		// Token: 0x04001DBA RID: 7610
		A = 1,
		// Token: 0x04001DBB RID: 7611
		B,
		// Token: 0x04001DBC RID: 7612
		C = 4,
		// Token: 0x04001DBD RID: 7613
		D = 8
	}
}
