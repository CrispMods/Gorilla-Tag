using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000567 RID: 1383
[Obsolete("This class is obsolete and will be removed in a future version. (MattO 2024-02-26) It doesn't appear to be used anywhere.")]
public class GorillaHatButton : MonoBehaviour
{
	// Token: 0x06002220 RID: 8736 RVA: 0x000A8A10 File Offset: 0x000A6C10
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

	// Token: 0x06002221 RID: 8737 RVA: 0x000A8A78 File Offset: 0x000A6C78
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

	// Token: 0x06002222 RID: 8738 RVA: 0x000A8B18 File Offset: 0x000A6D18
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

	// Token: 0x04002597 RID: 9623
	public GorillaHatButtonParent buttonParent;

	// Token: 0x04002598 RID: 9624
	public GorillaHatButton.HatButtonType buttonType;

	// Token: 0x04002599 RID: 9625
	public bool isOn;

	// Token: 0x0400259A RID: 9626
	public Material offMaterial;

	// Token: 0x0400259B RID: 9627
	public Material onMaterial;

	// Token: 0x0400259C RID: 9628
	public string offText;

	// Token: 0x0400259D RID: 9629
	public string onText;

	// Token: 0x0400259E RID: 9630
	public Text myText;

	// Token: 0x0400259F RID: 9631
	public float debounceTime = 0.25f;

	// Token: 0x040025A0 RID: 9632
	public float touchTime;

	// Token: 0x040025A1 RID: 9633
	public string cosmeticName;

	// Token: 0x040025A2 RID: 9634
	public bool testPress;

	// Token: 0x02000568 RID: 1384
	public enum HatButtonType
	{
		// Token: 0x040025A4 RID: 9636
		Hat,
		// Token: 0x040025A5 RID: 9637
		Face,
		// Token: 0x040025A6 RID: 9638
		Badge
	}
}
