using System;
using UnityEngine;

// Token: 0x02000434 RID: 1076
public class CosmeticCategoryButton : CosmeticButton
{
	// Token: 0x06001AA7 RID: 6823 RVA: 0x000420F6 File Offset: 0x000402F6
	public void SetIcon(Sprite sprite)
	{
		this.equippedLeftIcon.enabled = false;
		this.equippedRightIcon.enabled = false;
		this.equippedIcon.enabled = (sprite != null);
		this.equippedIcon.sprite = sprite;
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000D6CF4 File Offset: 0x000D4EF4
	public void SetDualIcon(Sprite leftSprite, Sprite rightSprite)
	{
		this.equippedLeftIcon.enabled = (leftSprite != null);
		this.equippedRightIcon.enabled = (rightSprite != null);
		this.equippedIcon.enabled = false;
		this.equippedLeftIcon.sprite = leftSprite;
		this.equippedRightIcon.sprite = rightSprite;
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000D6D4C File Offset: 0x000D4F4C
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

	// Token: 0x04001D67 RID: 7527
	[SerializeField]
	private SpriteRenderer equippedIcon;

	// Token: 0x04001D68 RID: 7528
	[SerializeField]
	private SpriteRenderer equippedLeftIcon;

	// Token: 0x04001D69 RID: 7529
	[SerializeField]
	private SpriteRenderer equippedRightIcon;
}
