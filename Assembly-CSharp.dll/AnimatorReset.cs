using System;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class AnimatorReset : MonoBehaviour
{
	// Token: 0x06001C06 RID: 7174 RVA: 0x0004253B File Offset: 0x0004073B
	public void Reset()
	{
		if (!this.target)
		{
			return;
		}
		this.target.Rebind();
		this.target.Update(0f);
	}

	// Token: 0x06001C07 RID: 7175 RVA: 0x00042566 File Offset: 0x00040766
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Reset();
		}
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x00042576 File Offset: 0x00040776
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
