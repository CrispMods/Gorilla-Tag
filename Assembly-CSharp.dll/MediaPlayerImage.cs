﻿using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002F2 RID: 754
public class MediaPlayerImage : Image
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06001212 RID: 4626 RVA: 0x0003B6CA File Offset: 0x000398CA
	// (set) Token: 0x06001213 RID: 4627 RVA: 0x0003B6D2 File Offset: 0x000398D2
	public MediaPlayerImage.ButtonType buttonType
	{
		get
		{
			return this.m_ButtonType;
		}
		set
		{
			if (this.m_ButtonType != value)
			{
				this.m_ButtonType = value;
				this.SetAllDirty();
			}
		}
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x000AD410 File Offset: 0x000AB610
	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
		Vector4 vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
		Color32 color = this.color;
		toFill.Clear();
		switch (this.m_ButtonType)
		{
		case MediaPlayerImage.ButtonType.Play:
			toFill.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
			toFill.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
			toFill.AddVert(new Vector3(vector.z, Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(1f, 0.5f));
			toFill.AddTriangle(0, 1, 2);
			return;
		case MediaPlayerImage.ButtonType.Pause:
			toFill.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
			toFill.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.35f), vector.w), color, new Vector2(0.35f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.35f), vector.y), color, new Vector2(0.35f, 0f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.65f), vector.y), color, new Vector2(0.65f, 0f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.65f), vector.w), color, new Vector2(0.65f, 1f));
			toFill.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(1f, 1f));
			toFill.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(1f, 0f));
			toFill.AddTriangle(0, 1, 2);
			toFill.AddTriangle(2, 3, 0);
			toFill.AddTriangle(4, 5, 6);
			toFill.AddTriangle(6, 7, 4);
			return;
		case MediaPlayerImage.ButtonType.FastForward:
			toFill.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
			toFill.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5f), Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(0.5f, 0.5f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5f), vector.y), color, new Vector2(0.5f, 0f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5f), vector.w), color, new Vector2(0.5f, 1f));
			toFill.AddVert(new Vector3(vector.z, Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(1f, 0.5f));
			toFill.AddTriangle(0, 1, 2);
			toFill.AddTriangle(3, 4, 5);
			return;
		case MediaPlayerImage.ButtonType.Rewind:
			toFill.AddVert(new Vector3(vector.x, Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(0f, 0.5f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5f), vector.w), color, new Vector2(0.5f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5f), vector.y), color, new Vector2(0.5f, 0f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5f), Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(0.5f, 0.5f));
			toFill.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(1f, 1f));
			toFill.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(1f, 0f));
			toFill.AddTriangle(0, 1, 2);
			toFill.AddTriangle(3, 4, 5);
			return;
		case MediaPlayerImage.ButtonType.SkipForward:
			toFill.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
			toFill.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.4375f), Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(0.4375f, 0.5f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.4375f), vector.y), color, new Vector2(0.4375f, 0f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.4375f), vector.w), color, new Vector2(0.4375f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.875f), Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(0.875f, 0.5f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.875f), vector.y), color, new Vector2(0.875f, 0f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.875f), vector.w), color, new Vector2(0.875f, 1f));
			toFill.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(1f, 1f));
			toFill.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(1f, 0f));
			toFill.AddTriangle(0, 1, 2);
			toFill.AddTriangle(3, 4, 5);
			toFill.AddTriangle(6, 7, 8);
			toFill.AddTriangle(8, 9, 6);
			return;
		case MediaPlayerImage.ButtonType.SkipBack:
			toFill.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
			toFill.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.125f), vector.w), color, new Vector2(0.125f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.125f), vector.y), color, new Vector2(0.125f, 0f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.125f), Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(0.125f, 0.5f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5625f), vector.w), color, new Vector2(0.5625f, 1f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5625f), vector.y), color, new Vector2(0.5625f, 0f));
			toFill.AddVert(new Vector3(Mathf.Lerp(vector.x, vector.z, 0.5625f), Mathf.Lerp(vector.y, vector.w, 0.5f)), color, new Vector2(0.5625f, 0.5f));
			toFill.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(1f, 1f));
			toFill.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(1f, 0f));
			toFill.AddTriangle(0, 1, 2);
			toFill.AddTriangle(2, 3, 0);
			toFill.AddTriangle(4, 5, 6);
			toFill.AddTriangle(7, 8, 9);
			return;
		}
		toFill.AddVert(new Vector3(vector.x, vector.y), color, new Vector2(0f, 0f));
		toFill.AddVert(new Vector3(vector.x, vector.w), color, new Vector2(0f, 1f));
		toFill.AddVert(new Vector3(vector.z, vector.w), color, new Vector2(1f, 1f));
		toFill.AddVert(new Vector3(vector.z, vector.y), color, new Vector2(1f, 0f));
		toFill.AddTriangle(0, 1, 2);
		toFill.AddTriangle(2, 3, 0);
	}

	// Token: 0x040013EE RID: 5102
	[SerializeField]
	private MediaPlayerImage.ButtonType m_ButtonType;

	// Token: 0x020002F3 RID: 755
	public enum ButtonType
	{
		// Token: 0x040013F0 RID: 5104
		Play,
		// Token: 0x040013F1 RID: 5105
		Pause,
		// Token: 0x040013F2 RID: 5106
		FastForward,
		// Token: 0x040013F3 RID: 5107
		Rewind,
		// Token: 0x040013F4 RID: 5108
		SkipForward,
		// Token: 0x040013F5 RID: 5109
		SkipBack,
		// Token: 0x040013F6 RID: 5110
		Stop
	}
}
