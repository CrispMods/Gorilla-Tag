using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200016F RID: 367
public class OnSqueezeTrigger : MonoBehaviour
{
	// Token: 0x0600092A RID: 2346 RVA: 0x00031797 File Offset: 0x0002F997
	private void Start()
	{
		this.myRig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x000317A8 File Offset: 0x0002F9A8
	private void Update()
	{
		bool flag;
		if (this.myHoldable.InLeftHand())
		{
			flag = ((this.indexFinger ? this.myRig.leftIndex.calcT : this.myRig.leftMiddle.calcT) > 0.5f);
		}
		else
		{
			flag = (this.myHoldable.InRightHand() && (this.indexFinger ? this.myRig.rightIndex.calcT : this.myRig.rightMiddle.calcT) > 0.5f);
		}
		if (flag != this.triggerWasDown)
		{
			if (flag)
			{
				this.onPress.Invoke();
				this.updateWhilePressed.Invoke();
			}
			else
			{
				this.onRelease.Invoke();
			}
		}
		else if (flag)
		{
			this.updateWhilePressed.Invoke();
		}
		this.triggerWasDown = flag;
	}

	// Token: 0x04000B2A RID: 2858
	[SerializeField]
	private TransferrableObject myHoldable;

	// Token: 0x04000B2B RID: 2859
	[SerializeField]
	private UnityEvent onPress;

	// Token: 0x04000B2C RID: 2860
	[SerializeField]
	private UnityEvent onRelease;

	// Token: 0x04000B2D RID: 2861
	[SerializeField]
	private UnityEvent updateWhilePressed;

	// Token: 0x04000B2E RID: 2862
	private VRRig myRig;

	// Token: 0x04000B2F RID: 2863
	private bool indexFinger = true;

	// Token: 0x04000B30 RID: 2864
	private bool triggerWasDown;
}
