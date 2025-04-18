﻿using System;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class HandMeshUI : MonoBehaviour
{
	// Token: 0x060012C2 RID: 4802 RVA: 0x0005AEB0 File Offset: 0x000590B0
	private void Start()
	{
		this.SetSliderValue(0, (float)this.rightMask.radialDivisions, false);
		this.SetSliderValue(1, this.rightMask.borderSize, false);
		this.SetSliderValue(2, this.rightMask.fingerTaper, false);
		this.SetSliderValue(3, this.rightMask.fingerTipLength, false);
		this.SetSliderValue(4, this.rightMask.webOffset, false);
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x0005AF20 File Offset: 0x00059120
	private void Update()
	{
		this.CheckForHands();
		Vector3 position = this.rightHand.Bones[20].Transform.position;
		Vector3 position2 = this.leftHand.Bones[20].Transform.position;
		if (this.rightHeldKnob >= 0)
		{
			Vector3 vector = this.knobs[this.rightHeldKnob].transform.parent.InverseTransformPoint(position);
			this.SetSliderValue(this.rightHeldKnob, Mathf.Clamp01(vector.x * 10f), true);
			if (vector.z < -0.02f)
			{
				this.rightHeldKnob = -1;
			}
		}
		else
		{
			for (int i = 0; i < this.knobs.Length; i++)
			{
				if (Vector3.Distance(position, this.knobs[i].transform.position) <= 0.02f && this.leftHeldKnob != i)
				{
					this.rightHeldKnob = i;
					break;
				}
			}
		}
		if (this.leftHeldKnob >= 0)
		{
			Vector3 vector2 = this.knobs[this.leftHeldKnob].transform.parent.InverseTransformPoint(position2);
			this.SetSliderValue(this.leftHeldKnob, Mathf.Clamp01(vector2.x * 10f), true);
			if (vector2.z < -0.02f)
			{
				this.leftHeldKnob = -1;
				return;
			}
		}
		else
		{
			for (int j = 0; j < this.knobs.Length; j++)
			{
				if (Vector3.Distance(position2, this.knobs[j].transform.position) <= 0.02f && this.rightHeldKnob != j)
				{
					this.leftHeldKnob = j;
					return;
				}
			}
		}
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x0005B0B4 File Offset: 0x000592B4
	private void SetSliderValue(int sliderID, float value, bool isNormalized)
	{
		float num = 0f;
		float num2 = 1f;
		float d = 0.1f;
		string format = "";
		switch (sliderID)
		{
		case 0:
			num = 2f;
			num2 = 16f;
			format = "{0, 0:0}";
			break;
		case 1:
			num = 0f;
			num2 = 0.05f;
			format = "{0, 0:0.000}";
			break;
		case 2:
			num = 0f;
			num2 = 0.3333f;
			format = "{0, 0:0.00}";
			break;
		case 3:
			num = 0.5f;
			num2 = 1.5f;
			format = "{0, 0:0.00}";
			break;
		case 4:
			num = 0f;
			num2 = 1f;
			format = "{0, 0:0.00}";
			break;
		}
		float num3 = isNormalized ? (value * (num2 - num) + num) : value;
		float d2 = isNormalized ? value : ((value - num) / (num2 - num));
		this.knobs[sliderID].transform.localPosition = Vector3.right * d2 * d;
		this.readouts[sliderID].text = string.Format(format, num3);
		switch (sliderID)
		{
		case 0:
			this.rightMask.radialDivisions = (int)num3;
			this.leftMask.radialDivisions = (int)num3;
			return;
		case 1:
			this.rightMask.borderSize = num3;
			this.leftMask.borderSize = num3;
			return;
		case 2:
			this.rightMask.fingerTaper = num3;
			this.leftMask.fingerTaper = num3;
			return;
		case 3:
			this.rightMask.fingerTipLength = num3;
			this.leftMask.fingerTipLength = num3;
			return;
		case 4:
			this.rightMask.webOffset = num3;
			this.leftMask.webOffset = num3;
			return;
		default:
			return;
		}
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x0005B258 File Offset: 0x00059458
	private void CheckForHands()
	{
		bool flag = OVRInput.GetActiveController() == OVRInput.Controller.Hands || OVRInput.GetActiveController() == OVRInput.Controller.LHand || OVRInput.GetActiveController() == OVRInput.Controller.RHand;
		if (base.transform.GetChild(0).gameObject.activeSelf)
		{
			if (!flag)
			{
				base.transform.GetChild(0).gameObject.SetActive(false);
				this.leftHeldKnob = -1;
				this.rightHeldKnob = -1;
				return;
			}
		}
		else if (flag)
		{
			base.transform.GetChild(0).gameObject.SetActive(true);
			base.transform.position = (this.rightHand.Bones[20].Transform.position + this.rightHand.Bones[20].Transform.position) * 0.5f;
			base.transform.position += (base.transform.position - Camera.main.transform.position).normalized * 0.1f;
			base.transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z));
		}
	}

	// Token: 0x040014BA RID: 5306
	public SphereCollider[] knobs;

	// Token: 0x040014BB RID: 5307
	public TextMesh[] readouts;

	// Token: 0x040014BC RID: 5308
	private int rightHeldKnob = -1;

	// Token: 0x040014BD RID: 5309
	private int leftHeldKnob = -1;

	// Token: 0x040014BE RID: 5310
	public OVRSkeleton leftHand;

	// Token: 0x040014BF RID: 5311
	public OVRSkeleton rightHand;

	// Token: 0x040014C0 RID: 5312
	public HandMeshMask leftMask;

	// Token: 0x040014C1 RID: 5313
	public HandMeshMask rightMask;
}
