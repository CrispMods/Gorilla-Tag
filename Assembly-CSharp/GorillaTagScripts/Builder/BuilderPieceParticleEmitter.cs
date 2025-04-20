using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A2E RID: 2606
	public class BuilderPieceParticleEmitter : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x06004159 RID: 16729 RVA: 0x0005AA8C File Offset: 0x00058C8C
		private void OnZoneChanged()
		{
			this.inBuilderZone = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
			if (this.inBuilderZone && this.isPieceActive)
			{
				this.StartParticles();
				return;
			}
			if (!this.inBuilderZone)
			{
				this.StopParticles();
			}
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x001717EC File Offset: 0x0016F9EC
		private void StopParticles()
		{
			foreach (ParticleSystem particleSystem in this.particles)
			{
				if (particleSystem.isPlaying)
				{
					particleSystem.Stop();
					particleSystem.Clear();
				}
			}
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x0017184C File Offset: 0x0016FA4C
		private void StartParticles()
		{
			foreach (ParticleSystem particleSystem in this.particles)
			{
				if (!particleSystem.isPlaying)
				{
					particleSystem.Play();
				}
			}
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x0005AAC5 File Offset: 0x00058CC5
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.StopParticles();
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
			this.OnZoneChanged();
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0005AAF9 File Offset: 0x00058CF9
		public void OnPieceDestroy()
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00030607 File Offset: 0x0002E807
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x0005AB21 File Offset: 0x00058D21
		public void OnPieceActivate()
		{
			this.isPieceActive = true;
			if (this.inBuilderZone)
			{
				this.StartParticles();
			}
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x0005AB38 File Offset: 0x00058D38
		public void OnPieceDeactivate()
		{
			this.isPieceActive = false;
			this.StopParticles();
		}

		// Token: 0x04004235 RID: 16949
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x04004236 RID: 16950
		[SerializeField]
		private List<ParticleSystem> particles;

		// Token: 0x04004237 RID: 16951
		private bool inBuilderZone;

		// Token: 0x04004238 RID: 16952
		private bool isPieceActive;
	}
}
