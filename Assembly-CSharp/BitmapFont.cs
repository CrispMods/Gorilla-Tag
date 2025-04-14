using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003A7 RID: 935
public class BitmapFont : ScriptableObject
{
	// Token: 0x060015E2 RID: 5602 RVA: 0x00069BAC File Offset: 0x00067DAC
	private void OnEnable()
	{
		this._charToSymbol = this.symbols.ToDictionary((BitmapFont.SymbolData s) => s.character, (BitmapFont.SymbolData s) => s);
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x00069C08 File Offset: 0x00067E08
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

	// Token: 0x04001808 RID: 6152
	public Texture2D fontImage;

	// Token: 0x04001809 RID: 6153
	public TextAsset fontJson;

	// Token: 0x0400180A RID: 6154
	public int symbolPixelsPerUnit = 1;

	// Token: 0x0400180B RID: 6155
	public string characterMap;

	// Token: 0x0400180C RID: 6156
	[Space]
	public BitmapFont.SymbolData[] symbols = new BitmapFont.SymbolData[0];

	// Token: 0x0400180D RID: 6157
	private Dictionary<char, BitmapFont.SymbolData> _charToSymbol;

	// Token: 0x0400180E RID: 6158
	private Color[] _empty = new Color[0];

	// Token: 0x020003A8 RID: 936
	[Serializable]
	public struct SymbolData
	{
		// Token: 0x0400180F RID: 6159
		public char character;

		// Token: 0x04001810 RID: 6160
		[Space]
		public int id;

		// Token: 0x04001811 RID: 6161
		public int width;

		// Token: 0x04001812 RID: 6162
		public int height;

		// Token: 0x04001813 RID: 6163
		public int x;

		// Token: 0x04001814 RID: 6164
		public int y;

		// Token: 0x04001815 RID: 6165
		public int xadvance;

		// Token: 0x04001816 RID: 6166
		public int yoffset;
	}
}
