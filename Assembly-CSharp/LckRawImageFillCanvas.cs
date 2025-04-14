using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200023B RID: 571
[ExecuteInEditMode]
public class LckRawImageFillCanvas : UIBehaviour
{
	// Token: 0x06000D24 RID: 3364 RVA: 0x00044503 File Offset: 0x00042703
	private new void OnEnable()
	{
		this.UpdateSizeDelta();
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x00044503 File Offset: 0x00042703
	private void Update()
	{
		this.UpdateSizeDelta();
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x0004450C File Offset: 0x0004270C
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

	// Token: 0x0400106C RID: 4204
	[SerializeField]
	private RawImage _rawImage;

	// Token: 0x0400106D RID: 4205
	[SerializeField]
	private LckRawImageFillCanvas.ScaleType _scaleType;

	// Token: 0x0200023C RID: 572
	private enum ScaleType
	{
		// Token: 0x0400106F RID: 4207
		Fill,
		// Token: 0x04001070 RID: 4208
		Inset,
		// Token: 0x04001071 RID: 4209
		Stretch
	}
}
