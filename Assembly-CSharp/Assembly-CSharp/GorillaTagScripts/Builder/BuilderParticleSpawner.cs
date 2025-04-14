using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009FD RID: 2557
	public class BuilderParticleSpawner : MonoBehaviour
	{
		// Token: 0x06003FE4 RID: 16356 RVA: 0x0012F308 File Offset: 0x0012D508
		private void Start()
		{
			this.spawnTrigger.onTriggerFirstEntered += this.OnEnter;
			this.spawnTrigger.onTriggerLastExited += this.OnExit;
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0012F338 File Offset: 0x0012D538
		private void OnDestroy()
		{
			if (this.spawnTrigger != null)
			{
				this.spawnTrigger.onTriggerFirstEntered -= this.OnEnter;
				this.spawnTrigger.onTriggerLastExited -= this.OnExit;
			}
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0012F378 File Offset: 0x0012D578
		public void TrySpawning()
		{
			if (Time.time > this.lastSpawnTime + this.cooldown)
			{
				this.lastSpawnTime = Time.time;
				ObjectPools.instance.Instantiate(this.prefab, this.spawnLocation.position, this.spawnLocation.rotation);
			}
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0012F3CB File Offset: 0x0012D5CB
		private void OnEnter()
		{
			if (this.spawnOnEnter)
			{
				this.TrySpawning();
			}
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0012F3DB File Offset: 0x0012D5DB
		private void OnExit()
		{
			if (this.spawnOnExit)
			{
				this.TrySpawning();
			}
		}

		// Token: 0x040040EF RID: 16623
		public GameObject prefab;

		// Token: 0x040040F0 RID: 16624
		public float cooldown = 0.1f;

		// Token: 0x040040F1 RID: 16625
		private float lastSpawnTime;

		// Token: 0x040040F2 RID: 16626
		[SerializeField]
		private BuilderSmallMonkeTrigger spawnTrigger;

		// Token: 0x040040F3 RID: 16627
		[SerializeField]
		private bool spawnOnEnter = true;

		// Token: 0x040040F4 RID: 16628
		[SerializeField]
		private bool spawnOnExit;

		// Token: 0x040040F5 RID: 16629
		[SerializeField]
		private Transform spawnLocation;
	}
}
