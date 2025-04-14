using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200098E RID: 2446
	public class BuilderRecycler : MonoBehaviour
	{
		// Token: 0x06003BF0 RID: 15344 RVA: 0x00114470 File Offset: 0x00112670
		private void Awake()
		{
			this.hasFans = (this.effectBehaviors.Count > 0 && this.bladeSoundPlayer != null && this.recycleParticles != null);
			this.hasPipes = (this.outputPipes.Count > 0);
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x001144C4 File Offset: 0x001126C4
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

		// Token: 0x06003BF2 RID: 15346 RVA: 0x001145F8 File Offset: 0x001127F8
		private void OnDestroy()
		{
			if (ZoneManagement.instance != null)
			{
				ZoneManagement instance = ZoneManagement.instance;
				instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x00114630 File Offset: 0x00112830
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

		// Token: 0x06003BF4 RID: 15348 RVA: 0x001146EC File Offset: 0x001128EC
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

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0011472C File Offset: 0x0011292C
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

		// Token: 0x06003BF6 RID: 15350 RVA: 0x001147CC File Offset: 0x001129CC
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

		// Token: 0x06003BF7 RID: 15351 RVA: 0x00114854 File Offset: 0x00112A54
		private Vector2 GetUVShiftOffset()
		{
			float y = Shader.GetGlobalVector("_Time").y;
			Vector4 vector = new Vector4(500f, 0f, 0f, 0f);
			Vector4 vector2 = vector / this.recycleEffectDuration;
			return new Vector2(-1f * (Mathf.Floor(y * vector2.x) * 1f / vector.x % 1f) * vector.x - vector.x + 165f, 0f);
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x001148E0 File Offset: 0x00112AE0
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

		// Token: 0x06003BF9 RID: 15353 RVA: 0x00114A44 File Offset: 0x00112C44
		private void ResetOutputPipes()
		{
			foreach (MeshRenderer meshRenderer in this.outputPipes)
			{
				meshRenderer.GetPropertyBlock(this.props, 1);
				this.props.SetColor("_BaseColor", Color.black);
				meshRenderer.SetPropertyBlock(this.props, 1);
			}
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x00114AC0 File Offset: 0x00112CC0
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

		// Token: 0x04003D14 RID: 15636
		public float recycleEffectDuration = 0.25f;

		// Token: 0x04003D15 RID: 15637
		private double timeToStopBlades = double.MinValue;

		// Token: 0x04003D16 RID: 15638
		private bool playingBladeEffect;

		// Token: 0x04003D17 RID: 15639
		private bool playingPipeEffect;

		// Token: 0x04003D18 RID: 15640
		private double timeToCheckPipes = double.MinValue;

		// Token: 0x04003D19 RID: 15641
		public List<MonoBehaviour> effectBehaviors;

		// Token: 0x04003D1A RID: 15642
		public GameObject recycleParticles;

		// Token: 0x04003D1B RID: 15643
		public SoundBankPlayer bladeSoundPlayer;

		// Token: 0x04003D1C RID: 15644
		public List<MeshRenderer> outputPipes;

		// Token: 0x04003D1D RID: 15645
		public BuilderResourceColors builderResourceColors;

		// Token: 0x04003D1E RID: 15646
		private bool hasFans;

		// Token: 0x04003D1F RID: 15647
		private bool hasPipes;

		// Token: 0x04003D20 RID: 15648
		private MaterialPropertyBlock props;

		// Token: 0x04003D21 RID: 15649
		private int[] totalRecycledCost;

		// Token: 0x04003D22 RID: 15650
		private int[] currentChainCost;

		// Token: 0x04003D23 RID: 15651
		private int numPipes;

		// Token: 0x04003D24 RID: 15652
		internal int recyclerID = -1;

		// Token: 0x04003D25 RID: 15653
		private List<Renderer> zoneRenderers = new List<Renderer>(10);

		// Token: 0x04003D26 RID: 15654
		private bool inBuilderZone;
	}
}
