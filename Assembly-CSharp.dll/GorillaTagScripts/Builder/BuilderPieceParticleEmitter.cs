using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x02000A04 RID: 2564
	public class BuilderPieceParticleEmitter : MonoBehaviour, IBuilderPieceComponent
	{
		// Token: 0x06004020 RID: 16416 RVA: 0x0005908A File Offset: 0x0005728A
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

		// Token: 0x06004021 RID: 16417 RVA: 0x0016A968 File Offset: 0x00168B68
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

		// Token: 0x06004022 RID: 16418 RVA: 0x0016A9C8 File Offset: 0x00168BC8
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

		// Token: 0x06004023 RID: 16419 RVA: 0x000590C3 File Offset: 0x000572C3
		public void OnPieceCreate(int pieceType, int pieceId)
		{
			this.StopParticles();
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
			this.OnZoneChanged();
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x000590F7 File Offset: 0x000572F7
		public void OnPieceDestroy()
		{
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x0002F75F File Offset: 0x0002D95F
		public void OnPiecePlacementDeserialized()
		{
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x0005911F File Offset: 0x0005731F
		public void OnPieceActivate()
		{
			this.isPieceActive = true;
			if (this.inBuilderZone)
			{
				this.StartParticles();
			}
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00059136 File Offset: 0x00057336
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
