using System;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class AnimatorReset : MonoBehaviour
{
	// Token: 0x06001C06 RID: 7174 RVA: 0x000887C0 File Offset: 0x000869C0
	public void Reset()
	{
		if (!this.target)
		{
			return;
		}
		this.target.Rebind();
		this.target.Update(0f);
	}

	// Token: 0x06001C07 RID: 7175 RVA: 0x000887EB File Offset: 0x000869EB
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Reset();
		}
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x000887FB File Offset: 0x000869FB
	private void OnDisable()
	{
		if (this.onDisable)
		{
			this.Reset();
		}
	}

	// Token: 0x04001F10 RID: 7952
	public Animator target;

	// Token: 0x04001F11 RID: 7953
	public bool onEnable;

	// Token: 0x04001F12 RID: 7954
	public bool onDisable = true;
}
