using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000568 RID: 1384
[Obsolete("This class is obsolete and will be removed in a future version. (MattO 2024-02-26) It doesn't appear to be used anywhere.")]
public class GorillaHatButton : MonoBehaviour
{
	// Token: 0x06002228 RID: 8744 RVA: 0x000F5BDC File Offset: 0x000F3DDC
	public void Update()
	{
		if (this.testPress)
		{
			this.testPress = false;
			if (this.touchTime + this.debounceTime < Time.time)
			{
				this.touchTime = Time.time;
				this.isOn = !this.isOn;
				this.buttonParent.PressButton(this.isOn, this.buttonType, this.cosmeticName);
			}
		}
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x000F5C44 File Offset: 0x000F3E44
	private void OnTriggerEnter(Collider collider)
	{
		if (this.touchTime + this.debounceTime < Time.time && collider.GetComponentInParent<GorillaTriggerColliderHandIndicator>() != null)
		{
			this.touchTime = Time.time;
			GorillaTriggerColliderHandIndicator component = collider.GetComponent<GorillaTriggerColliderHandIndicator>();
			this.isOn = !this.isOn;
			this.buttonParent.PressButton(this.isOn, this.buttonType, this.cosmeticName);
			if (component != null)
			{
				GorillaTagger.Instance.StartVibration(component.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
			}
		}
	}

	// Token: 0x0600222A RID: 8746 RVA: 0x000F5CE4 File Offset: 0x000F3EE4
	public void UpdateColor()
	{
		if (this.isOn)
		{
			base.GetComponent<MeshRenderer>().material = this.onMaterial;
			this.myText.text = this.onText;
			return;
		}
		base.GetComponent<MeshRenderer>().material = this.offMaterial;
		this.myText.text = this.offText;
	}

	// Token: 0x0400259D RID: 9629
	public GorillaHatButtonParent buttonParent;

	// Token: 0x0400259E RID: 9630
	public GorillaHatButton.HatButtonType buttonType;

	// Token: 0x0400259F RID: 9631
	public bool isOn;

	// Token: 0x040025A0 RID: 9632
	public Material offMaterial;

	// Token: 0x040025A1 RID: 9633
	public Material onMaterial;

	// Token: 0x040025A2 RID: 9634
	public string offText;

	// Token: 0x040025A3 RID: 9635
	public string onText;

	// Token: 0x040025A4 RID: 9636
	public Text myText;

	// Token: 0x040025A5 RID: 9637
	public float debounceTime = 0.25f;

	// Token: 0x040025A6 RID: 9638
	public float touchTime;

	// Token: 0x040025A7 RID: 9639
	public string cosmeticName;

	// Token: 0x040025A8 RID: 9640
	public bool testPress;

	// Token: 0x02000569 RID: 1385
	public enum HatButtonType
	{
		// Token: 0x040025AA RID: 9642
		Hat,
		// Token: 0x040025AB RID: 9643
		Face,
		// Token: 0x040025AC RID: 9644
		Badge
	}
}
