using System;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class PassthroughSurface : MonoBehaviour
{
	// Token: 0x0600135C RID: 4956 RVA: 0x0003D298 File Offset: 0x0003B498
	private void Start()
	{
		UnityEngine.Object.Destroy(this.projectionObject.GetComponent<MeshRenderer>());
		this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, true);
	}

	// Token: 0x0400155A RID: 5466
	public OVRPassthroughLayer passthroughLayer;

	// Token: 0x0400155B RID: 5467
	public MeshFilter projectionObject;
}
