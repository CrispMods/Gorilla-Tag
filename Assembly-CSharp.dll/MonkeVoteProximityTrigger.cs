using System;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class MonkeVoteProximityTrigger : GorillaTriggerBox
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000717 RID: 1815 RVA: 0x00087C4C File Offset: 0x00085E4C
	// (remove) Token: 0x06000718 RID: 1816 RVA: 0x00087C84 File Offset: 0x00085E84
	public event Action OnEnter;

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000719 RID: 1817 RVA: 0x0003418D File Offset: 0x0003238D
	// (set) Token: 0x0600071A RID: 1818 RVA: 0x00034195 File Offset: 0x00032395
	public bool isPlayerNearby { get; private set; }

	// Token: 0x0600071B RID: 1819 RVA: 0x0003419E File Offset: 0x0003239E
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

	// Token: 0x0600071C RID: 1820 RVA: 0x000341D6 File Offset: 0x000323D6
	public override void OnBoxExited()
	{
		this.isPlayerNearby = false;
	}

	// Token: 0x0400086F RID: 2159
	private float triggerTime = float.MinValue;

	// Token: 0x04000870 RID: 2160
	private float retriggerDelay = 0.25f;
}
