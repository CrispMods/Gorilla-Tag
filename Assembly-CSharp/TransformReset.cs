using System;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class TransformReset : MonoBehaviour
{
	// Token: 0x06001643 RID: 5699 RVA: 0x000C20A0 File Offset: 0x000C02A0
	private void Awake()
	{
		Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
		this.transformList = new TransformReset.OriginalGameObjectTransform[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.transformList[i] = new TransformReset.OriginalGameObjectTransform(componentsInChildren[i]);
		}
		this.ResetTransforms();
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x000C20EC File Offset: 0x000C02EC
	public void ReturnTransforms()
	{
		foreach (TransformReset.OriginalGameObjectTransform originalGameObjectTransform in this.tempTransformList)
		{
			originalGameObjectTransform.thisTransform.position = originalGameObjectTransform.thisPosition;
			originalGameObjectTransform.thisTransform.rotation = originalGameObjectTransform.thisRotation;
		}
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x000C213C File Offset: 0x000C033C
	public void SetScale(float ratio)
	{
		foreach (TransformReset.OriginalGameObjectTransform originalGameObjectTransform in this.transformList)
		{
			originalGameObjectTransform.thisTransform.localScale *= ratio;
		}
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x000C2180 File Offset: 0x000C0380
	public void ResetTransforms()
	{
		this.tempTransformList = new TransformReset.OriginalGameObjectTransform[this.transformList.Length];
		for (int i = 0; i < this.transformList.Length; i++)
		{
			this.tempTransformList[i] = new TransformReset.OriginalGameObjectTransform(this.transformList[i].thisTransform);
		}
		foreach (TransformReset.OriginalGameObjectTransform originalGameObjectTransform in this.transformList)
		{
			originalGameObjectTransform.thisTransform.position = originalGameObjectTransform.thisPosition;
			originalGameObjectTransform.thisTransform.rotation = originalGameObjectTransform.thisRotation;
		}
	}

	// Token: 0x0400187B RID: 6267
	private TransformReset.OriginalGameObjectTransform[] transformList;

	// Token: 0x0400187C RID: 6268
	private TransformReset.OriginalGameObjectTransform[] tempTransformList;

	// Token: 0x020003B9 RID: 953
	private struct OriginalGameObjectTransform
	{
		// Token: 0x06001648 RID: 5704 RVA: 0x0003F0B2 File Offset: 0x0003D2B2
		public OriginalGameObjectTransform(Transform constructionTransform)
		{
			this._thisTransform = constructionTransform;
			this._thisPosition = constructionTransform.position;
			this._thisRotation = constructionTransform.rotation;
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001649 RID: 5705 RVA: 0x0003F0D3 File Offset: 0x0003D2D3
		// (set) Token: 0x0600164A RID: 5706 RVA: 0x0003F0DB File Offset: 0x0003D2DB
		public Transform thisTransform
		{
			get
			{
				return this._thisTransform;
			}
			set
			{
				this._thisTransform = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600164B RID: 5707 RVA: 0x0003F0E4 File Offset: 0x0003D2E4
		// (set) Token: 0x0600164C RID: 5708 RVA: 0x0003F0EC File Offset: 0x0003D2EC
		public Vector3 thisPosition
		{
			get
			{
				return this._thisPosition;
			}
			set
			{
				this._thisPosition = value;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600164D RID: 5709 RVA: 0x0003F0F5 File Offset: 0x0003D2F5
		// (set) Token: 0x0600164E RID: 5710 RVA: 0x0003F0FD File Offset: 0x0003D2FD
		public Quaternion thisRotation
		{
			get
			{
				return this._thisRotation;
			}
			set
			{
				this._thisRotation = value;
			}
		}

		// Token: 0x0400187D RID: 6269
		private Transform _thisTransform;

		// Token: 0x0400187E RID: 6270
		private Vector3 _thisPosition;

		// Token: 0x0400187F RID: 6271
		private Quaternion _thisRotation;
	}
}
