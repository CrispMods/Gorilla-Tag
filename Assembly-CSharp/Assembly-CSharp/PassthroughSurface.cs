using System;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class PassthroughSurface : MonoBehaviour
{
	// Token: 0x06001313 RID: 4883 RVA: 0x0005D307 File Offset: 0x0005B507
	private void Start()
	{
		Object.Destroy(this.projectionObject.GetComponent<MeshRenderer>());
		this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, true);
	}

	// Token: 0x04001513 RID: 5395
	public OVRPassthroughLayer passthroughLayer;

	// Token: 0x04001514 RID: 5396
	public MeshFilter projectionObject;
}
