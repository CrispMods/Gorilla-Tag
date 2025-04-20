using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020001F5 RID: 501
public class ReparentOnAwakeWithRenderer : MonoBehaviour, IBuildValidation
{
	// Token: 0x06000BB0 RID: 2992 RVA: 0x00038488 File Offset: 0x00036688
	public bool BuildValidationCheck()
	{
		if (base.GetComponent<MeshRenderer>() != null && this.myRenderer == null)
		{
			Debug.Log("needs a reference to its renderer since it has one");
			return false;
		}
		return true;
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x0009BE24 File Offset: 0x0009A024
	private void OnEnable()
	{
		base.transform.SetParent(this.newParent, true);
		if (this.sortLast)
		{
			base.transform.SetAsLastSibling();
		}
		else
		{
			base.transform.SetAsFirstSibling();
		}
		if (this.myRenderer != null)
		{
			this.myRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			this.myRenderer.lightProbeUsage = LightProbeUsage.CustomProvided;
			this.myRenderer.probeAnchor = this.newParent;
		}
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x000384B3 File Offset: 0x000366B3
	[ContextMenu("Set Renderer")]
	public void SetMyRenderer()
	{
		this.myRenderer = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x04000E43 RID: 3651
	public Transform newParent;

	// Token: 0x04000E44 RID: 3652
	public MeshRenderer myRenderer;

	// Token: 0x04000E45 RID: 3653
	[Tooltip("We're mostly using this for UI elements like text and images, so this will help you separate these in whatever target parent object.Keep images and texts together, otherwise you'll get extra draw calls. Put images above text or they'll overlap weird tho lol")]
	public bool sortLast;
}
