using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000246 RID: 582
[ExecuteInEditMode]
public class LckRawImageFillCanvas : UIBehaviour
{
	// Token: 0x06000D6F RID: 3439 RVA: 0x00039867 File Offset: 0x00037A67
	private new void OnEnable()
	{
		this.UpdateSizeDelta();
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x00039867 File Offset: 0x00037A67
	private void Update()
	{
		this.UpdateSizeDelta();
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x000A1E18 File Offset: 0x000A0018
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

	// Token: 0x040010B2 RID: 4274
	[SerializeField]
	private RawImage _rawImage;

	// Token: 0x040010B3 RID: 4275
	[SerializeField]
	private LckRawImageFillCanvas.ScaleType _scaleType;

	// Token: 0x02000247 RID: 583
	private enum ScaleType
	{
		// Token: 0x040010B5 RID: 4277
		Fill,
		// Token: 0x040010B6 RID: 4278
		Inset,
		// Token: 0x040010B7 RID: 4279
		Stretch
	}
}
