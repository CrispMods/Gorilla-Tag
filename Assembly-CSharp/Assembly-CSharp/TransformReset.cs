using System;
using UnityEngine;

// Token: 0x020003AD RID: 941
public class TransformReset : MonoBehaviour
{
	// Token: 0x060015FA RID: 5626 RVA: 0x0006A440 File Offset: 0x00068640
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

	// Token: 0x060015FB RID: 5627 RVA: 0x0006A48C File Offset: 0x0006868C
	public void ReturnTransforms()
	{
		foreach (TransformReset.OriginalGameObjectTransform originalGameObjectTransform in this.tempTransformList)
		{
			originalGameObjectTransform.thisTransform.position = originalGameObjectTransform.thisPosition;
			originalGameObjectTransform.thisTransform.rotation = originalGameObjectTransform.thisRotation;
		}
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x0006A4DC File Offset: 0x000686DC
	public void SetScale(float ratio)
	{
		foreach (TransformReset.OriginalGameObjectTransform originalGameObjectTransform in this.transformList)
		{
			originalGameObjectTransform.thisTransform.localScale *= ratio;
		}
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x0006A520 File Offset: 0x00068720
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

	// Token: 0x04001835 RID: 6197
	private TransformReset.OriginalGameObjectTransform[] transformList;

	// Token: 0x04001836 RID: 6198
	private TransformReset.OriginalGameObjectTransform[] tempTransformList;

	// Token: 0x020003AE RID: 942
	private struct OriginalGameObjectTransform
	{
		// Token: 0x060015FF RID: 5631 RVA: 0x0006A5B8 File Offset: 0x000687B8
		public OriginalGameObjectTransform(Transform constructionTransform)
		{
			this._thisTransform = constructionTransform;
			this._thisPosition = constructionTransform.position;
			this._thisRotation = constructionTransform.rotation;
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x0006A5D9 File Offset: 0x000687D9
		// (set) Token: 0x06001601 RID: 5633 RVA: 0x0006A5E1 File Offset: 0x000687E1
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
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x0006A5EA File Offset: 0x000687EA
		// (set) Token: 0x06001603 RID: 5635 RVA: 0x0006A5F2 File Offset: 0x000687F2
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
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x0006A5FB File Offset: 0x000687FB
		// (set) Token: 0x06001605 RID: 5637 RVA: 0x0006A603 File Offset: 0x00068803
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

		// Token: 0x04001837 RID: 6199
		private Transform _thisTransform;

		// Token: 0x04001838 RID: 6200
		private Vector3 _thisPosition;

		// Token: 0x04001839 RID: 6201
		private Quaternion _thisRotation;
	}
}
