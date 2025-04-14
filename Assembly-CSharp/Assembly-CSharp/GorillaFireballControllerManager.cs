using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x0200068E RID: 1678
public class GorillaFireballControllerManager : MonoBehaviour
{
	// Token: 0x060029C1 RID: 10689 RVA: 0x000CF330 File Offset: 0x000CD530
	private void Update()
	{
		if (!this.hasInitialized)
		{
			this.hasInitialized = true;
			List<InputDevice> list = new List<InputDevice>();
			List<InputDevice> list2 = new List<InputDevice>();
			InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, list);
			InputDevices.GetDevicesAtXRNode(XRNode.RightHand, list2);
			if (list.Count == 1)
			{
				this.leftHand = list[0];
			}
			if (list2.Count == 1)
			{
				this.rightHand = list2[0];
			}
		}
		float axis = SteamVR_Actions.gorillaTag_LeftTriggerFloat.GetAxis(SteamVR_Input_Sources.LeftHand);
		if (this.leftHandLastState <= this.throwingThreshold && axis > this.throwingThreshold)
		{
			this.CreateFireball(true);
		}
		else if (this.leftHandLastState >= this.throwingThreshold && axis < this.throwingThreshold)
		{
			this.TryThrowFireball(true);
		}
		this.leftHandLastState = axis;
		axis = SteamVR_Actions.gorillaTag_RightTriggerFloat.GetAxis(SteamVR_Input_Sources.RightHand);
		if (this.rightHandLastState <= this.throwingThreshold && axis > this.throwingThreshold)
		{
			this.CreateFireball(false);
		}
		else if (this.rightHandLastState >= this.throwingThreshold && axis < this.throwingThreshold)
		{
			this.TryThrowFireball(false);
		}
		this.rightHandLastState = axis;
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x000CF434 File Offset: 0x000CD634
	public void TryThrowFireball(bool isLeftHand)
	{
		if (isLeftHand && GorillaPlaySpace.Instance.myVRRig.leftHandTransform.GetComponentInChildren<GorillaFireball>() != null)
		{
			GorillaPlaySpace.Instance.myVRRig.leftHandTransform.GetComponentInChildren<GorillaFireball>().ThrowThisThingo();
			return;
		}
		if (!isLeftHand && GorillaPlaySpace.Instance.myVRRig.rightHandTransform.GetComponentInChildren<GorillaFireball>() != null)
		{
			GorillaPlaySpace.Instance.myVRRig.rightHandTransform.GetComponentInChildren<GorillaFireball>().ThrowThisThingo();
		}
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x000CF4B4 File Offset: 0x000CD6B4
	public void CreateFireball(bool isLeftHand)
	{
		object[] array = new object[1];
		Vector3 position;
		if (isLeftHand)
		{
			array[0] = true;
			position = GorillaPlaySpace.Instance.myVRRig.leftHandTransform.position;
		}
		else
		{
			array[0] = false;
			position = GorillaPlaySpace.Instance.myVRRig.rightHandTransform.position;
		}
		PhotonNetwork.Instantiate("GorillaPrefabs/GorillaFireball", position, Quaternion.identity, 0, array);
	}

	// Token: 0x04002F0A RID: 12042
	public InputDevice leftHand;

	// Token: 0x04002F0B RID: 12043
	public InputDevice rightHand;

	// Token: 0x04002F0C RID: 12044
	public bool hasInitialized;

	// Token: 0x04002F0D RID: 12045
	public float leftHandLastState;

	// Token: 0x04002F0E RID: 12046
	public float rightHandLastState;

	// Token: 0x04002F0F RID: 12047
	public float throwingThreshold = 0.9f;
}
