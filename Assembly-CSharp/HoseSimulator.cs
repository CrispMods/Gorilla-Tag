using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class HoseSimulator : MonoBehaviour, ISpawnable
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000905 RID: 2309 RVA: 0x00030C8E File Offset: 0x0002EE8E
	// (set) Token: 0x06000906 RID: 2310 RVA: 0x00030C96 File Offset: 0x0002EE96
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000907 RID: 2311 RVA: 0x00030C9F File Offset: 0x0002EE9F
	// (set) Token: 0x06000908 RID: 2312 RVA: 0x00030CA7 File Offset: 0x0002EEA7
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000909 RID: 2313 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00030CB0 File Offset: 0x0002EEB0
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.anchors = rig.cosmeticReferences.Get(this.startAnchorRef).GetComponent<HoseSimulatorAnchors>();
		if (this.skinnedMeshRenderer != null)
		{
			Bounds localBounds = this.skinnedMeshRenderer.localBounds;
			localBounds.extents = this.localBoundsOverride;
			this.skinnedMeshRenderer.localBounds = localBounds;
		}
		this.hoseSectionLengths = new float[this.hoseBones.Length - 1];
		this.hoseBonePositions = new Vector3[this.hoseBones.Length];
		this.hoseBoneVelocities = new Vector3[this.hoseBones.Length];
		for (int i = 0; i < this.hoseSectionLengths.Length; i++)
		{
			float num = 1f;
			this.hoseSectionLengths[i] = num;
			this.totalHoseLength += num;
		}
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00030D78 File Offset: 0x0002EF78
	private void LateUpdate()
	{
		if (this.myHoldable.InLeftHand())
		{
			this.isLeftHanded = true;
		}
		else if (this.myHoldable.InRightHand())
		{
			this.isLeftHanded = false;
		}
		for (int i = 0; i < this.miscBones.Length; i++)
		{
			Transform transform = this.isLeftHanded ? this.anchors.miscAnchorsLeft[i] : this.anchors.miscAnchorsRight[i];
			this.miscBones[i].transform.position = transform.position;
			this.miscBones[i].transform.rotation = transform.rotation;
		}
		this.startAnchor = (this.isLeftHanded ? this.anchors.leftAnchorPoint : this.anchors.rightAnchorPoint);
		float x = this.myHoldable.transform.lossyScale.x;
		float num = 0f;
		Vector3 position = this.startAnchor.position;
		Vector3 ctrl = position + this.startAnchor.forward * this.startStiffness * x;
		Vector3 position2 = this.endAnchor.position;
		Vector3 ctrl2 = position2 - this.endAnchor.forward * this.endStiffness * x;
		for (int j = 0; j < this.hoseBones.Length; j++)
		{
			float num2 = num / this.totalHoseLength;
			Vector3 vector = BezierUtils.BezierSolve(num2, position, ctrl, ctrl2, position2);
			Vector3 a = BezierUtils.BezierSolve(num2 + 0.1f, position, ctrl, ctrl2, position2);
			if (this.firstUpdate)
			{
				this.hoseBones[j].transform.position = vector;
				this.hoseBonePositions[j] = vector;
				this.hoseBoneVelocities[j] = Vector3.zero;
			}
			else
			{
				this.hoseBoneVelocities[j] *= this.damping;
				this.hoseBonePositions[j] += this.hoseBoneVelocities[j] * Time.deltaTime;
				float num3 = this.hoseBoneMaxDisplacement[j] * x;
				if ((vector - this.hoseBonePositions[j]).IsLongerThan(num3))
				{
					Vector3 vector2 = vector + (this.hoseBonePositions[j] - vector).normalized * num3;
					this.hoseBoneVelocities[j] += (vector2 - this.hoseBonePositions[j]) / Time.deltaTime;
					this.hoseBonePositions[j] = vector2;
				}
				this.hoseBones[j].transform.position = this.hoseBonePositions[j];
			}
			this.hoseBones[j].transform.rotation = Quaternion.LookRotation(a - vector);
			if (j < this.hoseSectionLengths.Length)
			{
				num += this.hoseSectionLengths[j];
			}
		}
		this.firstUpdate = false;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x000310A6 File Offset: 0x0002F2A6
	private void OnDrawGizmosSelected()
	{
		if (this.hoseBonePositions != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLineStrip(this.hoseBonePositions, false);
		}
	}

	// Token: 0x04000AF4 RID: 2804
	[SerializeField]
	private SkinnedMeshRenderer skinnedMeshRenderer;

	// Token: 0x04000AF5 RID: 2805
	[SerializeField]
	private Vector3 localBoundsOverride;

	// Token: 0x04000AF6 RID: 2806
	[SerializeField]
	private Transform[] miscBones;

	// Token: 0x04000AF7 RID: 2807
	[SerializeField]
	private Transform[] hoseBones;

	// Token: 0x04000AF8 RID: 2808
	[SerializeField]
	private float[] hoseBoneMaxDisplacement;

	// Token: 0x04000AF9 RID: 2809
	[SerializeField]
	private CosmeticRefID startAnchorRef;

	// Token: 0x04000AFA RID: 2810
	private Transform startAnchor;

	// Token: 0x04000AFB RID: 2811
	[SerializeField]
	private float startStiffness = 0.5f;

	// Token: 0x04000AFC RID: 2812
	[SerializeField]
	private Transform endAnchor;

	// Token: 0x04000AFD RID: 2813
	[SerializeField]
	private float endStiffness = 0.5f;

	// Token: 0x04000AFE RID: 2814
	private Vector3[] hoseBonePositions;

	// Token: 0x04000AFF RID: 2815
	private Vector3[] hoseBoneVelocities;

	// Token: 0x04000B00 RID: 2816
	[SerializeField]
	private float damping = 0.97f;

	// Token: 0x04000B01 RID: 2817
	private float[] hoseSectionLengths;

	// Token: 0x04000B02 RID: 2818
	private float totalHoseLength;

	// Token: 0x04000B03 RID: 2819
	private bool firstUpdate = true;

	// Token: 0x04000B04 RID: 2820
	private HoseSimulatorAnchors anchors;

	// Token: 0x04000B05 RID: 2821
	[SerializeField]
	private TransferrableObject myHoldable;

	// Token: 0x04000B06 RID: 2822
	private bool isLeftHanded;
}
