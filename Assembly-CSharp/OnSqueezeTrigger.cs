using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200017A RID: 378
public class OnSqueezeTrigger : MonoBehaviour
{
	// Token: 0x06000977 RID: 2423 RVA: 0x00036A75 File Offset: 0x00034C75
	private void Start()
	{
		this.myRig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00091F90 File Offset: 0x00090190
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

	// Token: 0x04000B71 RID: 2929
	[SerializeField]
	private TransferrableObject myHoldable;

	// Token: 0x04000B72 RID: 2930
	[SerializeField]
	private UnityEvent onPress;

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private UnityEvent onRelease;

	// Token: 0x04000B74 RID: 2932
	[SerializeField]
	private UnityEvent updateWhilePressed;

	// Token: 0x04000B75 RID: 2933
	private VRRig myRig;

	// Token: 0x04000B76 RID: 2934
	private bool indexFinger = true;

	// Token: 0x04000B77 RID: 2935
	private bool triggerWasDown;
}
