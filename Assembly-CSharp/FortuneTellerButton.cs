using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000539 RID: 1337
public class FortuneTellerButton : GorillaPressableButton
{
	// Token: 0x06002067 RID: 8295 RVA: 0x00046102 File Offset: 0x00044302
	public void Awake()
	{
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x00046115 File Offset: 0x00044315
	public override void ButtonActivation()
	{
		this.PressButtonUpdate();
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x000F2E58 File Offset: 0x000F1058
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

	// Token: 0x0600206B RID: 8299 RVA: 0x0004614A File Offset: 0x0004434A
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

	// Token: 0x04002473 RID: 9331
	[SerializeField]
	private float durationPressed = 0.25f;

	// Token: 0x04002474 RID: 9332
	[SerializeField]
	private Vector3 pressedOffset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04002475 RID: 9333
	private float pressTime;

	// Token: 0x04002476 RID: 9334
	private Vector3 startingPos;
}
