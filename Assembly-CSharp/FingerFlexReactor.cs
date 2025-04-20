using System;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public class FingerFlexReactor : MonoBehaviour
{
	// Token: 0x06001FE7 RID: 8167 RVA: 0x000F077C File Offset: 0x000EE97C
	private void Setup()
	{
		this._rig = base.GetComponentInParent<VRRig>();
		if (!this._rig)
		{
			return;
		}
		this._fingers = new VRMap[]
		{
			this._rig.leftThumb,
			this._rig.leftIndex,
			this._rig.leftMiddle,
			this._rig.rightThumb,
			this._rig.rightIndex,
			this._rig.rightMiddle
		};
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x00045B29 File Offset: 0x00043D29
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06001FE9 RID: 8169 RVA: 0x00045B31 File Offset: 0x00043D31
	private void FixedUpdate()
	{
		this.UpdateBlendShapes();
	}

	// Token: 0x06001FEA RID: 8170 RVA: 0x000F0804 File Offset: 0x000EEA04
	public void UpdateBlendShapes()
	{
		if (!this._rig)
		{
			return;
		}
		if (this._blendShapeTargets == null || this._fingers == null)
		{
			return;
		}
		if (this._blendShapeTargets.Length == 0 || this._fingers.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this._blendShapeTargets.Length; i++)
		{
			FingerFlexReactor.BlendShapeTarget blendShapeTarget = this._blendShapeTargets[i];
			if (blendShapeTarget != null)
			{
				int sourceFinger = (int)blendShapeTarget.sourceFinger;
				if (sourceFinger != -1)
				{
					SkinnedMeshRenderer targetRenderer = blendShapeTarget.targetRenderer;
					if (targetRenderer)
					{
						float lerpValue = FingerFlexReactor.GetLerpValue(this._fingers[sourceFinger]);
						Vector2 inputRange = blendShapeTarget.inputRange;
						Vector2 outputRange = blendShapeTarget.outputRange;
						float num = MathUtils.Linear(lerpValue, inputRange.x, inputRange.y, outputRange.x, outputRange.y);
						blendShapeTarget.currentValue = num;
						targetRenderer.SetBlendShapeWeight(blendShapeTarget.blendShapeIndex, num);
					}
				}
			}
		}
	}

	// Token: 0x06001FEB RID: 8171 RVA: 0x000F08D8 File Offset: 0x000EEAD8
	private static float GetLerpValue(VRMap map)
	{
		VRMapThumb vrmapThumb = map as VRMapThumb;
		float result;
		if (vrmapThumb == null)
		{
			VRMapIndex vrmapIndex = map as VRMapIndex;
			if (vrmapIndex == null)
			{
				VRMapMiddle vrmapMiddle = map as VRMapMiddle;
				if (vrmapMiddle == null)
				{
					result = 0f;
				}
				else
				{
					result = vrmapMiddle.calcT;
				}
			}
			else
			{
				result = vrmapIndex.calcT;
			}
		}
		else
		{
			result = ((vrmapThumb.calcT > 0.1f) ? 1f : 0f);
		}
		return result;
	}

	// Token: 0x040023BC RID: 9148
	[SerializeField]
	private VRRig _rig;

	// Token: 0x040023BD RID: 9149
	[SerializeField]
	private VRMap[] _fingers = new VRMap[0];

	// Token: 0x040023BE RID: 9150
	[SerializeField]
	private FingerFlexReactor.BlendShapeTarget[] _blendShapeTargets = new FingerFlexReactor.BlendShapeTarget[0];

	// Token: 0x02000525 RID: 1317
	[Serializable]
	public class BlendShapeTarget
	{
		// Token: 0x040023BF RID: 9151
		public FingerFlexReactor.FingerMap sourceFinger;

		// Token: 0x040023C0 RID: 9152
		public SkinnedMeshRenderer targetRenderer;

		// Token: 0x040023C1 RID: 9153
		public int blendShapeIndex;

		// Token: 0x040023C2 RID: 9154
		public Vector2 inputRange = new Vector2(0f, 1f);

		// Token: 0x040023C3 RID: 9155
		public Vector2 outputRange = new Vector2(0f, 1f);

		// Token: 0x040023C4 RID: 9156
		[NonSerialized]
		public float currentValue;
	}

	// Token: 0x02000526 RID: 1318
	public enum FingerMap
	{
		// Token: 0x040023C6 RID: 9158
		None = -1,
		// Token: 0x040023C7 RID: 9159
		LeftThumb,
		// Token: 0x040023C8 RID: 9160
		LeftIndex,
		// Token: 0x040023C9 RID: 9161
		LeftMiddle,
		// Token: 0x040023CA RID: 9162
		RightThumb,
		// Token: 0x040023CB RID: 9163
		RightIndex,
		// Token: 0x040023CC RID: 9164
		RightMiddle
	}
}
