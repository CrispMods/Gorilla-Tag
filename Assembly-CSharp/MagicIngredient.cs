using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005E4 RID: 1508
[Obsolete("replaced with ThrowableSetDressing.cs")]
public class MagicIngredient : TransferrableObject
{
	// Token: 0x06002571 RID: 9585 RVA: 0x00049604 File Offset: 0x00047804
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.item = this.worldShareableInstance;
		this.grabPtInitParent = this.anchor.transform.parent;
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x00106194 File Offset: 0x00104394
	private void ReParent()
	{
		Transform transform = this.anchor.transform;
		base.gameObject.transform.parent = transform;
		transform.parent = this.grabPtInitParent;
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x0004962F File Offset: 0x0004782F
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

	// Token: 0x04002998 RID: 10648
	[FormerlySerializedAs("IngredientType")]
	public MagicIngredientType IngredientTypeSO;

	// Token: 0x04002999 RID: 10649
	public Transform rootParent;

	// Token: 0x0400299A RID: 10650
	private WorldShareableItem item;

	// Token: 0x0400299B RID: 10651
	private Transform grabPtInitParent;
}
