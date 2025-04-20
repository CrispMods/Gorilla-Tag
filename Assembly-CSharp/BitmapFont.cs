using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003B2 RID: 946
public class BitmapFont : ScriptableObject
{
	// Token: 0x0600162E RID: 5678 RVA: 0x000C1C50 File Offset: 0x000BFE50
	private void OnEnable()
	{
		this._charToSymbol = this.symbols.ToDictionary((BitmapFont.SymbolData s) => s.character, (BitmapFont.SymbolData s) => s);
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x000C1CAC File Offset: 0x000BFEAC
	public void RenderToTexture(Texture2D target, string text)
	{
		if (text == null)
		{
			text = string.Empty;
		}
		int num = target.width * target.height;
		if (this._empty.Length != num)
		{
			this._empty = new Color[num];
			for (int i = 0; i < this._empty.Length; i++)
			{
				this._empty[i] = Color.black;
			}
		}
		target.SetPixels(this._empty);
		int length = text.Length;
		int num2 = 1;
		int width = this.fontImage.width;
		int height = this.fontImage.height;
		for (int j = 0; j < length; j++)
		{
			char key = text[j];
			BitmapFont.SymbolData symbolData = this._charToSymbol[key];
			int width2 = symbolData.width;
			int height2 = symbolData.height;
			int x = symbolData.x;
			int y = symbolData.y;
			Graphics.CopyTexture(this.fontImage, 0, 0, x, height - (y + height2), width2, height2, target, 0, 0, num2, 2 + symbolData.yoffset);
			num2 += width2 + 1;
		}
		target.Apply(false);
	}

	// Token: 0x0400184F RID: 6223
	public Texture2D fontImage;

	// Token: 0x04001850 RID: 6224
	public TextAsset fontJson;

	// Token: 0x04001851 RID: 6225
	public int symbolPixelsPerUnit = 1;

	// Token: 0x04001852 RID: 6226
	public string characterMap;

	// Token: 0x04001853 RID: 6227
	[Space]
	public BitmapFont.SymbolData[] symbols = new BitmapFont.SymbolData[0];

	// Token: 0x04001854 RID: 6228
	private Dictionary<char, BitmapFont.SymbolData> _charToSymbol;

	// Token: 0x04001855 RID: 6229
	private Color[] _empty = new Color[0];

	// Token: 0x020003B3 RID: 947
	[Serializable]
	public struct SymbolData
	{
		// Token: 0x04001856 RID: 6230
		public char character;

		// Token: 0x04001857 RID: 6231
		[Space]
		public int id;

		// Token: 0x04001858 RID: 6232
		public int width;

		// Token: 0x04001859 RID: 6233
		public int height;

		// Token: 0x0400185A RID: 6234
		public int x;

		// Token: 0x0400185B RID: 6235
		public int y;

		// Token: 0x0400185C RID: 6236
		public int xadvance;

		// Token: 0x0400185D RID: 6237
		public int yoffset;
	}
}
