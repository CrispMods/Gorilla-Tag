using System;
using UnityEngine;

// Token: 0x020003AD RID: 941
public class TransformReset : MonoBehaviour
{
	// Token: 0x060015F7 RID: 5623 RVA: 0x0006A0BC File Offset: 0x000682BC
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

	// Token: 0x060015F8 RID: 5624 RVA: 0x0006A108 File Offset: 0x00068308
	public void ReturnTransforms()
	{
		foreach (TransformReset.OriginalGameObjectTransform originalGameObjectTransform in this.tempTransformList)
		{
			originalGameObjectTransform.thisTransform.position = originalGameObjectTransform.thisPosition;
			originalGameObjectTransform.thisTransform.rotation = originalGameObjectTransform.thisRotation;
		}
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x0006A158 File Offset: 0x00068358
	public void SetScale(float ratio)
	{
		foreach (TransformReset.OriginalGameObjectTransform originalGameObjectTransform in this.transformList)
		{
			originalGameObjectTransform.thisTransform.localScale *= ratio;
		}
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x0006A19C File Offset: 0x0006839C
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

	// Token: 0x04001834 RID: 6196
	private TransformReset.OriginalGameObjectTransform[] transformList;

	// Token: 0x04001835 RID: 6197
	private TransformReset.OriginalGameObjectTransform[] tempTransformList;

	// Token: 0x020003AE RID: 942
	private struct OriginalGameObjectTransform
	{
		// Token: 0x060015FC RID: 5628 RVA: 0x0006A234 File Offset: 0x00068434
		public OriginalGameObjectTransform(Transform constructionTransform)
		{
			this._thisTransform = constructionTransform;
			this._thisPosition = constructionTransform.position;
			this._thisRotation = constructionTransform.rotation;
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x0006A255 File Offset: 0x00068455
		// (set) Token: 0x060015FE RID: 5630 RVA: 0x0006A25D File Offset: 0x0006845D
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

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060015FF RID: 5631 RVA: 0x0006A266 File Offset: 0x00068466
		// (set) Token: 0x06001600 RID: 5632 RVA: 0x0006A26E File Offset: 0x0006846E
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

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x0006A277 File Offset: 0x00068477
		// (set) Token: 0x06001602 RID: 5634 RVA: 0x0006A27F File Offset: 0x0006847F
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

		// Token: 0x04001836 RID: 6198
		private Transform _thisTransform;

		// Token: 0x04001837 RID: 6199
		private Vector3 _thisPosition;

		// Token: 0x04001838 RID: 6200
		private Quaternion _thisRotation;
	}
}
