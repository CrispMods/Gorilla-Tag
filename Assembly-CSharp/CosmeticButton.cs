using System;
using UnityEngine;

// Token: 0x02000433 RID: 1075
public class CosmeticButton : GorillaPressableButton
{
	// Token: 0x06001AA3 RID: 6819 RVA: 0x000420A7 File Offset: 0x000402A7
	public void Awake()
	{
		this.startingPos = base.transform.localPosition;
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x000D6B34 File Offset: 0x000D4D34
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

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000D6BEC File Offset: 0x000D4DEC
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

	// Token: 0x04001D62 RID: 7522
	[SerializeField]
	private Vector3 pressedOffset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04001D63 RID: 7523
	[SerializeField]
	private Material disabledMaterial;

	// Token: 0x04001D64 RID: 7524
	[SerializeField]
	private Vector3 disabledOffset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04001D65 RID: 7525
	private Vector3 startingPos;

	// Token: 0x04001D66 RID: 7526
	protected Vector3 posOffset;
}
