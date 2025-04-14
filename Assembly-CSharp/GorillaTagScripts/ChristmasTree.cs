using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009A9 RID: 2473
	[NetworkBehaviourWeaved(1)]
	public class ChristmasTree : NetworkComponent
	{
		// Token: 0x06003D38 RID: 15672 RVA: 0x00121418 File Offset: 0x0011F618
		protected override void Awake()
		{
			base.Awake();
			foreach (AttachPoint attachPoint in this.hangers.GetComponentsInChildren<AttachPoint>())
			{
				this.attachPointsList.Add(attachPoint);
				AttachPoint attachPoint2 = attachPoint;
				attachPoint2.onHookedChanged = (UnityAction)Delegate.Combine(attachPoint2.onHookedChanged, new UnityAction(this.UpdateHangers));
			}
			this.lightRenderers = this.lights.GetComponentsInChildren<MeshRenderer>();
			MeshRenderer[] array = this.lightRenderers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].material = this.lightsOffMaterial;
			}
			this.wasActive = false;
			this.isActive = false;
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x001214B9 File Offset: 0x0011F6B9
		private void Update()
		{
			if (this.spinTheTop && this.topOrnament)
			{
				this.topOrnament.transform.Rotate(0f, this.spinSpeed * Time.deltaTime, 0f, Space.World);
			}
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x001214F8 File Offset: 0x0011F6F8
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			foreach (AttachPoint attachPoint in this.attachPointsList)
			{
				attachPoint.onHookedChanged = (UnityAction)Delegate.Remove(attachPoint.onHookedChanged, new UnityAction(this.UpdateHangers));
			}
			this.attachPointsList.Clear();
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00121578 File Offset: 0x0011F778
		private void UpdateHangers()
		{
			if (this.attachPointsList.Count == 0)
			{
				return;
			}
			using (List<AttachPoint>.Enumerator enumerator = this.attachPointsList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsHooked())
					{
						if (base.IsMine)
						{
							this.updateLight(true);
						}
						return;
					}
				}
			}
			if (base.IsMine)
			{
				this.updateLight(false);
			}
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x001215F8 File Offset: 0x0011F7F8
		private void updateLight(bool enable)
		{
			this.isActive = enable;
			for (int i = 0; i < this.lightRenderers.Length; i++)
			{
				this.lightRenderers[i].material = (enable ? this.lightsOnMaterials[i % this.lightsOnMaterials.Length] : this.lightsOffMaterial);
			}
			this.spinTheTop = enable;
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06003D3D RID: 15677 RVA: 0x0012164F File Offset: 0x0011F84F
		// (set) Token: 0x06003D3E RID: 15678 RVA: 0x00121679 File Offset: 0x0011F879
		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool Data
		{
			get
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChristmasTree.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(this.Ptr + 0);
			}
			set
			{
				if (this.Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChristmasTree.Data. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(this.Ptr + 0) = value;
			}
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x001216A4 File Offset: 0x0011F8A4
		public override void WriteDataFusion()
		{
			this.Data = this.isActive;
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x001216B7 File Offset: 0x0011F8B7
		public override void ReadDataFusion()
		{
			this.wasActive = this.isActive;
			this.isActive = this.Data;
			if (this.wasActive != this.isActive)
			{
				this.updateLight(this.isActive);
			}
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x001216F0 File Offset: 0x0011F8F0
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			stream.SendNext(this.isActive);
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00121714 File Offset: 0x0011F914
		protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			this.wasActive = this.isActive;
			this.isActive = (bool)stream.ReceiveNext();
			if (this.wasActive != this.isActive)
			{
				this.updateLight(this.isActive);
			}
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00121784 File Offset: 0x0011F984
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x0012179C File Offset: 0x0011F99C
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04003E9E RID: 16030
		public GameObject hangers;

		// Token: 0x04003E9F RID: 16031
		public GameObject lights;

		// Token: 0x04003EA0 RID: 16032
		public GameObject topOrnament;

		// Token: 0x04003EA1 RID: 16033
		public float spinSpeed = 60f;

		// Token: 0x04003EA2 RID: 16034
		private readonly List<AttachPoint> attachPointsList = new List<AttachPoint>();

		// Token: 0x04003EA3 RID: 16035
		private MeshRenderer[] lightRenderers;

		// Token: 0x04003EA4 RID: 16036
		private bool wasActive;

		// Token: 0x04003EA5 RID: 16037
		private bool isActive;

		// Token: 0x04003EA6 RID: 16038
		private bool spinTheTop;

		// Token: 0x04003EA7 RID: 16039
		[SerializeField]
		private Material lightsOffMaterial;

		// Token: 0x04003EA8 RID: 16040
		[SerializeField]
		private Material[] lightsOnMaterials;

		// Token: 0x04003EA9 RID: 16041
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _Data;
	}
}
