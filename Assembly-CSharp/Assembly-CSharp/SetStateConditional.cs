using System;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class SetStateConditional : StateMachineBehaviour
{
	// Token: 0x06001BFD RID: 7165 RVA: 0x000886E7 File Offset: 0x000868E7
	private void OnValidate()
	{
		this._setToID = this.setToState;
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x000886FA File Offset: 0x000868FA
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!this._didSetup)
		{
			this.parentAnimator = animator;
			this.Setup(animator, stateInfo, layerIndex);
			this._didSetup = true;
		}
		this._sinceEnter = TimeSince.Now();
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x00088728 File Offset: 0x00086928
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.delay > 0f && !this._sinceEnter.HasElapsed(this.delay, true))
		{
			return;
		}
		if (!this.CanSetState(animator, stateInfo, layerIndex))
		{
			return;
		}
		animator.Play(this._setToID);
	}

	// Token: 0x06001C00 RID: 7168 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void Setup(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	// Token: 0x06001C01 RID: 7169 RVA: 0x00044826 File Offset: 0x00042A26
	protected virtual bool CanSetState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		return true;
	}

	// Token: 0x04001F09 RID: 7945
	public Animator parentAnimator;

	// Token: 0x04001F0A RID: 7946
	public string setToState;

	// Token: 0x04001F0B RID: 7947
	[SerializeField]
	private AnimStateHash _setToID;

	// Token: 0x04001F0C RID: 7948
	public float delay = 1f;

	// Token: 0x04001F0D RID: 7949
	protected TimeSince _sinceEnter;

	// Token: 0x04001F0E RID: 7950
	[NonSerialized]
	private bool _didSetup;
}
