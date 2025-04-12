using System;
using GorillaTagScripts;
using TMPro;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
public class BuilderUIResource : MonoBehaviour
{
	// Token: 0x06001EE1 RID: 7905 RVA: 0x000EBCB0 File Offset: 0x000E9EB0
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

	// Token: 0x06001EE2 RID: 7906 RVA: 0x00043DAB File Offset: 0x00041FAB
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

	// Token: 0x04002292 RID: 8850
	public TextMeshPro resourceNameLabel;

	// Token: 0x04002293 RID: 8851
	public TextMeshPro costLabel;

	// Token: 0x04002294 RID: 8852
	public TextMeshPro availableLabel;
}
