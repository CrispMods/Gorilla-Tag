using System;
using UnityEngine;

// Token: 0x020003AA RID: 938
public class BitmapFontText : MonoBehaviour
{
	// Token: 0x060015E9 RID: 5609 RVA: 0x00069D5A File Offset: 0x00067F5A
	private void Awake()
	{
		this.Init();
		this.Render();
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x00069D68 File Offset: 0x00067F68
	public void Render()
	{
		this.font.RenderToTexture(this.texture, this.uppercaseOnly ? this.text.ToUpperInvariant() : this.text);
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x00069D98 File Offset: 0x00067F98
	public void Init()
	{
		this.texture = new Texture2D(this.textArea.x, this.textArea.y, this.font.fontImage.format, false);
		this.texture.filterMode = FilterMode.Point;
		this.material = new Material(this.renderer.sharedMaterial);
		this.material.mainTexture = this.texture;
		this.renderer.sharedMaterial = this.material;
	}

	// Token: 0x0400181A RID: 6170
	public string text;

	// Token: 0x0400181B RID: 6171
	public bool uppercaseOnly;

	// Token: 0x0400181C RID: 6172
	public Vector2Int textArea;

	// Token: 0x0400181D RID: 6173
	[Space]
	public Renderer renderer;

	// Token: 0x0400181E RID: 6174
	public Texture2D texture;

	// Token: 0x0400181F RID: 6175
	public Material material;

	// Token: 0x04001820 RID: 6176
	public BitmapFont font;
}
