using System;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class SetStateConditional : StateMachineBehaviour
{
	// Token: 0x06001BFA RID: 7162 RVA: 0x00088363 File Offset: 0x00086563
	private void OnValidate()
	{
		this._setToID = this.setToState;
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x00088376 File Offset: 0x00086576
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

	// Token: 0x06001BFC RID: 7164 RVA: 0x000883A4 File Offset: 0x000865A4
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

	// Token: 0x06001BFD RID: 7165 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void Setup(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x000444E2 File Offset: 0x000426E2
	protected virtual bool CanSetState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		return true;
	}

	// Token: 0x04001F08 RID: 7944
	public Animator parentAnimator;

	// Token: 0x04001F09 RID: 7945
	public string setToState;

	// Token: 0x04001F0A RID: 7946
	[SerializeField]
	private AnimStateHash _setToID;

	// Token: 0x04001F0B RID: 7947
	public float delay = 1f;

	// Token: 0x04001F0C RID: 7948
	protected TimeSince _sinceEnter;

	// Token: 0x04001F0D RID: 7949
	[NonSerialized]
	private bool _didSetup;
}
