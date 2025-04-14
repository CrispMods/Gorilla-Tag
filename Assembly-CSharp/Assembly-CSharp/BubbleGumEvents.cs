using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000434 RID: 1076
public class BubbleGumEvents : MonoBehaviour
{
	// Token: 0x06001A8D RID: 6797 RVA: 0x00083249 File Offset: 0x00081449
	private void OnEnable()
	{
		this._edible.onBiteWorld.AddListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._edible.onBiteView.AddListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x00083283 File Offset: 0x00081483
	private void OnDisable()
	{
		this._edible.onBiteWorld.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._edible.onBiteView.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x000832BD File Offset: 0x000814BD
	public void OnBiteView(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, true);
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000832C8 File Offset: 0x000814C8
	public void OnBiteWorld(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, false);
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000832D4 File Offset: 0x000814D4
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

	// Token: 0x04001D53 RID: 7507
	[SerializeField]
	private EdibleHoldable _edible;

	// Token: 0x04001D54 RID: 7508
	[SerializeField]
	private GumBubble _bubble;

	// Token: 0x04001D55 RID: 7509
	private static Dictionary<GameObject, GumBubble> gTargetCache = new Dictionary<GameObject, GumBubble>(16);

	// Token: 0x02000435 RID: 1077
	public enum EdibleState
	{
		// Token: 0x04001D57 RID: 7511
		A = 1,
		// Token: 0x04001D58 RID: 7512
		B,
		// Token: 0x04001D59 RID: 7513
		C = 4,
		// Token: 0x04001D5A RID: 7514
		D = 8
	}
}
