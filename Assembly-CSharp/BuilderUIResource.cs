using System;
using GorillaTagScripts;
using TMPro;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
public class BuilderUIResource : MonoBehaviour
{
	// Token: 0x06001EDE RID: 7902 RVA: 0x0009C5D8 File Offset: 0x0009A7D8
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

	// Token: 0x06001EDF RID: 7903 RVA: 0x0009C65F File Offset: 0x0009A85F
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

	// Token: 0x04002291 RID: 8849
	public TextMeshPro resourceNameLabel;

	// Token: 0x04002292 RID: 8850
	public TextMeshPro costLabel;

	// Token: 0x04002293 RID: 8851
	public TextMeshPro availableLabel;
}
