using System;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public class SetStateIfNoOverlaps : SetStateConditional
{
	// Token: 0x06001C00 RID: 7168 RVA: 0x00088403 File Offset: 0x00086603
	protected override void Setup(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this._volume = animator.GetComponent<VolumeCast>();
	}

	// Token: 0x06001C01 RID: 7169 RVA: 0x00088411 File Offset: 0x00086611
	protected override bool CanSetState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		bool flag = this._volume.CheckOverlaps();
		if (flag)
		{
			this._sinceEnter = 0f;
		}
		return !flag;
	}

	// Token: 0x04001F0E RID: 7950
	public VolumeCast _volume;
}
