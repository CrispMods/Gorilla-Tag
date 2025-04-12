using System;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class GorillaGestureTracker : MonoBehaviour
{
	// Token: 0x060008BE RID: 2238 RVA: 0x000353AA File Offset: 0x000335AA
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0008DBDC File Offset: 0x0008BDDC
	private void Setup()
	{
		if (this._rig.AsNull<VRRig>() == null)
		{
			this._rig = base.GetComponentInChildren<VRRig>();
		}
		if (this._rig.AsNull<VRRig>() == null)
		{
			return;
		}
		this._rigTransform = this._rig.transform;
		this._vrNodes[1] = this._rig.rightHand;
		this._vrNodes[5] = this._rig.rightThumb;
		this._vrNodes[6] = this._rig.rightIndex;
		this._vrNodes[7] = this._rig.rightMiddle;
		this._vrNodes[8] = this._rig.leftHand;
		this._vrNodes[12] = this._rig.leftThumb;
		this._vrNodes[13] = this._rig.leftIndex;
		this._vrNodes[14] = this._rig.leftMiddle;
		foreach (Transform transform in this._rig.mainSkin.bones)
		{
			string name = transform.name;
			if (name.Contains("head", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[0] = transform;
			}
			else if (name.Contains("hand.R", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[1] = transform;
			}
			else if (name.Contains("thumb.03.R", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[5] = transform;
			}
			else if (name.Contains("f_index.02.R", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[6] = transform;
			}
			else if (name.Contains("f_middle.02.R", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[7] = transform;
			}
			else if (name.Contains("hand.L", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[8] = transform;
			}
			else if (name.Contains("thumb.03.L", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[12] = transform;
			}
			else if (name.Contains("f_index.02.L", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[13] = transform;
			}
			else if (name.Contains("f_middle.02.L", StringComparison.OrdinalIgnoreCase))
			{
				this._bones[14] = transform;
			}
		}
		this._matchesR = new bool[this._gestures.Length];
		this._matchesL = new bool[this._gestures.Length];
		this._setupDone = true;
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x000353B2 File Offset: 0x000335B2
	private void FixedUpdate()
	{
		this.PollNodes();
		this.PollGestures();
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x0008DE10 File Offset: 0x0008C010
	private void PollGestures()
	{
		if (this._gestures == null)
		{
			return;
		}
		int num = this._gestures.Length;
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < num; i++)
		{
			this.PollGesture(1, i, deltaTime, ref this._matchesR);
			this.PollGesture(8, i, deltaTime, ref this._matchesL);
		}
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x0008DE60 File Offset: 0x0008C060
	private void PollNodes()
	{
		this.PollFace(0);
		this.PollHandAxes(1);
		int num;
		this.PollThumb(5, out num);
		int num2;
		this.PollIndex(6, out num2);
		int num3;
		this.PollMiddle(7, out num3);
		this.PollHandAxes(8);
		int num4;
		this.PollThumb(12, out num4);
		int num5;
		this.PollIndex(13, out num5);
		int num6;
		this.PollMiddle(14, out num6);
		this._flexes[1] = num + 1 + (num2 + 1) + (num3 + 1);
		this._flexes[8] = num4 + 1 + (num5 + 1) + (num6 + 1);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0008DEE4 File Offset: 0x0008C0E4
	private void PollThumb(int i, out int flex)
	{
		VRMapThumb vrmapThumb = (VRMapThumb)this._vrNodes[i];
		Transform transform = this._bones[i];
		float num = 0f;
		bool flag = vrmapThumb.primaryButtonTouch || vrmapThumb.secondaryButtonTouch;
		bool flag2 = vrmapThumb.primaryButtonPress || vrmapThumb.secondaryButtonPress;
		if (flag)
		{
			num = 0.1f;
		}
		if (flag2)
		{
			num = 1f;
		}
		flex = -1;
		if (flag2)
		{
			flex = 1;
		}
		else if (!flag)
		{
			flex = 0;
		}
		Vector3 position = transform.position;
		Vector3 up = transform.up;
		this._positions[i] = position;
		this._normals[i] = up;
		this._inputs[i] = num;
		this._flexes[i] = flex;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0008DF94 File Offset: 0x0008C194
	private void PollIndex(int i, out int flex)
	{
		VRMapIndex vrmapIndex = (VRMapIndex)this._vrNodes[i];
		Transform transform = this._bones[i];
		float num = Mathf.Clamp01(vrmapIndex.triggerValue / 0.88f);
		flex = -1;
		if (num.Approx(0f, 1E-06f))
		{
			flex = 0;
		}
		if (num.Approx(1f, 1E-06f))
		{
			flex = 1;
		}
		Vector3 position = transform.position;
		Vector3 up = transform.up;
		this._positions[i] = position;
		this._normals[i] = up;
		this._inputs[i] = num;
		this._flexes[i] = flex;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0008E030 File Offset: 0x0008C230
	private void PollMiddle(int i, out int flex)
	{
		VRMapMiddle vrmapMiddle = (VRMapMiddle)this._vrNodes[i];
		Transform transform = this._bones[i];
		float gripValue = vrmapMiddle.gripValue;
		flex = -1;
		if (gripValue.Approx(0f, 1E-06f))
		{
			flex = 0;
		}
		if (gripValue.Approx(1f, 1E-06f))
		{
			flex = 1;
		}
		Vector3 position = transform.position;
		Vector3 up = transform.up;
		this._positions[i] = position;
		this._normals[i] = up;
		this._inputs[i] = gripValue;
		this._flexes[i] = flex;
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0008E0C4 File Offset: 0x0008C2C4
	private void PollGesture(int hand, int i, float dt, ref bool[] results)
	{
		results[i] = false;
		GorillaHandGesture gorillaHandGesture = this._gestures[i];
		if (!gorillaHandGesture.track)
		{
			return;
		}
		GestureNode[] nodes = gorillaHandGesture.nodes;
		int num = 0;
		int num2 = 0;
		this.TrackHand(hand, (GestureHandNode)nodes[0], ref num, ref num2);
		this.TrackHandAxis(hand + 1, nodes[1], ref num, ref num2);
		this.TrackHandAxis(hand + 2, nodes[2], ref num, ref num2);
		this.TrackHandAxis(hand + 3, nodes[3], ref num, ref num2);
		this.TrackDigit(hand + 4, (GestureDigitNode)nodes[4], ref num, ref num2);
		this.TrackDigit(hand + 5, (GestureDigitNode)nodes[5], ref num, ref num2);
		this.TrackDigit(hand + 6, (GestureDigitNode)nodes[6], ref num, ref num2);
		results[i] = (num == num2);
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0008E180 File Offset: 0x0008C380
	private void TrackHand(int hand, GestureHandNode node, ref int tracked, ref int matches)
	{
		if (!node.track)
		{
			return;
		}
		GestureHandState state = node.state;
		if ((state & GestureHandState.IsLeft) == GestureHandState.IsLeft)
		{
			tracked++;
			if (hand == 8)
			{
				matches++;
			}
		}
		if ((state & GestureHandState.IsRight) == GestureHandState.IsRight)
		{
			tracked++;
			if (hand == 1)
			{
				matches++;
			}
		}
		if ((state & GestureHandState.Open) == GestureHandState.Open)
		{
			tracked++;
			if (this._flexes[hand] == 3)
			{
				matches++;
			}
		}
		if ((state & GestureHandState.Closed) == GestureHandState.Closed)
		{
			tracked++;
			if (this._flexes[hand] == 6)
			{
				matches++;
			}
		}
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0008E20C File Offset: 0x0008C40C
	private void TrackHandAxis(int axis, GestureNode node, ref int tracked, ref int matches)
	{
		if (!node.track)
		{
			return;
		}
		GestureAlignment alignment = node.alignment;
		Vector3 lhs = this._normals[axis];
		Vector3 rhs = this._normals[0];
		float num = Vector3.Dot(lhs, Vector3.up);
		float num2 = -num;
		float num3 = Vector3.Dot(lhs, rhs);
		float num4 = -num3;
		if ((alignment & GestureAlignment.WorldUp) == GestureAlignment.WorldUp)
		{
			tracked++;
			if (num > 1E-05f)
			{
				matches++;
			}
		}
		if ((alignment & GestureAlignment.WorldDown) == GestureAlignment.WorldDown)
		{
			tracked++;
			if (num2 > 1E-05f)
			{
				matches++;
			}
		}
		if ((alignment & GestureAlignment.TowardFace) == GestureAlignment.TowardFace)
		{
			tracked++;
			if (num3 > 1E-05f)
			{
				matches++;
			}
		}
		if ((alignment & GestureAlignment.AwayFromFace) == GestureAlignment.AwayFromFace)
		{
			tracked++;
			if (num4 > 1E-05f)
			{
				matches++;
			}
		}
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0008E2EC File Offset: 0x0008C4EC
	private void TrackDigit(int digit, GestureDigitNode node, ref int tracked, ref int matches)
	{
		if (!node.track)
		{
			return;
		}
		GestureAlignment alignment = node.alignment;
		GestureDigitFlexion flexion = node.flexion;
		Vector3 lhs = this._normals[digit];
		Vector3 rhs = this._normals[0];
		int num = this._flexes[digit];
		bool flag = num == 0;
		bool flag2 = num == 1;
		bool flag3 = num == -1;
		float num2 = Vector3.Dot(lhs, Vector3.up);
		float num3 = -num2;
		float num4 = Vector3.Dot(lhs, rhs);
		float num5 = -num4;
		if ((alignment & GestureAlignment.WorldUp) == GestureAlignment.WorldUp)
		{
			tracked++;
			if (num2 > 1E-05f)
			{
				matches++;
			}
		}
		if ((alignment & GestureAlignment.WorldDown) == GestureAlignment.WorldDown)
		{
			tracked++;
			if (num3 > 1E-05f)
			{
				matches++;
			}
		}
		if ((alignment & GestureAlignment.TowardFace) == GestureAlignment.TowardFace)
		{
			tracked++;
			if (num4 > 1E-05f)
			{
				matches++;
			}
		}
		if ((alignment & GestureAlignment.AwayFromFace) == GestureAlignment.AwayFromFace)
		{
			tracked++;
			if (num5 > 1E-05f)
			{
				matches++;
			}
		}
		if ((flexion & GestureDigitFlexion.Bent) == GestureDigitFlexion.Bent)
		{
			tracked++;
			if (flag3)
			{
				matches++;
			}
		}
		if ((flexion & GestureDigitFlexion.Open) == GestureDigitFlexion.Open)
		{
			tracked++;
			if (flag)
			{
				matches++;
			}
		}
		if ((flexion & GestureDigitFlexion.Closed) == GestureDigitFlexion.Closed)
		{
			tracked++;
			if (flag2)
			{
				matches++;
			}
		}
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0008E440 File Offset: 0x0008C640
	private void PollFace(int index)
	{
		Transform transform = this._bones[index];
		this._positions[index] = transform.TransformPoint(this._faceBasisOffset);
		this._normals[index] = this._faceBasisAngles * transform.forward;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0008E48C File Offset: 0x0008C68C
	private void PollHandAxes(int hand)
	{
		bool flag = hand == 1;
		bool flag2 = hand == 8;
		int num = hand + 1;
		int num2 = hand + 2;
		int num3 = hand + 3;
		Transform transform = this._bones[hand];
		Vector3 handBasisAngles = this._handBasisAngles;
		if (flag2)
		{
			handBasisAngles.z *= -1f;
		}
		Quaternion rotation = transform.rotation * Quaternion.Euler(handBasisAngles);
		this._positions[hand] = transform.position;
		this._normals[num] = rotation * Vector3.right * (flag ? 1f : -1f);
		this._normals[num2] = rotation * Vector3.forward;
		this._normals[num3] = rotation * Vector3.up;
	}

	// Token: 0x04000A99 RID: 2713
	[SerializeField]
	private VRRig _rig;

	// Token: 0x04000A9A RID: 2714
	[SerializeField]
	private Transform _rigTransform;

	// Token: 0x04000A9B RID: 2715
	public const int N_FACE = 0;

	// Token: 0x04000A9C RID: 2716
	public const int R_HAND = 1;

	// Token: 0x04000A9D RID: 2717
	public const int R_PALM = 2;

	// Token: 0x04000A9E RID: 2718
	public const int R_WRIST = 3;

	// Token: 0x04000A9F RID: 2719
	public const int R_DIGITS = 4;

	// Token: 0x04000AA0 RID: 2720
	public const int R_THUMB = 5;

	// Token: 0x04000AA1 RID: 2721
	public const int R_INDEX = 6;

	// Token: 0x04000AA2 RID: 2722
	public const int R_MIDDLE = 7;

	// Token: 0x04000AA3 RID: 2723
	public const int L_HAND = 8;

	// Token: 0x04000AA4 RID: 2724
	public const int L_PALM = 9;

	// Token: 0x04000AA5 RID: 2725
	public const int L_WRIST = 10;

	// Token: 0x04000AA6 RID: 2726
	public const int L_DIGITS = 11;

	// Token: 0x04000AA7 RID: 2727
	public const int L_THUMB = 12;

	// Token: 0x04000AA8 RID: 2728
	public const int L_INDEX = 13;

	// Token: 0x04000AA9 RID: 2729
	public const int L_MIDDLE = 14;

	// Token: 0x04000AAA RID: 2730
	public const int N_SIZE = 15;

	// Token: 0x04000AAB RID: 2731
	[Space]
	[SerializeField]
	private Vector3 _handBasisAngles = new Vector3(0f, 2f, 341f);

	// Token: 0x04000AAC RID: 2732
	[Space]
	[SerializeField]
	private Vector3 _faceBasisOffset = new Vector3(0f, 0.1f, 0.136f);

	// Token: 0x04000AAD RID: 2733
	[SerializeField]
	private Quaternion _faceBasisAngles = Quaternion.Euler(-8f, 0f, 0f);

	// Token: 0x04000AAE RID: 2734
	[Space]
	[SerializeField]
	private bool _debug;

	// Token: 0x04000AAF RID: 2735
	[NonSerialized]
	private bool _setupDone;

	// Token: 0x04000AB0 RID: 2736
	public static uint TickRate = 24U;

	// Token: 0x04000AB1 RID: 2737
	[Space]
	[SerializeField]
	private Transform[] _bones = new Transform[15];

	// Token: 0x04000AB2 RID: 2738
	[NonSerialized]
	private VRMap[] _vrNodes = new VRMap[15];

	// Token: 0x04000AB3 RID: 2739
	[NonSerialized]
	private float[] _inputs = new float[15];

	// Token: 0x04000AB4 RID: 2740
	[NonSerialized]
	private int[] _flexes = new int[15];

	// Token: 0x04000AB5 RID: 2741
	[NonSerialized]
	private Vector3[] _normals = new Vector3[15];

	// Token: 0x04000AB6 RID: 2742
	[NonSerialized]
	private Vector3[] _positions = new Vector3[15];

	// Token: 0x04000AB7 RID: 2743
	[Space]
	[SerializeField]
	private GorillaHandGesture[] _gestures = new GorillaHandGesture[0];

	// Token: 0x04000AB8 RID: 2744
	[NonSerialized]
	private bool[] _matchesR = new bool[0];

	// Token: 0x04000AB9 RID: 2745
	[NonSerialized]
	private bool[] _matchesL = new bool[0];

	// Token: 0x04000ABA RID: 2746
	private const int H_BENT = 0;

	// Token: 0x04000ABB RID: 2747
	private const int H_OPEN = 3;

	// Token: 0x04000ABC RID: 2748
	private const int H_CLOSED = 6;

	// Token: 0x04000ABD RID: 2749
	private const int N_HAND = 0;

	// Token: 0x04000ABE RID: 2750
	private const int A_PALM = 1;

	// Token: 0x04000ABF RID: 2751
	private const int A_WRIST = 2;

	// Token: 0x04000AC0 RID: 2752
	private const int A_DIGITS = 3;

	// Token: 0x04000AC1 RID: 2753
	private const int D_THUMB = 4;

	// Token: 0x04000AC2 RID: 2754
	private const int D_INDEX = 5;

	// Token: 0x04000AC3 RID: 2755
	private const int D_MIDDLE = 6;
}
