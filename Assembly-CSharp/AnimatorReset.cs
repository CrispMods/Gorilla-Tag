using System;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class AnimatorReset : MonoBehaviour
{
	// Token: 0x06001C57 RID: 7255 RVA: 0x00043874 File Offset: 0x00041A74
	public void Reset()
	{
		if (!this.target)
		{
			return;
		}
		this.target.Rebind();
		this.target.Update(0f);
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x0004389F File Offset: 0x00041A9F
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Reset();
		}
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x000438AF File Offset: 0x00041AAF
	private void OnDisable()
	{
		if (this.onDisable)
		{
			this.Reset();
		}
	}

	// Token: 0x04001F5E RID: 8030
	public Animator target;

	// Token: 0x04001F5F RID: 8031
	public bool onEnable;

	// Token: 0x04001F60 RID: 8032
	public bool onDisable = true;
}
