using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class SetStateIfNoOverlaps : SetStateConditional
{
	// Token: 0x06001C54 RID: 7252 RVA: 0x0004383B File Offset: 0x00041A3B
	protected override void Setup(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this._volume = animator.GetComponent<VolumeCast>();
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x00043849 File Offset: 0x00041A49
	protected override bool CanSetState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		bool flag = this._volume.CheckOverlaps();
		if (flag)
		{
			this._sinceEnter = 0f;
		}
		return !flag;
	}

	// Token: 0x04001F5D RID: 8029
	public VolumeCast _volume;
}
