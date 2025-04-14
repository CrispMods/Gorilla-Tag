using System;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class MonkeVoteProximityTrigger : GorillaTriggerBox
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000715 RID: 1813 RVA: 0x000287B4 File Offset: 0x000269B4
	// (remove) Token: 0x06000716 RID: 1814 RVA: 0x000287EC File Offset: 0x000269EC
	public event Action OnEnter;

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000717 RID: 1815 RVA: 0x00028821 File Offset: 0x00026A21
	// (set) Token: 0x06000718 RID: 1816 RVA: 0x00028829 File Offset: 0x00026A29
	public bool isPlayerNearby { get; private set; }

	// Token: 0x06000719 RID: 1817 RVA: 0x00028832 File Offset: 0x00026A32
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

	// Token: 0x0600071A RID: 1818 RVA: 0x0002886A File Offset: 0x00026A6A
	public override void OnBoxExited()
	{
		this.isPlayerNearby = false;
	}

	// Token: 0x0400086E RID: 2158
	private float triggerTime = float.MinValue;

	// Token: 0x0400086F RID: 2159
	private float retriggerDelay = 0.25f;
}
