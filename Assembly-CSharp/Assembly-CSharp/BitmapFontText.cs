using System;
using UnityEngine;

// Token: 0x020003AA RID: 938
public class BitmapFontText : MonoBehaviour
{
	// Token: 0x060015EC RID: 5612 RVA: 0x0006A0DE File Offset: 0x000682DE
	private void Awake()
	{
		this.Init();
		this.Render();
	}

	// Token: 0x060015ED RID: 5613 RVA: 0x0006A0EC File Offset: 0x000682EC
	public void Render()
	{
		this.font.RenderToTexture(this.texture, this.uppercaseOnly ? this.text.ToUpperInvariant() : this.text);
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x0006A11C File Offset: 0x0006831C
	public void Init()
	{
		this.texture = new Texture2D(this.textArea.x, this.textArea.y, this.font.fontImage.format, false);
		this.texture.filterMode = FilterMode.Point;
		this.material = new Material(this.renderer.sharedMaterial);
		this.material.mainTexture = this.texture;
		this.renderer.sharedMaterial = this.material;
	}

	// Token: 0x0400181B RID: 6171
	public string text;

	// Token: 0x0400181C RID: 6172
	public bool uppercaseOnly;

	// Token: 0x0400181D RID: 6173
	public Vector2Int textArea;

	// Token: 0x0400181E RID: 6174
	[Space]
	public Renderer renderer;

	// Token: 0x0400181F RID: 6175
	public Texture2D texture;

	// Token: 0x04001820 RID: 6176
	public Material material;

	// Token: 0x04001821 RID: 6177
	public BitmapFont font;
}
