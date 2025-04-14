using System;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class MonkeVoteProximityTrigger : GorillaTriggerBox
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000717 RID: 1815 RVA: 0x00028AD8 File Offset: 0x00026CD8
	// (remove) Token: 0x06000718 RID: 1816 RVA: 0x00028B10 File Offset: 0x00026D10
	public event Action OnEnter;

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000719 RID: 1817 RVA: 0x00028B45 File Offset: 0x00026D45
	// (set) Token: 0x0600071A RID: 1818 RVA: 0x00028B4D File Offset: 0x00026D4D
	public bool isPlayerNearby { get; private set; }

	// Token: 0x0600071B RID: 1819 RVA: 0x00028B56 File Offset: 0x00026D56
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

	// Token: 0x0600071C RID: 1820 RVA: 0x00028B8E File Offset: 0x00026D8E
	public override void OnBoxExited()
	{
		this.isPlayerNearby = false;
	}

	// Token: 0x0400086F RID: 2159
	private float triggerTime = float.MinValue;

	// Token: 0x04000870 RID: 2160
	private float retriggerDelay = 0.25f;
}
