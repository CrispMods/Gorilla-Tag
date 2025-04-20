using System;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class BuildingColor : MonoBehaviour
{
	// Token: 0x06000479 RID: 1145 RVA: 0x0007D2E8 File Offset: 0x0007B4E8
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		Color value = new Color(this.Red, this.Green, this.Blue, 1f);
		component.material.SetColor("_Color", value);
	}

	// Token: 0x04000531 RID: 1329
	[Range(0f, 178f)]
	public float Red;

	// Token: 0x04000532 RID: 1330
	[Range(0f, 178f)]
	public float Green;

	// Token: 0x04000533 RID: 1331
	[Range(0f, 178f)]
	public float Blue;
}
