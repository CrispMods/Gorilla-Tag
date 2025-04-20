using System;
using GorillaExtensions;
using GorillaTagScripts.ModIO;
using TMPro;
using UnityEngine;

// Token: 0x020006A8 RID: 1704
public class VirtualStumpTeleporter : MonoBehaviour, IBuildValidation
{
	// Token: 0x06002A63 RID: 10851 RVA: 0x0004C9FF File Offset: 0x0004ABFF
	public bool BuildValidationCheck()
	{
		if (this.mySerializer == null && base.transform.parent.parent.GetComponentInChildren<VirtualStumpTeleporterSerializer>() == null)
		{
			Debug.LogError("This teleporter needs a reference to a VirtualStumpTeleporterSerializer, or to be placed alongside a VirtualStumpTeleporterSerializer. Check out the arcade or the stump", this);
			return false;
		}
		return true;
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x0011C5DC File Offset: 0x0011A7DC
	public void OnEnable()
	{
		if (this.mySerializer == null)
		{
			this.mySerializer = base.transform.parent.parent.GetComponentInChildren<VirtualStumpTeleporterSerializer>();
		}
		ModIOManager.ModIOEnabledEvent -= this.OnModIOEnabled;
		ModIOManager.ModIOEnabledEvent += this.OnModIOEnabled;
		ModIOManager.ModIODisabledEvent -= this.OnModIODisabled;
		ModIOManager.ModIODisabledEvent += this.OnModIODisabled;
		if (ModIOManager.IsDisabled)
		{
			this.HideHandHolds();
			return;
		}
		this.ShowHandHolds();
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x0004CA3A File Offset: 0x0004AC3A
	public void OnDisable()
	{
		ModIOManager.ModIOEnabledEvent -= this.OnModIOEnabled;
		ModIOManager.ModIODisabledEvent -= this.OnModIODisabled;
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x0004CA5E File Offset: 0x0004AC5E
	private void OnModIOEnabled()
	{
		this.ShowHandHolds();
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x0004CA66 File Offset: 0x0004AC66
	private void OnModIODisabled()
	{
		this.HideHandHolds();
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x0011C66C File Offset: 0x0011A86C
	public void OnTriggerEnter(Collider other)
	{
		if (ModIOManager.IsDisabled || this.teleporting || CustomMapManager.WaitingForRoomJoin || CustomMapManager.WaitingForDisconnect)
		{
			return;
		}
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			this.triggerEntryTime = Time.time;
			this.ShowCountdownText();
		}
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x0011C6C4 File Offset: 0x0011A8C4
	public void OnTriggerStay(Collider other)
	{
		if (ModIOManager.IsDisabled)
		{
			return;
		}
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject && this.triggerEntryTime >= 0f)
		{
			this.UpdateCountdownText();
			if (!this.teleporting && this.triggerEntryTime + this.stayInTriggerDuration <= Time.time)
			{
				this.TeleportPlayer();
				this.HideCountdownText();
			}
		}
	}

	// Token: 0x06002A6A RID: 10858 RVA: 0x0004CA6E File Offset: 0x0004AC6E
	public void OnTriggerExit(Collider other)
	{
		if (ModIOManager.IsDisabled)
		{
			return;
		}
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			this.triggerEntryTime = -1f;
			this.HideCountdownText();
		}
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x0011C730 File Offset: 0x0011A930
	private void ShowCountdownText()
	{
		if (ModIOManager.IsDisabled)
		{
			return;
		}
		if (!this.countdownTexts.IsNullOrEmpty<TMP_Text>())
		{
			int num = 1 + Mathf.FloorToInt(this.stayInTriggerDuration);
			for (int i = 0; i < this.countdownTexts.Length; i++)
			{
				if (!this.countdownTexts[i].IsNull())
				{
					this.countdownTexts[i].text = num.ToString();
					this.countdownTexts[i].gameObject.SetActive(true);
				}
			}
		}
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x0011C7AC File Offset: 0x0011A9AC
	private void HideCountdownText()
	{
		if (!this.countdownTexts.IsNullOrEmpty<TMP_Text>())
		{
			for (int i = 0; i < this.countdownTexts.Length; i++)
			{
				if (!this.countdownTexts[i].IsNull())
				{
					this.countdownTexts[i].text = "";
					this.countdownTexts[i].gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x0011C810 File Offset: 0x0011AA10
	private void UpdateCountdownText()
	{
		if (ModIOManager.IsDisabled)
		{
			return;
		}
		if (!this.countdownTexts.IsNullOrEmpty<TMP_Text>())
		{
			float f = this.stayInTriggerDuration - (Time.time - this.triggerEntryTime);
			int num = 1 + Mathf.FloorToInt(f);
			for (int i = 0; i < this.countdownTexts.Length; i++)
			{
				if (!this.countdownTexts[i].IsNull())
				{
					this.countdownTexts[i].text = num.ToString();
				}
			}
		}
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x0011C888 File Offset: 0x0011AA88
	public void TeleportPlayer()
	{
		if (ModIOManager.IsDisabled)
		{
			return;
		}
		if (!this.teleporting)
		{
			this.teleporting = true;
			base.StartCoroutine(CustomMapManager.TeleportToVirtualStump(this.teleporterIndex, new Action<bool>(this.FinishTeleport), this.entrancePoint, this.mySerializer));
		}
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x0004CAA5 File Offset: 0x0004ACA5
	private void FinishTeleport(bool success = true)
	{
		if (this.teleporting)
		{
			this.teleporting = false;
			this.triggerEntryTime = -1f;
		}
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x0011C8D8 File Offset: 0x0011AAD8
	private void HideHandHolds()
	{
		foreach (GameObject gameObject in this.handHoldObjects)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x0011C910 File Offset: 0x0011AB10
	private void ShowHandHolds()
	{
		if (ModIOManager.IsDisabled)
		{
			return;
		}
		foreach (GameObject gameObject in this.handHoldObjects)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04002FEB RID: 12267
	[SerializeField]
	private short teleporterIndex;

	// Token: 0x04002FEC RID: 12268
	[SerializeField]
	private float stayInTriggerDuration = 3f;

	// Token: 0x04002FED RID: 12269
	[SerializeField]
	private TMP_Text[] countdownTexts;

	// Token: 0x04002FEE RID: 12270
	[SerializeField]
	private GameObject[] handHoldObjects;

	// Token: 0x04002FEF RID: 12271
	private VirtualStumpTeleporterSerializer mySerializer;

	// Token: 0x04002FF0 RID: 12272
	private bool teleporting;

	// Token: 0x04002FF1 RID: 12273
	private float triggerEntryTime = -1f;

	// Token: 0x04002FF2 RID: 12274
	public GTZone entrancePoint = GTZone.arcade;
}
