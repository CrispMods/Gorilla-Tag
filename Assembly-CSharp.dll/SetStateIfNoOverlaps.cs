using System;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public class SetStateIfNoOverlaps : SetStateConditional
{
	// Token: 0x06001C03 RID: 7171 RVA: 0x00042502 File Offset: 0x00040702
	protected override void Setup(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this._volume = animator.GetComponent<VolumeCast>();
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x00042510 File Offset: 0x00040710
	protected override bool CanSetState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		bool flag = this._volume.CheckOverlaps();
		if (flag)
		{
			this._sinceEnter = 0f;
		}
		return !flag;
	}

	// Token: 0x04001F0F RID: 7951
	public VolumeCast _volume;
}
