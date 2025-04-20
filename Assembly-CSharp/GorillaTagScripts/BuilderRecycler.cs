using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B4 RID: 2484
	public class BuilderRecycler : MonoBehaviour
	{
		// Token: 0x06003D08 RID: 15624 RVA: 0x001571D0 File Offset: 0x001553D0
		private void Awake()
		{
			this.hasFans = (this.effectBehaviors.Count > 0 && this.bladeSoundPlayer != null && this.recycleParticles != null);
			this.hasPipes = (this.outputPipes.Count > 0);
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x00157224 File Offset: 0x00155424
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

		// Token: 0x06003D0A RID: 15626 RVA: 0x00057D35 File Offset: 0x00055F35
		private void OnDestroy()
		{
			if (ZoneManagement.instance != null)
			{
				ZoneManagement instance = ZoneManagement.instance;
				instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
			}
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x00157358 File Offset: 0x00155558
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

		// Token: 0x06003D0C RID: 15628 RVA: 0x00157414 File Offset: 0x00155614
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

		// Token: 0x06003D0D RID: 15629 RVA: 0x00157454 File Offset: 0x00155654
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

		// Token: 0x06003D0E RID: 15630 RVA: 0x001574F4 File Offset: 0x001556F4
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

		// Token: 0x06003D0F RID: 15631 RVA: 0x0015757C File Offset: 0x0015577C
		private Vector2 GetUVShiftOffset()
		{
			float y = Shader.GetGlobalVector("_Time").y;
			Vector4 vector = new Vector4(500f, 0f, 0f, 0f);
			Vector4 vector2 = vector / this.recycleEffectDuration;
			return new Vector2(-1f * (Mathf.Floor(y * vector2.x) * 1f / vector.x % 1f) * vector.x - vector.x + 165f, 0f);
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x00157608 File Offset: 0x00155808
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

		// Token: 0x06003D11 RID: 15633 RVA: 0x0015776C File Offset: 0x0015596C
		private void ResetOutputPipes()
		{
			foreach (MeshRenderer meshRenderer in this.outputPipes)
			{
				meshRenderer.GetPropertyBlock(this.props, 1);
				this.props.SetColor("_BaseColor", Color.black);
				meshRenderer.SetPropertyBlock(this.props, 1);
			}
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x001577E8 File Offset: 0x001559E8
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

		// Token: 0x04003DEE RID: 15854
		public float recycleEffectDuration = 0.25f;

		// Token: 0x04003DEF RID: 15855
		private double timeToStopBlades = double.MinValue;

		// Token: 0x04003DF0 RID: 15856
		private bool playingBladeEffect;

		// Token: 0x04003DF1 RID: 15857
		private bool playingPipeEffect;

		// Token: 0x04003DF2 RID: 15858
		private double timeToCheckPipes = double.MinValue;

		// Token: 0x04003DF3 RID: 15859
		public List<MonoBehaviour> effectBehaviors;

		// Token: 0x04003DF4 RID: 15860
		public GameObject recycleParticles;

		// Token: 0x04003DF5 RID: 15861
		public SoundBankPlayer bladeSoundPlayer;

		// Token: 0x04003DF6 RID: 15862
		public List<MeshRenderer> outputPipes;

		// Token: 0x04003DF7 RID: 15863
		public BuilderResourceColors builderResourceColors;

		// Token: 0x04003DF8 RID: 15864
		private bool hasFans;

		// Token: 0x04003DF9 RID: 15865
		private bool hasPipes;

		// Token: 0x04003DFA RID: 15866
		private MaterialPropertyBlock props;

		// Token: 0x04003DFB RID: 15867
		private int[] totalRecycledCost;

		// Token: 0x04003DFC RID: 15868
		private int[] currentChainCost;

		// Token: 0x04003DFD RID: 15869
		private int numPipes;

		// Token: 0x04003DFE RID: 15870
		internal int recyclerID = -1;

		// Token: 0x04003DFF RID: 15871
		private List<Renderer> zoneRenderers = new List<Renderer>(10);

		// Token: 0x04003E00 RID: 15872
		private bool inBuilderZone;
	}
}
