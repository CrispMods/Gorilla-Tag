using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000377 RID: 887
public class GorillaReportButton : MonoBehaviour
{
	// Token: 0x060014A1 RID: 5281 RVA: 0x0003DE62 File Offset: 0x0003C062
	public void AssignParentLine(GorillaPlayerScoreboardLine parent)
	{
		this.parentLine = parent;
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x000BCB98 File Offset: 0x000BAD98
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

	// Token: 0x060014A3 RID: 5283 RVA: 0x0003DE6B File Offset: 0x0003C06B
	private void OnTriggerExit(Collider other)
	{
		if (this.metaReportType != GorillaReportButton.MetaReportReason.Cancel)
		{
			other.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null;
		}
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x0003DE83 File Offset: 0x0003C083
	public void UpdateColor()
	{
		if (this.isOn)
		{
			base.GetComponent<MeshRenderer>().material = this.onMaterial;
			return;
		}
		base.GetComponent<MeshRenderer>().material = this.offMaterial;
	}

	// Token: 0x040016BD RID: 5821
	public GorillaReportButton.MetaReportReason metaReportType;

	// Token: 0x040016BE RID: 5822
	public GorillaPlayerLineButton.ButtonType buttonType;

	// Token: 0x040016BF RID: 5823
	public GorillaPlayerScoreboardLine parentLine;

	// Token: 0x040016C0 RID: 5824
	public bool isOn;

	// Token: 0x040016C1 RID: 5825
	public Material offMaterial;

	// Token: 0x040016C2 RID: 5826
	public Material onMaterial;

	// Token: 0x040016C3 RID: 5827
	public string offText;

	// Token: 0x040016C4 RID: 5828
	public string onText;

	// Token: 0x040016C5 RID: 5829
	public Text myText;

	// Token: 0x040016C6 RID: 5830
	public float debounceTime = 0.25f;

	// Token: 0x040016C7 RID: 5831
	public float touchTime;

	// Token: 0x040016C8 RID: 5832
	public bool testPress;

	// Token: 0x040016C9 RID: 5833
	public bool selected;

	// Token: 0x02000378 RID: 888
	[SerializeField]
	public enum MetaReportReason
	{
		// Token: 0x040016CB RID: 5835
		HateSpeech,
		// Token: 0x040016CC RID: 5836
		Cheating,
		// Token: 0x040016CD RID: 5837
		Toxicity,
		// Token: 0x040016CE RID: 5838
		Bullying,
		// Token: 0x040016CF RID: 5839
		Doxing,
		// Token: 0x040016D0 RID: 5840
		Impersonation,
		// Token: 0x040016D1 RID: 5841
		Submit,
		// Token: 0x040016D2 RID: 5842
		Cancel
	}
}
