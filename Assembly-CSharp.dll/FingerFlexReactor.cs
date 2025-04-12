using System;
using UnityEngine;

// Token: 0x02000517 RID: 1303
public class FingerFlexReactor : MonoBehaviour
{
	// Token: 0x06001F91 RID: 8081 RVA: 0x000ED9F8 File Offset: 0x000EBBF8
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

	// Token: 0x06001F92 RID: 8082 RVA: 0x0004478A File Offset: 0x0004298A
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x00044792 File Offset: 0x00042992
	private void FixedUpdate()
	{
		this.UpdateBlendShapes();
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x000EDA80 File Offset: 0x000EBC80
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

	// Token: 0x06001F95 RID: 8085 RVA: 0x000EDB54 File Offset: 0x000EBD54
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

	// Token: 0x0400236A RID: 9066
	[SerializeField]
	private VRRig _rig;

	// Token: 0x0400236B RID: 9067
	[SerializeField]
	private VRMap[] _fingers = new VRMap[0];

	// Token: 0x0400236C RID: 9068
	[SerializeField]
	private FingerFlexReactor.BlendShapeTarget[] _blendShapeTargets = new FingerFlexReactor.BlendShapeTarget[0];

	// Token: 0x02000518 RID: 1304
	[Serializable]
	public class BlendShapeTarget
	{
		// Token: 0x0400236D RID: 9069
		public FingerFlexReactor.FingerMap sourceFinger;

		// Token: 0x0400236E RID: 9070
		public SkinnedMeshRenderer targetRenderer;

		// Token: 0x0400236F RID: 9071
		public int blendShapeIndex;

		// Token: 0x04002370 RID: 9072
		public Vector2 inputRange = new Vector2(0f, 1f);

		// Token: 0x04002371 RID: 9073
		public Vector2 outputRange = new Vector2(0f, 1f);

		// Token: 0x04002372 RID: 9074
		[NonSerialized]
		public float currentValue;
	}

	// Token: 0x02000519 RID: 1305
	public enum FingerMap
	{
		// Token: 0x04002374 RID: 9076
		None = -1,
		// Token: 0x04002375 RID: 9077
		LeftThumb,
		// Token: 0x04002376 RID: 9078
		LeftIndex,
		// Token: 0x04002377 RID: 9079
		LeftMiddle,
		// Token: 0x04002378 RID: 9080
		RightThumb,
		// Token: 0x04002379 RID: 9081
		RightIndex,
		// Token: 0x0400237A RID: 9082
		RightMiddle
	}
}
