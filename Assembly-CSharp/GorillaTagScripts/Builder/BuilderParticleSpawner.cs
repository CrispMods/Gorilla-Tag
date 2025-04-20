using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A27 RID: 2599
	public class BuilderParticleSpawner : MonoBehaviour
	{
		// Token: 0x0600411D RID: 16669 RVA: 0x0005A8D4 File Offset: 0x00058AD4
		private void Start()
		{
			this.spawnTrigger.onTriggerFirstEntered += this.OnEnter;
			this.spawnTrigger.onTriggerLastExited += this.OnExit;
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x0005A904 File Offset: 0x00058B04
		private void OnDestroy()
		{
			if (this.spawnTrigger != null)
			{
				this.spawnTrigger.onTriggerFirstEntered -= this.OnEnter;
				this.spawnTrigger.onTriggerLastExited -= this.OnExit;
			}
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x0016FDF0 File Offset: 0x0016DFF0
		public void TrySpawning()
		{
			if (Time.time > this.lastSpawnTime + this.cooldown)
			{
				this.lastSpawnTime = Time.time;
				ObjectPools.instance.Instantiate(this.prefab, this.spawnLocation.position, this.spawnLocation.rotation);
			}
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x0005A942 File Offset: 0x00058B42
		private void OnEnter()
		{
			if (this.spawnOnEnter)
			{
				this.TrySpawning();
			}
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x0005A952 File Offset: 0x00058B52
		private void OnExit()
		{
			if (this.spawnOnExit)
			{
				this.TrySpawning();
			}
		}

		// Token: 0x040041D7 RID: 16855
		public GameObject prefab;

		// Token: 0x040041D8 RID: 16856
		public float cooldown = 0.1f;

		// Token: 0x040041D9 RID: 16857
		private float lastSpawnTime;

		// Token: 0x040041DA RID: 16858
		[SerializeField]
		private BuilderSmallMonkeTrigger spawnTrigger;

		// Token: 0x040041DB RID: 16859
		[SerializeField]
		private bool spawnOnEnter = true;

		// Token: 0x040041DC RID: 16860
		[SerializeField]
		private bool spawnOnExit;

		// Token: 0x040041DD RID: 16861
		[SerializeField]
		private Transform spawnLocation;
	}
}
