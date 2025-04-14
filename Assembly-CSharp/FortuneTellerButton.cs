using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200052C RID: 1324
public class FortuneTellerButton : GorillaPressableButton
{
	// Token: 0x0600200E RID: 8206 RVA: 0x000A19D1 File Offset: 0x0009FBD1
	public void Awake()
	{
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x0600200F RID: 8207 RVA: 0x000A19E4 File Offset: 0x0009FBE4
	public override void ButtonActivation()
	{
		this.PressButtonUpdate();
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x000A19EC File Offset: 0x0009FBEC
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

	// Token: 0x06002012 RID: 8210 RVA: 0x000A1A79 File Offset: 0x0009FC79
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

	// Token: 0x04002420 RID: 9248
	[SerializeField]
	private float durationPressed = 0.25f;

	// Token: 0x04002421 RID: 9249
	[SerializeField]
	private Vector3 pressedOffset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04002422 RID: 9250
	private float pressTime;

	// Token: 0x04002423 RID: 9251
	private Vector3 startingPos;
}
