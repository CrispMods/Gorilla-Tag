using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020001EA RID: 490
public class ReparentOnAwakeWithRenderer : MonoBehaviour, IBuildValidation
{
	// Token: 0x06000B64 RID: 2916 RVA: 0x0003D0DB File Offset: 0x0003B2DB
	public bool BuildValidationCheck()
	{
		if (base.GetComponent<MeshRenderer>() != null && this.myRenderer == null)
		{
			Debug.Log("needs a reference to its renderer since it has one");
			return false;
		}
		return true;
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0003D108 File Offset: 0x0003B308
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

	// Token: 0x06000B66 RID: 2918 RVA: 0x0003D17E File Offset: 0x0003B37E
	[ContextMenu("Set Renderer")]
	public void SetMyRenderer()
	{
		this.myRenderer = base.GetComponent<MeshRenderer>();
	}

	// Token: 0x04000DFD RID: 3581
	public Transform newParent;

	// Token: 0x04000DFE RID: 3582
	public MeshRenderer myRenderer;

	// Token: 0x04000DFF RID: 3583
	[Tooltip("We're mostly using this for UI elements like text and images, so this will help you separate these in whatever target parent object.Keep images and texts together, otherwise you'll get extra draw calls. Put images above text or they'll overlap weird tho lol")]
	public bool sortLast;
}
