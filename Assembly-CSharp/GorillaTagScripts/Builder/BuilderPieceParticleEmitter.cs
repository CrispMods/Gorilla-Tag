using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A01 RID: 2561
	public class BuilderPieceParticleEmitter : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x06004014 RID: 16404 RVA: 0x001308AD File Offset: 0x0012EAAD
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

		// Token: 0x06004015 RID: 16405 RVA: 0x001308E8 File Offset: 0x0012EAE8
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

		// Token: 0x06004016 RID: 16406 RVA: 0x00130948 File Offset: 0x0012EB48
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

		// Token: 0x06004017 RID: 16407 RVA: 0x001309A4 File Offset: 0x0012EBA4
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.StopParticles();
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
			this.OnZoneChanged();
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x001309D8 File Offset: 0x0012EBD8
		public void OnPieceDestroy()
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x00130A00 File Offset: 0x0012EC00
		public void OnPieceActivate()
		{
			this.isPieceActive = true;
			if (this.inBuilderZone)
			{
				this.StartParticles();
			}
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x00130A17 File Offset: 0x0012EC17
		public void OnPieceDeactivate()
		{
			this.isPieceActive = false;
			this.StopParticles();
		}

		// Token: 0x0400413B RID: 16699
		[SerializeField]
		private BuilderPiece myPiece;

		// Token: 0x0400413C RID: 16700
		[SerializeField]
		private List<ParticleSystem> particles;

		// Token: 0x0400413D RID: 16701
		private bool inBuilderZone;

		// Token: 0x0400413E RID: 16702
		private bool isPieceActive;
	}
}
