using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036C RID: 876
public class GorillaReportButton : MonoBehaviour
{
	// Token: 0x06001455 RID: 5205 RVA: 0x000638FE File Offset: 0x00061AFE
	public void AssignParentLine(GorillaPlayerScoreboardLine parent)
	{
		this.parentLine = parent;
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x00063908 File Offset: 0x00061B08
	private void OnTriggerEnter(Collider collider)
	{
		if (base.enabled && this.touchTime + this.debounceTime < Time.time)
		{
			this.isOn = !this.isOn;
			this.UpdateColor();
			this.selected = !this.selected;
			this.touchTime = Time.time;
			GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
			GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, false, 0.05f);
			if (NetworkSystem.Instance.InRoom && GorillaTagger.Instance.myVRRig != null)
			{
				GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
				{
					67,
					false,
					0.05f
				});
			}
		}
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x000639FB File Offset: 0x00061BFB
	private void OnTriggerExit(Collider other)
	{
		if (this.metaReportType != GorillaReportButton.MetaReportReason.Cancel)
		{
			other.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null;
		}
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x00063A13 File Offset: 0x00061C13
	public void UpdateColor()
	{
		if (this.isOn)
		{
			base.GetComponent<MeshRenderer>().material = this.onMaterial;
			return;
		}
		base.GetComponent<MeshRenderer>().material = this.offMaterial;
	}

	// Token: 0x04001675 RID: 5749
	public GorillaReportButton.MetaReportReason metaReportType;

	// Token: 0x04001676 RID: 5750
	public GorillaPlayerLineButton.ButtonType buttonType;

	// Token: 0x04001677 RID: 5751
	public GorillaPlayerScoreboardLine parentLine;

	// Token: 0x04001678 RID: 5752
	public bool isOn;

	// Token: 0x04001679 RID: 5753
	public Material offMaterial;

	// Token: 0x0400167A RID: 5754
	public Material onMaterial;

	// Token: 0x0400167B RID: 5755
	public string offText;

	// Token: 0x0400167C RID: 5756
	public string onText;

	// Token: 0x0400167D RID: 5757
	public Text myText;

	// Token: 0x0400167E RID: 5758
	public float debounceTime = 0.25f;

	// Token: 0x0400167F RID: 5759
	public float touchTime;

	// Token: 0x04001680 RID: 5760
	public bool testPress;

	// Token: 0x04001681 RID: 5761
	public bool selected;

	// Token: 0x0200036D RID: 877
	[SerializeField]
	public enum MetaReportReason
	{
		// Token: 0x04001683 RID: 5763
		HateSpeech,
		// Token: 0x04001684 RID: 5764
		Cheating,
		// Token: 0x04001685 RID: 5765
		Toxicity,
		// Token: 0x04001686 RID: 5766
		Bullying,
		// Token: 0x04001687 RID: 5767
		Doxing,
		// Token: 0x04001688 RID: 5768
		Impersonation,
		// Token: 0x04001689 RID: 5769
		Submit,
		// Token: 0x0400168A RID: 5770
		Cancel
	}
}
