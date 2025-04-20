using System;
using Sirenix.OdinInspector;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class CritterSpawnTrigger : MonoBehaviour
{
	// Token: 0x06000260 RID: 608 RVA: 0x00031D33 File Offset: 0x0002FF33
	private ValueDropdownList<int> GetCritterTypeList()
	{
		return new ValueDropdownList<int>();
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00073C48 File Offset: 0x00071E48
	private void OnTriggerEnter(Collider other)
	{
		if (!CrittersManager.instance.LocalAuthority())
		{
			return;
		}
		if (Time.realtimeSinceStartup < this._nextSpawnTime)
		{
			return;
		}
		CrittersActor componentInParent = other.GetComponentInParent<CrittersActor>();
		if (!componentInParent)
		{
			return;
		}
		if (componentInParent.crittersActorType != this.triggerActorType)
		{
			return;
		}
		if (this.requiredSubObjectIndex >= 0 && componentInParent.subObjectIndex != this.requiredSubObjectIndex)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.triggerActorName) && !componentInParent.GetActorSubtype().Contains(this.triggerActorName))
		{
			return;
		}
		CrittersManager.instance.DespawnActor(componentInParent);
		CrittersManager.instance.SpawnCritter(this.critterType, this.spawnPoint.position, this.spawnPoint.rotation);
		this._nextSpawnTime = Time.realtimeSinceStartup + this.triggerCooldown;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00031D3A File Offset: 0x0002FF3A
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(base.transform.position, this.spawnPoint.position);
		Gizmos.DrawWireSphere(this.spawnPoint.position, 0.1f);
	}

	// Token: 0x040002EA RID: 746
	[Header("Trigger Settings")]
	[SerializeField]
	private CrittersActor.CrittersActorType triggerActorType;

	// Token: 0x040002EB RID: 747
	[SerializeField]
	private int requiredSubObjectIndex = -1;

	// Token: 0x040002EC RID: 748
	[SerializeField]
	private string triggerActorName;

	// Token: 0x040002ED RID: 749
	[SerializeField]
	private float triggerCooldown = 1f;

	// Token: 0x040002EE RID: 750
	[Header("Spawn Settings")]
	[SerializeField]
	private Transform spawnPoint;

	// Token: 0x040002EF RID: 751
	[SerializeField]
	private int critterType;

	// Token: 0x040002F0 RID: 752
	private float _nextSpawnTime;
}
