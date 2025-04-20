using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000440 RID: 1088
public class BubbleGumEvents : MonoBehaviour
{
	// Token: 0x06001ADE RID: 6878 RVA: 0x00042315 File Offset: 0x00040515
	private void OnEnable()
	{
		this._edible.onBiteWorld.AddListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._edible.onBiteView.AddListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x0004234F File Offset: 0x0004054F
	private void OnDisable()
	{
		this._edible.onBiteWorld.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._edible.onBiteView.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x00042389 File Offset: 0x00040589
	public void OnBiteView(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, true);
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x00042394 File Offset: 0x00040594
	public void OnBiteWorld(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, false);
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000D8088 File Offset: 0x000D6288
	public void OnBite(VRRig rig, int nextState, bool isViewRig)
	{
		GorillaTagger instance = GorillaTagger.Instance;
		GameObject gameObject = null;
		if (isViewRig && instance != null)
		{
			gameObject = instance.gameObject;
		}
		else if (!isViewRig)
		{
			gameObject = rig.gameObject;
		}
		if (!BubbleGumEvents.gTargetCache.TryGetValue(gameObject, out this._bubble))
		{
			this._bubble = gameObject.GetComponentsInChildren<GumBubble>(true).FirstOrDefault((GumBubble g) => g.transform.parent.name == "$gum");
			if (isViewRig)
			{
				this._bubble.audioSource = instance.offlineVRRig.tagSound;
				this._bubble.targetScale = Vector3.one * 1.36f;
			}
			else
			{
				this._bubble.audioSource = rig.tagSound;
				this._bubble.targetScale = Vector3.one * 2f;
			}
			BubbleGumEvents.gTargetCache.Add(gameObject, this._bubble);
		}
		GumBubble bubble = this._bubble;
		if (bubble != null)
		{
			bubble.transform.parent.gameObject.SetActive(true);
		}
		GumBubble bubble2 = this._bubble;
		if (bubble2 == null)
		{
			return;
		}
		bubble2.InflateDelayed();
	}

	// Token: 0x04001DA1 RID: 7585
	[SerializeField]
	private EdibleHoldable _edible;

	// Token: 0x04001DA2 RID: 7586
	[SerializeField]
	private GumBubble _bubble;

	// Token: 0x04001DA3 RID: 7587
	private static Dictionary<GameObject, GumBubble> gTargetCache = new Dictionary<GameObject, GumBubble>(16);

	// Token: 0x02000441 RID: 1089
	public enum EdibleState
	{
		// Token: 0x04001DA5 RID: 7589
		A = 1,
		// Token: 0x04001DA6 RID: 7590
		B,
		// Token: 0x04001DA7 RID: 7591
		C = 4,
		// Token: 0x04001DA8 RID: 7592
		D = 8
	}
}
