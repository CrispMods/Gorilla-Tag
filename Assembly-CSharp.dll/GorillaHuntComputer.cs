﻿using System;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTag;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200056B RID: 1387
public class GorillaHuntComputer : MonoBehaviour
{
	// Token: 0x06002231 RID: 8753 RVA: 0x000F6004 File Offset: 0x000F4204
	private void Update()
	{
		if (!NetworkSystem.Instance.InRoom || GorillaGameManager.instance == null)
		{
			GorillaTagger.Instance.offlineVRRig.huntComputer.SetActive(false);
			return;
		}
		if (this.huntManager == null)
		{
			this.huntManager = GorillaGameManager.instance.gameObject.GetComponent<GorillaHuntManager>();
			if (this.huntManager == null)
			{
				GorillaTagger.Instance.offlineVRRig.huntComputer.SetActive(false);
				return;
			}
		}
		if (!this.huntManager.huntStarted)
		{
			if (this.huntManager.waitingToStartNextHuntGame && this.huntManager.currentTarget.Contains(NetworkSystem.Instance.LocalPlayer) && !this.huntManager.currentHunted.Contains(NetworkSystem.Instance.LocalPlayer) && this.huntManager.countDownTime == 0)
			{
				this.material.gameObject.SetActive(false);
				this.hat.gameObject.SetActive(false);
				this.face.gameObject.SetActive(false);
				this.badge.gameObject.SetActive(false);
				this.leftHand.gameObject.SetActive(false);
				this.rightHand.gameObject.SetActive(false);
				this.text.text = "YOU WON! CONGRATS, HUNTER!";
				return;
			}
			if (this.huntManager.countDownTime != 0)
			{
				this.material.gameObject.SetActive(false);
				this.hat.gameObject.SetActive(false);
				this.face.gameObject.SetActive(false);
				this.badge.gameObject.SetActive(false);
				this.leftHand.gameObject.SetActive(false);
				this.rightHand.gameObject.SetActive(false);
				this.text.text = "GAME STARTING IN:\n" + this.huntManager.countDownTime.ToString() + "...";
				return;
			}
			this.material.gameObject.SetActive(false);
			this.hat.gameObject.SetActive(false);
			this.face.gameObject.SetActive(false);
			this.badge.gameObject.SetActive(false);
			this.leftHand.gameObject.SetActive(false);
			this.rightHand.gameObject.SetActive(false);
			this.text.text = "WAITING TO START";
			return;
		}
		else
		{
			this.myTarget = this.huntManager.GetTargetOf(NetworkSystem.Instance.LocalPlayer);
			if (this.myTarget == null || !this.myTarget.InRoom)
			{
				this.material.gameObject.SetActive(false);
				this.hat.gameObject.SetActive(false);
				this.face.gameObject.SetActive(false);
				this.badge.gameObject.SetActive(false);
				this.leftHand.gameObject.SetActive(false);
				this.rightHand.gameObject.SetActive(false);
				this.text.text = "YOU ARE DEAD\nTAG OTHERS\nTO SLOW THEM";
				return;
			}
			RigContainer rigContainer;
			if (VRRigCache.Instance.TryGetVrrig(this.myTarget, out rigContainer))
			{
				this.myRig = rigContainer.Rig;
				if (this.myRig != null)
				{
					this.material.material = this.myRig.materialsToChangeTo[this.myRig.setMatIndex];
					Text text = this.text;
					string[] array = new string[5];
					array[0] = "TARGET:\n";
					int num = 1;
					bool doIt = true;
					NetPlayer creator = this.myRig.creator;
					array[num] = this.NormalizeName(doIt, (creator != null) ? creator.NickName : null);
					array[2] = "\nDISTANCE: ";
					array[3] = Mathf.CeilToInt((GTPlayer.Instance.headCollider.transform.position - this.myRig.transform.position).magnitude).ToString();
					array[4] = "M";
					text.text = string.Concat(array);
					this.SetImage(this.myRig.cosmeticSet.items[0].displayName, ref this.hat);
					this.SetImage(this.myRig.cosmeticSet.items[2].displayName, ref this.face);
					this.SetImage(this.myRig.cosmeticSet.items[1].displayName, ref this.badge);
					this.SetImage(this.GetPrioritizedItemForHand(this.myRig, true).displayName, ref this.leftHand);
					this.SetImage(this.GetPrioritizedItemForHand(this.myRig, false).displayName, ref this.rightHand);
					this.material.gameObject.SetActive(true);
				}
			}
			return;
		}
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x000F64D0 File Offset: 0x000F46D0
	private void SetImage(string itemDisplayName, ref Image image)
	{
		this.tempItem = CosmeticsController.instance.GetItemFromDict(CosmeticsController.instance.GetItemNameFromDisplayName(itemDisplayName));
		if (this.tempItem.displayName != "NOTHING" && this.myRig != null && this.myRig.IsItemAllowed(this.tempItem.itemName))
		{
			image.gameObject.SetActive(true);
			image.sprite = this.tempItem.itemPicture;
			return;
		}
		image.gameObject.SetActive(false);
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000F6568 File Offset: 0x000F4768
	public string NormalizeName(bool doIt, string text)
	{
		if (doIt)
		{
			if (GorillaComputer.instance.CheckAutoBanListForName(text))
			{
				text = new string(Array.FindAll<char>(text.ToCharArray(), (char c) => char.IsLetterOrDigit(c)));
				if (text.Length > 12)
				{
					text = text.Substring(0, 11);
				}
				text = text.ToUpper();
			}
			else
			{
				text = "BADGORILLA";
			}
		}
		return text;
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000F65E0 File Offset: 0x000F47E0
	public CosmeticsController.CosmeticItem GetPrioritizedItemForHand(VRRig targetRig, bool forLeftHand)
	{
		if (forLeftHand)
		{
			CosmeticsController.CosmeticItem cosmeticItem = targetRig.cosmeticSet.items[7];
			if (cosmeticItem.displayName != "null")
			{
				return cosmeticItem;
			}
			cosmeticItem = targetRig.cosmeticSet.items[4];
			if (cosmeticItem.displayName != "null")
			{
				return cosmeticItem;
			}
			return targetRig.cosmeticSet.items[5];
		}
		else
		{
			CosmeticsController.CosmeticItem cosmeticItem = targetRig.cosmeticSet.items[8];
			if (cosmeticItem.displayName != "null")
			{
				return cosmeticItem;
			}
			cosmeticItem = targetRig.cosmeticSet.items[3];
			if (cosmeticItem.displayName != "null")
			{
				return cosmeticItem;
			}
			return targetRig.cosmeticSet.items[6];
		}
	}

	// Token: 0x040025B6 RID: 9654
	public Text text;

	// Token: 0x040025B7 RID: 9655
	public Image material;

	// Token: 0x040025B8 RID: 9656
	public Image hat;

	// Token: 0x040025B9 RID: 9657
	public Image face;

	// Token: 0x040025BA RID: 9658
	public Image badge;

	// Token: 0x040025BB RID: 9659
	public Image leftHand;

	// Token: 0x040025BC RID: 9660
	public Image rightHand;

	// Token: 0x040025BD RID: 9661
	public NetPlayer myTarget;

	// Token: 0x040025BE RID: 9662
	public NetPlayer tempTarget;

	// Token: 0x040025BF RID: 9663
	[DebugReadout]
	public VRRig myRig;

	// Token: 0x040025C0 RID: 9664
	public Sprite tempSprite;

	// Token: 0x040025C1 RID: 9665
	public CosmeticsController.CosmeticItem tempItem;

	// Token: 0x040025C2 RID: 9666
	private GorillaHuntManager huntManager;
}
