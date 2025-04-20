using System;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class SetStateConditional : StateMachineBehaviour
{
	// Token: 0x06001C4E RID: 7246 RVA: 0x000437E9 File Offset: 0x000419E9
	private void OnValidate()
	{
		this._setToID = this.setToState;
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x000437FC File Offset: 0x000419FC
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

	// Token: 0x06001C50 RID: 7248 RVA: 0x000DC014 File Offset: 0x000DA214
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

	// Token: 0x06001C51 RID: 7249 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void Setup(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x00039846 File Offset: 0x00037A46
	protected virtual bool CanSetState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		return true;
	}

	// Token: 0x04001F57 RID: 8023
	public Animator parentAnimator;

	// Token: 0x04001F58 RID: 8024
	public string setToState;

	// Token: 0x04001F59 RID: 8025
	[SerializeField]
	private AnimStateHash _setToID;

	// Token: 0x04001F5A RID: 8026
	public float delay = 1f;

	// Token: 0x04001F5B RID: 8027
	protected TimeSince _sinceEnter;

	// Token: 0x04001F5C RID: 8028
	[NonSerialized]
	private bool _didSetup;
}
