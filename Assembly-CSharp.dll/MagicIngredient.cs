using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005D7 RID: 1495
[Obsolete("replaced with ThrowableSetDressing.cs")]
public class MagicIngredient : TransferrableObject
{
	// Token: 0x06002517 RID: 9495 RVA: 0x000481E9 File Offset: 0x000463E9
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.item = this.worldShareableInstance;
		this.grabPtInitParent = this.anchor.transform.parent;
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x001032B0 File Offset: 0x001014B0
	private void ReParent()
	{
		Transform transform = this.anchor.transform;
		base.gameObject.transform.parent = transform;
		transform.parent = this.grabPtInitParent;
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x00048214 File Offset: 0x00046414
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

	// Token: 0x0400293F RID: 10559
	[FormerlySerializedAs("IngredientType")]
	public MagicIngredientType IngredientTypeSO;

	// Token: 0x04002940 RID: 10560
	public Transform rootParent;

	// Token: 0x04002941 RID: 10561
	private WorldShareableItem item;

	// Token: 0x04002942 RID: 10562
	private Transform grabPtInitParent;
}
