using System;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class PassthroughSurface : MonoBehaviour
{
	// Token: 0x06001310 RID: 4880 RVA: 0x0005CF83 File Offset: 0x0005B183
	private void Start()
	{
		Object.Destroy(this.projectionObject.GetComponent<MeshRenderer>());
		this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, true);
	}

	// Token: 0x04001512 RID: 5394
	public OVRPassthroughLayer passthroughLayer;

	// Token: 0x04001513 RID: 5395
	public MeshFilter projectionObject;
}
