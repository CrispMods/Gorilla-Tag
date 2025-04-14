using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009AC RID: 2476
	[NetworkBehaviourWeaved(1)]
	public class ChristmasTree : NetworkComponent
	{
		// Token: 0x06003D44 RID: 15684 RVA: 0x001219E0 File Offset: 0x0011FBE0
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

		// Token: 0x06003D45 RID: 15685 RVA: 0x00121A81 File Offset: 0x0011FC81
		private void Update()
		{
			if (this.spinTheTop && this.topOrnament)
			{
				this.topOrnament.transform.Rotate(0f, this.spinSpeed * Time.deltaTime, 0f, Space.World);
			}
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00121AC0 File Offset: 0x0011FCC0
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			foreach (AttachPoint attachPoint in this.attachPointsList)
			{
				attachPoint.onHookedChanged = (UnityAction)Delegate.Remove(attachPoint.onHookedChanged, new UnityAction(this.UpdateHangers));
			}
			this.attachPointsList.Clear();
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00121B40 File Offset: 0x0011FD40
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

		// Token: 0x06003D48 RID: 15688 RVA: 0x00121BC0 File Offset: 0x0011FDC0
		private void updateLight(bool enable)
		{
			this.isActive = enable;
			for (int i = 0; i < this.lightRenderers.Length; i++)
			{
				this.lightRenderers[i].material = (enable ? this.lightsOnMaterials[i % this.lightsOnMaterials.Length] : this.lightsOffMaterial);
			}
			this.spinTheTop = enable;
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06003D49 RID: 15689 RVA: 0x00121C17 File Offset: 0x0011FE17
		// (set) Token: 0x06003D4A RID: 15690 RVA: 0x00121C41 File Offset: 0x0011FE41
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

		// Token: 0x06003D4B RID: 15691 RVA: 0x00121C6C File Offset: 0x0011FE6C
		public override void WriteDataFusion()
		{
			this.Data = this.isActive;
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00121C7F File Offset: 0x0011FE7F
		public override void ReadDataFusion()
		{
			this.wasActive = this.isActive;
			this.isActive = this.Data;
			if (this.wasActive != this.isActive)
			{
				this.updateLight(this.isActive);
			}
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x00121CB8 File Offset: 0x0011FEB8
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			stream.SendNext(this.isActive);
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x00121CDC File Offset: 0x0011FEDC
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

		// Token: 0x06003D50 RID: 15696 RVA: 0x00121D4C File Offset: 0x0011FF4C
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x00121D64 File Offset: 0x0011FF64
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04003EB0 RID: 16048
		public GameObject hangers;

		// Token: 0x04003EB1 RID: 16049
		public GameObject lights;

		// Token: 0x04003EB2 RID: 16050
		public GameObject topOrnament;

		// Token: 0x04003EB3 RID: 16051
		public float spinSpeed = 60f;

		// Token: 0x04003EB4 RID: 16052
		private readonly List<AttachPoint> attachPointsList = new List<AttachPoint>();

		// Token: 0x04003EB5 RID: 16053
		private MeshRenderer[] lightRenderers;

		// Token: 0x04003EB6 RID: 16054
		private bool wasActive;

		// Token: 0x04003EB7 RID: 16055
		private bool isActive;

		// Token: 0x04003EB8 RID: 16056
		private bool spinTheTop;

		// Token: 0x04003EB9 RID: 16057
		[SerializeField]
		private Material lightsOffMaterial;

		// Token: 0x04003EBA RID: 16058
		[SerializeField]
		private Material[] lightsOnMaterials;

		// Token: 0x04003EBB RID: 16059
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _Data;
	}
}
