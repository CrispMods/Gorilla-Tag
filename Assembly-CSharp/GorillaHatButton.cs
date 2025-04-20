using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000575 RID: 1397
[Obsolete("This class is obsolete and will be removed in a future version. (MattO 2024-02-26) It doesn't appear to be used anywhere.")]
public class GorillaHatButton : MonoBehaviour
{
	// Token: 0x0600227E RID: 8830 RVA: 0x000F8958 File Offset: 0x000F6B58
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

	// Token: 0x0600227F RID: 8831 RVA: 0x000F89C0 File Offset: 0x000F6BC0
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

	// Token: 0x06002280 RID: 8832 RVA: 0x000F8A60 File Offset: 0x000F6C60
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

	// Token: 0x040025EF RID: 9711
	public GorillaHatButtonParent buttonParent;

	// Token: 0x040025F0 RID: 9712
	public GorillaHatButton.HatButtonType buttonType;

	// Token: 0x040025F1 RID: 9713
	public bool isOn;

	// Token: 0x040025F2 RID: 9714
	public Material offMaterial;

	// Token: 0x040025F3 RID: 9715
	public Material onMaterial;

	// Token: 0x040025F4 RID: 9716
	public string offText;

	// Token: 0x040025F5 RID: 9717
	public string onText;

	// Token: 0x040025F6 RID: 9718
	public Text myText;

	// Token: 0x040025F7 RID: 9719
	public float debounceTime = 0.25f;

	// Token: 0x040025F8 RID: 9720
	public float touchTime;

	// Token: 0x040025F9 RID: 9721
	public string cosmeticName;

	// Token: 0x040025FA RID: 9722
	public bool testPress;

	// Token: 0x02000576 RID: 1398
	public enum HatButtonType
	{
		// Token: 0x040025FC RID: 9724
		Hat,
		// Token: 0x040025FD RID: 9725
		Face,
		// Token: 0x040025FE RID: 9726
		Badge
	}
}
