using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000434 RID: 1076
public class BubbleGumEvents : MonoBehaviour
{
	// Token: 0x06001A8A RID: 6794 RVA: 0x00082EC5 File Offset: 0x000810C5
	private void OnEnable()
	{
		this._edible.onBiteWorld.AddListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._edible.onBiteView.AddListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x00082EFF File Offset: 0x000810FF
	private void OnDisable()
	{
		this._edible.onBiteWorld.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteWorld));
		this._edible.onBiteView.RemoveListener(new UnityAction<VRRig, int>(this.OnBiteView));
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x00082F39 File Offset: 0x00081139
	public void OnBiteView(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, true);
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x00082F44 File Offset: 0x00081144
	public void OnBiteWorld(VRRig rig, int nextState)
	{
		this.OnBite(rig, nextState, false);
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x00082F50 File Offset: 0x00081150
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

	// Token: 0x04001D52 RID: 7506
	[SerializeField]
	private EdibleHoldable _edible;

	// Token: 0x04001D53 RID: 7507
	[SerializeField]
	private GumBubble _bubble;

	// Token: 0x04001D54 RID: 7508
	private static Dictionary<GameObject, GumBubble> gTargetCache = new Dictionary<GameObject, GumBubble>(16);

	// Token: 0x02000435 RID: 1077
	public enum EdibleState
	{
		// Token: 0x04001D56 RID: 7510
		A = 1,
		// Token: 0x04001D57 RID: 7511
		B,
		// Token: 0x04001D58 RID: 7512
		C = 4,
		// Token: 0x04001D59 RID: 7513
		D = 8
	}
}
