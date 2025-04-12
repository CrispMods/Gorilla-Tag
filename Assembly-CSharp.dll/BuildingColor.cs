using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class BuildingColor : MonoBehaviour
{
	// Token: 0x0600043F RID: 1087 RVA: 0x0007AA8C File Offset: 0x00078C8C
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		Color value = new Color(this.Red, this.Green, this.Blue, 1f);
		component.material.SetColor("_Color", value);
	}

	// Token: 0x040004F2 RID: 1266
	[Range(0f, 178f)]
	public float Red;

	// Token: 0x040004F3 RID: 1267
	[Range(0f, 178f)]
	public float Green;

	// Token: 0x040004F4 RID: 1268
	[Range(0f, 178f)]
	public float Blue;
}
