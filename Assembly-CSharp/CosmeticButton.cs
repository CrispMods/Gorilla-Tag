using System;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class CosmeticButton : GorillaPressableButton
{
	// Token: 0x06001A4F RID: 6735 RVA: 0x000816FB File Offset: 0x0007F8FB
	public void Awake()
	{
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x00081710 File Offset: 0x0007F910
	public override void UpdateColor()
	{
		if (!base.enabled)
		{
			this.buttonRenderer.material = this.disabledMaterial;
			if (this.myText != null)
			{
				this.myText.text = this.offText;
			}
		}
		else if (this.isOn)
		{
			this.buttonRenderer.material = this.pressedMaterial;
			if (this.myText != null)
			{
				this.myText.text = this.onText;
			}
		}
		else
		{
			this.buttonRenderer.material = this.unpressedMaterial;
			if (this.myText != null)
			{
				this.myText.text = this.offText;
			}
		}
		this.UpdatePosition();
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x000817C8 File Offset: 0x0007F9C8
	public virtual void UpdatePosition()
	{
		Vector3 vector = this.startingPos;
		if (!base.enabled)
		{
			vector += this.disabledOffset;
		}
		else if (this.isOn)
		{
			vector += this.pressedOffset;
		}
		this.posOffset = base.transform.position;
		base.transform.localPosition = vector;
		this.posOffset = base.transform.position - this.posOffset;
		if (this.myText != null)
		{
			this.myText.transform.position += this.posOffset;
		}
		if (this.myTmpText != null)
		{
			this.myTmpText.transform.position += this.posOffset;
		}
		if (this.myTmpText2 != null)
		{
			this.myTmpText2.transform.position += this.posOffset;
		}
	}

	// Token: 0x04001D13 RID: 7443
	[SerializeField]
	private Vector3 pressedOffset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04001D14 RID: 7444
	[SerializeField]
	private Material disabledMaterial;

	// Token: 0x04001D15 RID: 7445
	[SerializeField]
	private Vector3 disabledOffset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04001D16 RID: 7446
	private Vector3 startingPos;

	// Token: 0x04001D17 RID: 7447
	protected Vector3 posOffset;
}
