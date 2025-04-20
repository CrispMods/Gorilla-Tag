using System;
using UnityEngine;

// Token: 0x020003B5 RID: 949
public class BitmapFontText : MonoBehaviour
{
	// Token: 0x06001635 RID: 5685 RVA: 0x0003F034 File Offset: 0x0003D234
	private void Awake()
	{
		this.Init();
		this.Render();
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x0003F042 File Offset: 0x0003D242
	public void Render()
	{
		this.font.RenderToTexture(this.texture, this.uppercaseOnly ? this.text.ToUpperInvariant() : this.text);
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x000C1DC0 File Offset: 0x000BFFC0
	public void Init()
	{
		this.texture = new Texture2D(this.textArea.x, this.textArea.y, this.font.fontImage.format, false);
		this.texture.filterMode = FilterMode.Point;
		this.material = new Material(this.renderer.sharedMaterial);
		this.material.mainTexture = this.texture;
		this.renderer.sharedMaterial = this.material;
	}

	// Token: 0x04001861 RID: 6241
	public string text;

	// Token: 0x04001862 RID: 6242
	public bool uppercaseOnly;

	// Token: 0x04001863 RID: 6243
	public Vector2Int textArea;

	// Token: 0x04001864 RID: 6244
	[Space]
	public Renderer renderer;

	// Token: 0x04001865 RID: 6245
	public Texture2D texture;

	// Token: 0x04001866 RID: 6246
	public Material material;

	// Token: 0x04001867 RID: 6247
	public BitmapFont font;
}
