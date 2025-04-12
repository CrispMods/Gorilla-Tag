using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class FlashlightController : MonoBehaviour
{
	// Token: 0x0600129F RID: 4767 RVA: 0x000B0C90 File Offset: 0x000AEE90
	private void Start()
	{
		this.localRotation = this.flashlightRoot.localRotation;
		this.localPosition = this.flashlightRoot.localPosition;
		this.skeletons = new OVRSkeleton[2];
		this.hands = new OVRHand[2];
		this.externalController = base.GetComponent<GrabObject>();
		if (this.externalController)
		{
			GrabObject grabObject = this.externalController;
			grabObject.GrabbedObjectDelegate = (GrabObject.GrabbedObject)Delegate.Combine(grabObject.GrabbedObjectDelegate, new GrabObject.GrabbedObject(this.Grab));
			GrabObject grabObject2 = this.externalController;
			grabObject2.ReleasedObjectDelegate = (GrabObject.ReleasedObject)Delegate.Combine(grabObject2.ReleasedObjectDelegate, new GrabObject.ReleasedObject(this.Release));
		}
		if (base.GetComponent<Flashlight>())
		{
			base.GetComponent<Flashlight>().EnableFlashlight(false);
		}
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x000B0D58 File Offset: 0x000AEF58
	private void LateUpdate()
	{
		if (!this.externalController)
		{
			this.FindHands();
			if (OVRInput.GetActiveController() != OVRInput.Controller.RTouch && OVRInput.GetActiveController() != OVRInput.Controller.LTouch && OVRInput.GetActiveController() != OVRInput.Controller.Touch)
			{
				if (this.handIndex >= 0)
				{
					this.AlignWithHand(this.hands[this.handIndex], this.skeletons[this.handIndex]);
				}
				if (this.infoText)
				{
					this.infoText.text = "Pinch to toggle flashlight";
					return;
				}
			}
			else
			{
				this.AlignWithController(OVRInput.Controller.RTouch);
				if (OVRInput.GetUp(OVRInput.RawButton.A, OVRInput.Controller.Active) && base.GetComponent<Flashlight>())
				{
					base.GetComponent<Flashlight>().ToggleFlashlight();
				}
				if (this.infoText)
				{
					this.infoText.text = "Press A to toggle flashlight";
				}
			}
		}
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x000B0E28 File Offset: 0x000AF028
	private void FindHands()
	{
		if (this.skeletons[0] == null || this.skeletons[1] == null)
		{
			OVRSkeleton[] array = UnityEngine.Object.FindObjectsOfType<OVRSkeleton>();
			if (array[0])
			{
				this.skeletons[0] = array[0];
				this.hands[0] = this.skeletons[0].GetComponent<OVRHand>();
				this.handIndex = 0;
			}
			if (array[1])
			{
				this.skeletons[1] = array[1];
				this.hands[1] = this.skeletons[1].GetComponent<OVRHand>();
				this.handIndex = 1;
				return;
			}
		}
		else if (this.handIndex == 0)
		{
			if (this.hands[1].GetFingerIsPinching(OVRHand.HandFinger.Index))
			{
				this.handIndex = 1;
				return;
			}
		}
		else if (this.hands[0].GetFingerIsPinching(OVRHand.HandFinger.Index))
		{
			this.handIndex = 0;
		}
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x000B0EF8 File Offset: 0x000AF0F8
	private void AlignWithHand(OVRHand hand, OVRSkeleton skeleton)
	{
		if (this.pinching)
		{
			if (hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 0.8f)
			{
				this.pinching = false;
			}
		}
		else if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index))
		{
			if (base.GetComponent<Flashlight>())
			{
				base.GetComponent<Flashlight>().ToggleFlashlight();
			}
			this.pinching = true;
		}
		this.flashlightRoot.position = skeleton.Bones[6].Transform.position;
		this.flashlightRoot.rotation = Quaternion.LookRotation(skeleton.Bones[6].Transform.position - skeleton.Bones[0].Transform.position);
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x000B0FB0 File Offset: 0x000AF1B0
	private void AlignWithController(OVRInput.Controller controller)
	{
		base.transform.position = OVRInput.GetLocalControllerPosition(controller);
		base.transform.rotation = OVRInput.GetLocalControllerRotation(controller);
		this.flashlightRoot.localRotation = this.localRotation;
		this.flashlightRoot.localPosition = this.localPosition;
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x000B1004 File Offset: 0x000AF204
	public void Grab(OVRInput.Controller grabHand)
	{
		if (base.GetComponent<Flashlight>())
		{
			base.GetComponent<Flashlight>().EnableFlashlight(true);
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeLighting(new Color(0f, 0f, 0f, 0.95f), 0f, 0.25f));
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x0003BC8D File Offset: 0x00039E8D
	public void Release()
	{
		if (base.GetComponent<Flashlight>())
		{
			base.GetComponent<Flashlight>().EnableFlashlight(false);
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeLighting(Color.clear, 1f, 0.25f));
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x0003BCCA File Offset: 0x00039ECA
	private IEnumerator FadeLighting(Color newColor, float sceneLightIntensity, float fadeTime)
	{
		float timer = 0f;
		Color currentColor = Camera.main.backgroundColor;
		float currentLight = this.sceneLight ? this.sceneLight.intensity : 0f;
		while (timer <= fadeTime)
		{
			timer += Time.deltaTime;
			float t = Mathf.Clamp01(timer / fadeTime);
			Camera.main.backgroundColor = Color.Lerp(currentColor, newColor, t);
			if (this.sceneLight)
			{
				this.sceneLight.intensity = Mathf.Lerp(currentLight, sceneLightIntensity, t);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001488 RID: 5256
	public Light sceneLight;

	// Token: 0x04001489 RID: 5257
	public Transform flashlightRoot;

	// Token: 0x0400148A RID: 5258
	private Vector3 localPosition = Vector3.zero;

	// Token: 0x0400148B RID: 5259
	private Quaternion localRotation = Quaternion.identity;

	// Token: 0x0400148C RID: 5260
	public TextMesh infoText;

	// Token: 0x0400148D RID: 5261
	private GrabObject externalController;

	// Token: 0x0400148E RID: 5262
	private OVRSkeleton[] skeletons;

	// Token: 0x0400148F RID: 5263
	private OVRHand[] hands;

	// Token: 0x04001490 RID: 5264
	private int handIndex = -1;

	// Token: 0x04001491 RID: 5265
	private bool pinching;
}
