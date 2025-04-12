using System;
using UnityEngine;

// Token: 0x02000428 RID: 1064
public class CosmeticCategoryButton : CosmeticButton
{
	// Token: 0x06001A56 RID: 6742 RVA: 0x00040DBD File Offset: 0x0003EFBD
	public void SetIcon(Sprite sprite)
	{
		this.equippedLeftIcon.enabled = false;
		this.equippedRightIcon.enabled = false;
		this.equippedIcon.enabled = (sprite != null);
		this.equippedIcon.sprite = sprite;
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x000D403C File Offset: 0x000D223C
	public void SetDualIcon(Sprite leftSprite, Sprite rightSprite)
	{
		this.equippedLeftIcon.enabled = (leftSprite != null);
		this.equippedRightIcon.enabled = (rightSprite != null);
		this.equippedIcon.enabled = false;
		this.equippedLeftIcon.sprite = leftSprite;
		this.equippedRightIcon.sprite = rightSprite;
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x000D4094 File Offset: 0x000D2294
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

	// Token: 0x04001D19 RID: 7449
	[SerializeField]
	private SpriteRenderer equippedIcon;

	// Token: 0x04001D1A RID: 7450
	[SerializeField]
	private SpriteRenderer equippedLeftIcon;

	// Token: 0x04001D1B RID: 7451
	[SerializeField]
	private SpriteRenderer equippedRightIcon;
}
