using System;
using UnityEngine;

// Token: 0x02000428 RID: 1064
public class CosmeticCategoryButton : CosmeticButton
{
	// Token: 0x06001A53 RID: 6739 RVA: 0x0008190A File Offset: 0x0007FB0A
	public void SetIcon(Sprite sprite)
	{
		this.equippedLeftIcon.enabled = false;
		this.equippedRightIcon.enabled = false;
		this.equippedIcon.enabled = (sprite != null);
		this.equippedIcon.sprite = sprite;
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x00081944 File Offset: 0x0007FB44
	public void SetDualIcon(Sprite leftSprite, Sprite rightSprite)
	{
		this.equippedLeftIcon.enabled = (leftSprite != null);
		this.equippedRightIcon.enabled = (rightSprite != null);
		this.equippedIcon.enabled = false;
		this.equippedLeftIcon.sprite = leftSprite;
		this.equippedRightIcon.sprite = rightSprite;
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x0008199C File Offset: 0x0007FB9C
	public override void UpdatePosition()
	{
		base.UpdatePosition();
		if (this.equippedIcon != null)
		{
			this.equippedIcon.transform.position += this.posOffset;
		}
		if (this.equippedLeftIcon != null)
		{
			this.equippedLeftIcon.transform.position += this.posOffset;
		}
		if (this.equippedRightIcon != null)
		{
			this.equippedRightIcon.transform.position += this.posOffset;
		}
	}

	// Token: 0x04001D18 RID: 7448
	[SerializeField]
	private SpriteRenderer equippedIcon;

	// Token: 0x04001D19 RID: 7449
	[SerializeField]
	private SpriteRenderer equippedLeftIcon;

	// Token: 0x04001D1A RID: 7450
	[SerializeField]
	private SpriteRenderer equippedRightIcon;
}
