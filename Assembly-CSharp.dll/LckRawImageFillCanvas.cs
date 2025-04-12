using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200023B RID: 571
[ExecuteInEditMode]
public class LckRawImageFillCanvas : UIBehaviour
{
	// Token: 0x06000D26 RID: 3366 RVA: 0x000385A7 File Offset: 0x000367A7
	private new void OnEnable()
	{
		this.UpdateSizeDelta();
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x000385A7 File Offset: 0x000367A7
	private void Update()
	{
		this.UpdateSizeDelta();
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x0009F58C File Offset: 0x0009D78C
	private void UpdateSizeDelta()
	{
		if (this._rawImage == null || this._rawImage.texture == null)
		{
			return;
		}
		RectTransform rectTransform = this._rawImage.rectTransform;
		Vector2 sizeDelta = ((RectTransform)rectTransform.parent).sizeDelta;
		Vector2 vector = new Vector2((float)this._rawImage.texture.width, (float)this._rawImage.texture.height);
		float num = sizeDelta.x / sizeDelta.y;
		float num2 = vector.x / vector.y;
		float num3 = num / num2;
		Vector2 vector2 = new Vector2(sizeDelta.x, sizeDelta.x / num2);
		Vector2 vector3 = new Vector2(sizeDelta.y * num2, sizeDelta.y);
		switch (this._scaleType)
		{
		case LckRawImageFillCanvas.ScaleType.Fill:
			rectTransform.sizeDelta = ((num3 > 1f) ? vector2 : vector3);
			return;
		case LckRawImageFillCanvas.ScaleType.Inset:
			rectTransform.sizeDelta = ((num3 < 1f) ? vector2 : vector3);
			return;
		case LckRawImageFillCanvas.ScaleType.Stretch:
			rectTransform.sizeDelta = sizeDelta;
			return;
		default:
			return;
		}
	}

	// Token: 0x0400106D RID: 4205
	[SerializeField]
	private RawImage _rawImage;

	// Token: 0x0400106E RID: 4206
	[SerializeField]
	private LckRawImageFillCanvas.ScaleType _scaleType;

	// Token: 0x0200023C RID: 572
	private enum ScaleType
	{
		// Token: 0x04001070 RID: 4208
		Fill,
		// Token: 0x04001071 RID: 4209
		Inset,
		// Token: 0x04001072 RID: 4210
		Stretch
	}
}
