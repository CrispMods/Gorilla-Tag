using System;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class AnimatorReset : MonoBehaviour
{
	// Token: 0x06001C03 RID: 7171 RVA: 0x0008843C File Offset: 0x0008663C
	public void Reset()
	{
		if (!this.target)
		{
			return;
		}
		this.target.Rebind();
		this.target.Update(0f);
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x00088467 File Offset: 0x00086667
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Reset();
		}
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x00088477 File Offset: 0x00086677
	private void OnDisable()
	{
		if (this.onDisable)
		{
			this.Reset();
		}
	}

	// Token: 0x04001F0F RID: 7951
	public Animator target;

	// Token: 0x04001F10 RID: 7952
	public bool onEnable;

	// Token: 0x04001F11 RID: 7953
	public bool onDisable = true;
}
