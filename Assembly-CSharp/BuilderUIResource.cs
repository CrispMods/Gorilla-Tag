using System;
using GorillaTagScripts;
using TMPro;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class BuilderUIResource : MonoBehaviour
{
	// Token: 0x06001F37 RID: 7991 RVA: 0x000EE9EC File Offset: 0x000ECBEC
	public void SetResourceCost(BuilderResourceQuantity resourceCost)
	{
		BuilderResourceType type = resourceCost.type;
		int count = resourceCost.count;
		int availableResources = BuilderTable.instance.GetAvailableResources(type);
		if (this.resourceNameLabel != null)
		{
			this.resourceNameLabel.text = this.GetResourceName(type);
		}
		if (this.costLabel != null)
		{
			this.costLabel.text = count.ToString();
		}
		if (this.availableLabel != null)
		{
			this.availableLabel.text = availableResources.ToString();
		}
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x0004514A File Offset: 0x0004334A
	private string GetResourceName(BuilderResourceType type)
	{
		switch (type)
		{
		case BuilderResourceType.Basic:
			return "Basic";
		case BuilderResourceType.Decorative:
			return "Decorative";
		case BuilderResourceType.Functional:
			return "Functional";
		default:
			return "Resource Needs Name";
		}
	}

	// Token: 0x040022E4 RID: 8932
	public TextMeshPro resourceNameLabel;

	// Token: 0x040022E5 RID: 8933
	public TextMeshPro costLabel;

	// Token: 0x040022E6 RID: 8934
	public TextMeshPro availableLabel;
}
