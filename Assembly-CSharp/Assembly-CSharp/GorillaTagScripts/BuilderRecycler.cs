using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000991 RID: 2449
	public class BuilderRecycler : MonoBehaviour
	{
		// Token: 0x06003BFC RID: 15356 RVA: 0x00114A38 File Offset: 0x00112C38
		private void Awake()
		{
			this.hasFans = (this.effectBehaviors.Count > 0 && this.bladeSoundPlayer != null && this.recycleParticles != null);
			this.hasPipes = (this.outputPipes.Count > 0);
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00114A8C File Offset: 0x00112C8C
		private void Start()
		{
			if (this.hasPipes)
			{
				this.numPipes = Mathf.Min(this.outputPipes.Count, 3);
				this.props = new MaterialPropertyBlock();
				this.ResetOutputPipes();
				this.totalRecycledCost = new int[3];
				this.currentChainCost = new int[3];
				for (int i = 0; i < this.totalRecycledCost.Length; i++)
				{
					this.totalRecycledCost[i] = 0;
					this.currentChainCost[i] = 0;
				}
			}
			this.zoneRenderers.Clear();
			if (this.hasPipes)
			{
				this.zoneRenderers.AddRange(this.outputPipes);
			}
			if (this.hasFans)
			{
				foreach (MonoBehaviour monoBehaviour in this.effectBehaviors)
				{
					Renderer component = monoBehaviour.GetComponent<Renderer>();
					if (component != null)
					{
						this.zoneRenderers.Add(component);
					}
				}
			}
			this.inBuilderZone = true;
			ZoneManagement instance = ZoneManagement.instance;
			instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
			this.OnZoneChanged();
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x00114BC0 File Offset: 0x00112DC0
		private void OnDestroy()
		{
			if (ZoneManagement.instance != null)
			{
				ZoneManagement instance = ZoneManagement.instance;
				instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
			}
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x00114BF8 File Offset: 0x00112DF8
		private void OnZoneChanged()
		{
			bool flag = ZoneManagement.instance.IsZoneActive(GTZone.monkeBlocks);
			if (flag && !this.inBuilderZone)
			{
				using (List<Renderer>.Enumerator enumerator = this.zoneRenderers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Renderer renderer = enumerator.Current;
						renderer.enabled = true;
					}
					goto IL_8B;
				}
			}
			if (!flag && this.inBuilderZone)
			{
				foreach (Renderer renderer2 in this.zoneRenderers)
				{
					renderer2.enabled = false;
				}
			}
			IL_8B:
			this.inBuilderZone = flag;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x00114CB4 File Offset: 0x00112EB4
		private void OnTriggerEnter(Collider other)
		{
			BuilderPiece builderPieceFromCollider = BuilderPiece.GetBuilderPieceFromCollider(other);
			if (builderPieceFromCollider == null)
			{
				return;
			}
			if (!builderPieceFromCollider.isBuiltIntoTable && !builderPieceFromCollider.isArmShelf)
			{
				BuilderTable.instance.RequestRecyclePiece(builderPieceFromCollider, true, this.recyclerID);
			}
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x00114CF4 File Offset: 0x00112EF4
		public void OnRecycleRequestedAtRecycler(BuilderPiece piece)
		{
			if (this.hasPipes)
			{
				this.AddPieceCost(piece.cost);
			}
			if (this.hasFans)
			{
				foreach (MonoBehaviour monoBehaviour in this.effectBehaviors)
				{
					monoBehaviour.enabled = true;
				}
				this.recycleParticles.SetActive(true);
				this.bladeSoundPlayer.Play();
				this.timeToStopBlades = (double)(Time.time + this.recycleEffectDuration);
				this.playingBladeEffect = true;
			}
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x00114D94 File Offset: 0x00112F94
		private void AddPieceCost(BuilderResources cost)
		{
			foreach (BuilderResourceQuantity builderResourceQuantity in cost.quantities)
			{
				if (builderResourceQuantity.type >= BuilderResourceType.Basic && builderResourceQuantity.type < BuilderResourceType.Count)
				{
					this.totalRecycledCost[(int)builderResourceQuantity.type] += builderResourceQuantity.count;
				}
			}
			if (!this.playingPipeEffect)
			{
				this.UpdatePipeLoop();
			}
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x00114E1C File Offset: 0x0011301C
		private Vector2 GetUVShiftOffset()
		{
			float y = Shader.GetGlobalVector("_Time").y;
			Vector4 vector = new Vector4(500f, 0f, 0f, 0f);
			Vector4 vector2 = vector / this.recycleEffectDuration;
			return new Vector2(-1f * (Mathf.Floor(y * vector2.x) * 1f / vector.x % 1f) * vector.x - vector.x + 165f, 0f);
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x00114EA8 File Offset: 0x001130A8
		private void UpdatePipeLoop()
		{
			bool flag = false;
			for (int i = 0; i < this.numPipes; i++)
			{
				if (this.totalRecycledCost[i] > 0)
				{
					flag = true;
					this.outputPipes[i].GetPropertyBlock(this.props, 1);
					Vector4 value = new Vector4(500f, 0f, 0f, 0f) / this.recycleEffectDuration;
					Vector2 uvshiftOffset = this.GetUVShiftOffset();
					this.props.SetColor("_BaseColor", this.builderResourceColors.colors[i].color);
					this.props.SetVector("_UvShiftRate", value);
					this.props.SetVector("_UvShiftOffset", uvshiftOffset);
					this.outputPipes[i].SetPropertyBlock(this.props, 1);
					this.totalRecycledCost[i] = Mathf.Max(this.totalRecycledCost[i] - 1, 0);
				}
				else
				{
					this.outputPipes[i].GetPropertyBlock(this.props, 1);
					this.props.SetColor("_BaseColor", Color.black);
					this.outputPipes[i].SetPropertyBlock(this.props, 1);
				}
			}
			if (flag)
			{
				this.playingPipeEffect = true;
				this.timeToCheckPipes = (double)(Time.time + this.recycleEffectDuration);
				return;
			}
			this.playingPipeEffect = false;
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x0011500C File Offset: 0x0011320C
		private void ResetOutputPipes()
		{
			foreach (MeshRenderer meshRenderer in this.outputPipes)
			{
				meshRenderer.GetPropertyBlock(this.props, 1);
				this.props.SetColor("_BaseColor", Color.black);
				meshRenderer.SetPropertyBlock(this.props, 1);
			}
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x00115088 File Offset: 0x00113288
		public void UpdateRecycler()
		{
			if (this.playingBladeEffect && (double)Time.time > this.timeToStopBlades)
			{
				if (this.hasFans)
				{
					foreach (MonoBehaviour monoBehaviour in this.effectBehaviors)
					{
						monoBehaviour.enabled = false;
					}
					this.recycleParticles.SetActive(false);
				}
				this.playingBladeEffect = false;
			}
			if (this.playingPipeEffect && (double)Time.time > this.timeToCheckPipes)
			{
				this.UpdatePipeLoop();
			}
		}

		// Token: 0x04003D26 RID: 15654
		public float recycleEffectDuration = 0.25f;

		// Token: 0x04003D27 RID: 15655
		private double timeToStopBlades = double.MinValue;

		// Token: 0x04003D28 RID: 15656
		private bool playingBladeEffect;

		// Token: 0x04003D29 RID: 15657
		private bool playingPipeEffect;

		// Token: 0x04003D2A RID: 15658
		private double timeToCheckPipes = double.MinValue;

		// Token: 0x04003D2B RID: 15659
		public List<MonoBehaviour> effectBehaviors;

		// Token: 0x04003D2C RID: 15660
		public GameObject recycleParticles;

		// Token: 0x04003D2D RID: 15661
		public SoundBankPlayer bladeSoundPlayer;

		// Token: 0x04003D2E RID: 15662
		public List<MeshRenderer> outputPipes;

		// Token: 0x04003D2F RID: 15663
		public BuilderResourceColors builderResourceColors;

		// Token: 0x04003D30 RID: 15664
		private bool hasFans;

		// Token: 0x04003D31 RID: 15665
		private bool hasPipes;

		// Token: 0x04003D32 RID: 15666
		private MaterialPropertyBlock props;

		// Token: 0x04003D33 RID: 15667
		private int[] totalRecycledCost;

		// Token: 0x04003D34 RID: 15668
		private int[] currentChainCost;

		// Token: 0x04003D35 RID: 15669
		private int numPipes;

		// Token: 0x04003D36 RID: 15670
		internal int recyclerID = -1;

		// Token: 0x04003D37 RID: 15671
		private List<Renderer> zoneRenderers = new List<Renderer>(10);

		// Token: 0x04003D38 RID: 15672
		private bool inBuilderZone;
	}
}
