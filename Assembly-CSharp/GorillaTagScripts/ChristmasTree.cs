using System;
using System.Collections.Generic;
using Fusion;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts
{
	// Token: 0x020009CF RID: 2511
	[NetworkBehaviourWeaved(1)]
	public class ChristmasTree : NetworkComponent
	{
		// Token: 0x06003E50 RID: 15952 RVA: 0x00163690 File Offset: 0x00161890
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

		// Token: 0x06003E51 RID: 15953 RVA: 0x0005880F File Offset: 0x00056A0F
		private void Update()
		{
			if (this.spinTheTop && this.topOrnament)
			{
				this.topOrnament.transform.Rotate(0f, this.spinSpeed * Time.deltaTime, 0f, Space.World);
			}
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x00163734 File Offset: 0x00161934
		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			foreach (AttachPoint attachPoint in this.attachPointsList)
			{
				attachPoint.onHookedChanged = (UnityAction)Delegate.Remove(attachPoint.onHookedChanged, new UnityAction(this.UpdateHangers));
			}
			this.attachPointsList.Clear();
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x001637B4 File Offset: 0x001619B4
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

		// Token: 0x06003E54 RID: 15956 RVA: 0x00163834 File Offset: 0x00161A34
		private void updateLight(bool enable)
		{
			this.isActive = enable;
			for (int i = 0; i < this.lightRenderers.Length; i++)
			{
				this.lightRenderers[i].material = (enable ? this.lightsOnMaterials[i % this.lightsOnMaterials.Length] : this.lightsOffMaterial);
			}
			this.spinTheTop = enable;
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06003E55 RID: 15957 RVA: 0x0005884D File Offset: 0x00056A4D
		// (set) Token: 0x06003E56 RID: 15958 RVA: 0x00058877 File Offset: 0x00056A77
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

		// Token: 0x06003E57 RID: 15959 RVA: 0x000588A2 File Offset: 0x00056AA2
		public override void WriteDataFusion()
		{
			this.Data = this.isActive;
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x000588B5 File Offset: 0x00056AB5
		public override void ReadDataFusion()
		{
			this.wasActive = this.isActive;
			this.isActive = this.Data;
			if (this.wasActive != this.isActive)
			{
				this.updateLight(this.isActive);
			}
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x000588EE File Offset: 0x00056AEE
		protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
		{
			if (!info.Sender.IsMasterClient)
			{
				return;
			}
			stream.SendNext(this.isActive);
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x0016388C File Offset: 0x00161A8C
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

		// Token: 0x06003E5C RID: 15964 RVA: 0x0005892D File Offset: 0x00056B2D
		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool A_1)
		{
			base.CopyBackingFieldsToState(A_1);
			this.Data = this._Data;
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x00058945 File Offset: 0x00056B45
		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			this._Data = this.Data;
		}

		// Token: 0x04003F78 RID: 16248
		public GameObject hangers;

		// Token: 0x04003F79 RID: 16249
		public GameObject lights;

		// Token: 0x04003F7A RID: 16250
		public GameObject topOrnament;

		// Token: 0x04003F7B RID: 16251
		public float spinSpeed = 60f;

		// Token: 0x04003F7C RID: 16252
		private readonly List<AttachPoint> attachPointsList = new List<AttachPoint>();

		// Token: 0x04003F7D RID: 16253
		private MeshRenderer[] lightRenderers;

		// Token: 0x04003F7E RID: 16254
		private bool wasActive;

		// Token: 0x04003F7F RID: 16255
		private bool isActive;

		// Token: 0x04003F80 RID: 16256
		private bool spinTheTop;

		// Token: 0x04003F81 RID: 16257
		[SerializeField]
		private Material lightsOffMaterial;

		// Token: 0x04003F82 RID: 16258
		[SerializeField]
		private Material[] lightsOnMaterials;

		// Token: 0x04003F83 RID: 16259
		[WeaverGenerated]
		[DefaultForProperty("Data", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _Data;
	}
}
