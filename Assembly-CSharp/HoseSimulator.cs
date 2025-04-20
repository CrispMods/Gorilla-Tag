using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class HoseSimulator : MonoBehaviour, ISpawnable
{
	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000952 RID: 2386 RVA: 0x0003693D File Offset: 0x00034B3D
	// (set) Token: 0x06000953 RID: 2387 RVA: 0x00036945 File Offset: 0x00034B45
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000954 RID: 2388 RVA: 0x0003694E File Offset: 0x00034B4E
	// (set) Token: 0x06000955 RID: 2389 RVA: 0x00036956 File Offset: 0x00034B56
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000956 RID: 2390 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x000915C0 File Offset: 0x0008F7C0
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

	// Token: 0x06000958 RID: 2392 RVA: 0x00091688 File Offset: 0x0008F888
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

	// Token: 0x06000959 RID: 2393 RVA: 0x0003695F File Offset: 0x00034B5F
	private void OnDrawGizmosSelected()
	{
		if (this.hoseBonePositions != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLineStrip(this.hoseBonePositions, false);
		}
	}

	// Token: 0x04000B3B RID: 2875
	[SerializeField]
	private SkinnedMeshRenderer skinnedMeshRenderer;

	// Token: 0x04000B3C RID: 2876
	[SerializeField]
	private Vector3 localBoundsOverride;

	// Token: 0x04000B3D RID: 2877
	[SerializeField]
	private Transform[] miscBones;

	// Token: 0x04000B3E RID: 2878
	[SerializeField]
	private Transform[] hoseBones;

	// Token: 0x04000B3F RID: 2879
	[SerializeField]
	private float[] hoseBoneMaxDisplacement;

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private CosmeticRefID startAnchorRef;

	// Token: 0x04000B41 RID: 2881
	private Transform startAnchor;

	// Token: 0x04000B42 RID: 2882
	[SerializeField]
	private float startStiffness = 0.5f;

	// Token: 0x04000B43 RID: 2883
	[SerializeField]
	private Transform endAnchor;

	// Token: 0x04000B44 RID: 2884
	[SerializeField]
	private float endStiffness = 0.5f;

	// Token: 0x04000B45 RID: 2885
	private Vector3[] hoseBonePositions;

	// Token: 0x04000B46 RID: 2886
	private Vector3[] hoseBoneVelocities;

	// Token: 0x04000B47 RID: 2887
	[SerializeField]
	private float damping = 0.97f;

	// Token: 0x04000B48 RID: 2888
	private float[] hoseSectionLengths;

	// Token: 0x04000B49 RID: 2889
	private float totalHoseLength;

	// Token: 0x04000B4A RID: 2890
	private bool firstUpdate = true;

	// Token: 0x04000B4B RID: 2891
	private HoseSimulatorAnchors anchors;

	// Token: 0x04000B4C RID: 2892
	[SerializeField]
	private TransferrableObject myHoldable;

	// Token: 0x04000B4D RID: 2893
	private bool isLeftHanded;
}
