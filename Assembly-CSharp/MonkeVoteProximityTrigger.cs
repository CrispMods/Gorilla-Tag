using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class MonkeVoteProximityTrigger : GorillaTriggerBox
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000756 RID: 1878 RVA: 0x0008A554 File Offset: 0x00088754
	// (remove) Token: 0x06000757 RID: 1879 RVA: 0x0008A58C File Offset: 0x0008878C
	public event Action OnEnter;

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000758 RID: 1880 RVA: 0x000353F1 File Offset: 0x000335F1
	// (set) Token: 0x06000759 RID: 1881 RVA: 0x000353F9 File Offset: 0x000335F9
	public bool isPlayerNearby { get; private set; }

	// Token: 0x0600075A RID: 1882 RVA: 0x00035402 File Offset: 0x00033602
	public override void OnBoxTriggered()
	{
		this.isPlayerNearby = true;
		if (this.triggerTime + this.retriggerDelay < Time.unscaledTime)
		{
			this.triggerTime = Time.unscaledTime;
			Action onEnter = this.OnEnter;
			if (onEnter == null)
			{
				return;
			}
			onEnter();
		}
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0003543A File Offset: 0x0003363A
	public override void OnBoxExited()
	{
		this.isPlayerNearby = false;
	}

	// Token: 0x040008AF RID: 2223
	private float triggerTime = float.MinValue;

	// Token: 0x040008B0 RID: 2224
	private float retriggerDelay = 0.25f;
}
