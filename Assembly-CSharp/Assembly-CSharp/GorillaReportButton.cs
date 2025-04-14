using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200036C RID: 876
public class GorillaReportButton : MonoBehaviour
{
	// Token: 0x06001458 RID: 5208 RVA: 0x00063C82 File Offset: 0x00061E82
	public void AssignParentLine(GorillaPlayerScoreboardLine parent)
	{
		this.parentLine = parent;
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x00063C8C File Offset: 0x00061E8C
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

	// Token: 0x0600145A RID: 5210 RVA: 0x00063D7F File Offset: 0x00061F7F
	private void OnTriggerExit(Collider other)
	{
		if (this.metaReportType != GorillaReportButton.MetaReportReason.Cancel)
		{
			other.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null;
		}
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x00063D97 File Offset: 0x00061F97
	public void UpdateColor()
	{
		if (this.isOn)
		{
			base.GetComponent<MeshRenderer>().material = this.onMaterial;
			return;
		}
		base.GetComponent<MeshRenderer>().material = this.offMaterial;
	}

	// Token: 0x04001676 RID: 5750
	public GorillaReportButton.MetaReportReason metaReportType;

	// Token: 0x04001677 RID: 5751
	public GorillaPlayerLineButton.ButtonType buttonType;

	// Token: 0x04001678 RID: 5752
	public GorillaPlayerScoreboardLine parentLine;

	// Token: 0x04001679 RID: 5753
	public bool isOn;

	// Token: 0x0400167A RID: 5754
	public Material offMaterial;

	// Token: 0x0400167B RID: 5755
	public Material onMaterial;

	// Token: 0x0400167C RID: 5756
	public string offText;

	// Token: 0x0400167D RID: 5757
	public string onText;

	// Token: 0x0400167E RID: 5758
	public Text myText;

	// Token: 0x0400167F RID: 5759
	public float debounceTime = 0.25f;

	// Token: 0x04001680 RID: 5760
	public float touchTime;

	// Token: 0x04001681 RID: 5761
	public bool testPress;

	// Token: 0x04001682 RID: 5762
	public bool selected;

	// Token: 0x0200036D RID: 877
	[SerializeField]
	public enum MetaReportReason
	{
		// Token: 0x04001684 RID: 5764
		HateSpeech,
		// Token: 0x04001685 RID: 5765
		Cheating,
		// Token: 0x04001686 RID: 5766
		Toxicity,
		// Token: 0x04001687 RID: 5767
		Bullying,
		// Token: 0x04001688 RID: 5768
		Doxing,
		// Token: 0x04001689 RID: 5769
		Impersonation,
		// Token: 0x0400168A RID: 5770
		Submit,
		// Token: 0x0400168B RID: 5771
		Cancel
	}
}
