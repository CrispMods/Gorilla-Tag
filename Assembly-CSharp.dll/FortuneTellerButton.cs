using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200052C RID: 1324
public class FortuneTellerButton : GorillaPressableButton
{
	// Token: 0x06002011 RID: 8209 RVA: 0x00044D63 File Offset: 0x00042F63
	public void Awake()
	{
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x00044D76 File Offset: 0x00042F76
	public override void ButtonActivation()
	{
		this.PressButtonUpdate();
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x000F00D4 File Offset: 0x000EE2D4
	public void PressButtonUpdate()
	{
		if (this.pressTime != 0f)
		{
			return;
		}
		base.transform.localPosition = this.startingPos + this.pressedOffset;
		this.buttonRenderer.material = this.pressedMaterial;
		this.pressTime = Time.time;
		base.StartCoroutine(this.<PressButtonUpdate>g__ButtonColorUpdate_Local|6_0());
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x00044DAB File Offset: 0x00042FAB
	[CompilerGenerated]
	private IEnumerator <PressButtonUpdate>g__ButtonColorUpdate_Local|6_0()
	{
		yield return new WaitForSeconds(this.durationPressed);
		if (this.pressTime != 0f && Time.time > this.durationPressed + this.pressTime)
		{
			base.transform.localPosition = this.startingPos;
			this.buttonRenderer.material = this.unpressedMaterial;
			this.pressTime = 0f;
		}
		yield break;
	}

	// Token: 0x04002421 RID: 9249
	[SerializeField]
	private float durationPressed = 0.25f;

	// Token: 0x04002422 RID: 9250
	[SerializeField]
	private Vector3 pressedOffset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04002423 RID: 9251
	private float pressTime;

	// Token: 0x04002424 RID: 9252
	private Vector3 startingPos;
}
