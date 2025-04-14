using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A04 RID: 2564
	public class BuilderPieceParticleEmitter : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x06004020 RID: 16416 RVA: 0x00130E75 File Offset: 0x0012F075
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

		// Token: 0x06004021 RID: 16417 RVA: 0x00130EB0 File Offset: 0x0012F0B0
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

		// Token: 0x06004022 RID: 16418 RVA: 0x00130F10 File Offset: 0x0012F110
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

		// Token: 0x06004023 RID: 16419 RVA: 0x00130F6C File Offset: 0x0012F16C
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.StopParticles();
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
			this.OnZoneChanged();
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00130FA0 File Offset: 0x0012F1A0
		public void OnPieceDestroy()
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00130FC8 File Offset: 0x0012F1C8
		public void OnPieceActivate()
		{
			this.isPieceActive = true;
			if (this.inBuilderZone)
			{
				this.StartParticles();
			}
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00130FDF File Offset: 0x0012F1DF
		public void OnPieceDeactivate()
		{
			this.isPieceActive = false;
			this.StopParticles();
		}

		// Token: 0x0400414D RID: 16717
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x0400414E RID: 16718
		[SerializeField]
		private List<ParticleSystem> particles;

		// Token: 0x0400414F RID: 16719
		private bool inBuilderZone;

		// Token: 0x04004150 RID: 16720
		private bool isPieceActive;
	}
}
