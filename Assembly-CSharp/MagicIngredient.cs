using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005D6 RID: 1494
[Obsolete("replaced with ThrowableSetDressing.cs")]
public class MagicIngredient : TransferrableObject
{
	// Token: 0x0600250F RID: 9487 RVA: 0x000B7F50 File Offset: 0x000B6150
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.item = this.worldShareableInstance;
		this.grabPtInitParent = this.anchor.transform.parent;
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000B7F7C File Offset: 0x000B617C
	private void ReParent()
	{
		Transform transform = this.anchor.transform;
		base.gameObject.transform.parent = transform;
		transform.parent = this.grabPtInitParent;
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000B7FB2 File Offset: 0x000B61B2
	public void Disable()
	{
		this.DropItem();
		base.OnDisable();
		if (this.item)
		{
			this.item.OnDisable();
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04002939 RID: 10553
	[FormerlySerializedAs("IngredientType")]
	public MagicIngredientType IngredientTypeSO;

	// Token: 0x0400293A RID: 10554
	public Transform rootParent;

	// Token: 0x0400293B RID: 10555
	private WorldShareableItem item;

	// Token: 0x0400293C RID: 10556
	private Transform grabPtInitParent;
}
