using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000678 RID: 1656
public class ModIOKeyboardButton : GorillaPressableButton
{
	// Token: 0x0600290E RID: 10510 RVA: 0x000CA10C File Offset: 0x000C830C
	public static string BindingToString(ModIOKeyboardButton.ModIOKeyboardBindings binding)
	{
		if (binding < ModIOKeyboardButton.ModIOKeyboardBindings.up || (binding > ModIOKeyboardButton.ModIOKeyboardBindings.option3 && binding < ModIOKeyboardButton.ModIOKeyboardBindings.at))
		{
			if (binding >= ModIOKeyboardButton.ModIOKeyboardBindings.up)
			{
				return binding.ToString();
			}
			int num = (int)binding;
			return num.ToString();
		}
		else
		{
			switch (binding)
			{
			case ModIOKeyboardButton.ModIOKeyboardBindings.at:
				return "@";
			case ModIOKeyboardButton.ModIOKeyboardBindings.dash:
				return "-";
			case ModIOKeyboardButton.ModIOKeyboardBindings.period:
				return ".";
			case ModIOKeyboardButton.ModIOKeyboardBindings.underscore:
				return "_";
			case ModIOKeyboardButton.ModIOKeyboardBindings.plus:
				return "+";
			case ModIOKeyboardButton.ModIOKeyboardBindings.space:
				return " ";
			default:
				return "";
			}
		}
	}

	// Token: 0x0600290F RID: 10511 RVA: 0x000CA191 File Offset: 0x000C8391
	public override void Start()
	{
		base.Start();
		this.ResetButtonColor();
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x000CA1A0 File Offset: 0x000C83A0
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		GameEvents.OnModIOKeyboardButtonPressedEvent.Invoke(this.modIOBinding);
		base.StartCoroutine(this.PressButtonColorUpdate());
		GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);
		GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, isLeftHand, 0.1f);
		if (NetworkSystem.Instance.InRoom && GorillaTagger.Instance.myVRRig != null)
		{
			GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
			{
				66,
				isLeftHand,
				0.1f
			});
		}
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x000CA25F File Offset: 0x000C845F
	private void ResetButtonColor()
	{
		if (this.buttonRenderer != null)
		{
			this.buttonRenderer.material = this.unpressedMaterial;
		}
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x000CA280 File Offset: 0x000C8480
	private IEnumerator PressButtonColorUpdate()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.debounceTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002E15 RID: 11797
	public ModIOKeyboardButton.ModIOKeyboardBindings modIOBinding;

	// Token: 0x04002E16 RID: 11798
	private float pressedTime;

	// Token: 0x02000679 RID: 1657
	public enum ModIOKeyboardBindings
	{
		// Token: 0x04002E18 RID: 11800
		zero,
		// Token: 0x04002E19 RID: 11801
		one,
		// Token: 0x04002E1A RID: 11802
		two,
		// Token: 0x04002E1B RID: 11803
		three,
		// Token: 0x04002E1C RID: 11804
		four,
		// Token: 0x04002E1D RID: 11805
		five,
		// Token: 0x04002E1E RID: 11806
		six,
		// Token: 0x04002E1F RID: 11807
		seven,
		// Token: 0x04002E20 RID: 11808
		eight,
		// Token: 0x04002E21 RID: 11809
		nine,
		// Token: 0x04002E22 RID: 11810
		up,
		// Token: 0x04002E23 RID: 11811
		down,
		// Token: 0x04002E24 RID: 11812
		delete,
		// Token: 0x04002E25 RID: 11813
		enter,
		// Token: 0x04002E26 RID: 11814
		option1,
		// Token: 0x04002E27 RID: 11815
		option2,
		// Token: 0x04002E28 RID: 11816
		option3,
		// Token: 0x04002E29 RID: 11817
		A,
		// Token: 0x04002E2A RID: 11818
		B,
		// Token: 0x04002E2B RID: 11819
		C,
		// Token: 0x04002E2C RID: 11820
		D,
		// Token: 0x04002E2D RID: 11821
		E,
		// Token: 0x04002E2E RID: 11822
		F,
		// Token: 0x04002E2F RID: 11823
		G,
		// Token: 0x04002E30 RID: 11824
		H,
		// Token: 0x04002E31 RID: 11825
		I,
		// Token: 0x04002E32 RID: 11826
		J,
		// Token: 0x04002E33 RID: 11827
		K,
		// Token: 0x04002E34 RID: 11828
		L,
		// Token: 0x04002E35 RID: 11829
		M,
		// Token: 0x04002E36 RID: 11830
		N,
		// Token: 0x04002E37 RID: 11831
		O,
		// Token: 0x04002E38 RID: 11832
		P,
		// Token: 0x04002E39 RID: 11833
		Q,
		// Token: 0x04002E3A RID: 11834
		R,
		// Token: 0x04002E3B RID: 11835
		S,
		// Token: 0x04002E3C RID: 11836
		T,
		// Token: 0x04002E3D RID: 11837
		U,
		// Token: 0x04002E3E RID: 11838
		V,
		// Token: 0x04002E3F RID: 11839
		W,
		// Token: 0x04002E40 RID: 11840
		X,
		// Token: 0x04002E41 RID: 11841
		Y,
		// Token: 0x04002E42 RID: 11842
		Z,
		// Token: 0x04002E43 RID: 11843
		at,
		// Token: 0x04002E44 RID: 11844
		dash,
		// Token: 0x04002E45 RID: 11845
		period,
		// Token: 0x04002E46 RID: 11846
		underscore,
		// Token: 0x04002E47 RID: 11847
		plus,
		// Token: 0x04002E48 RID: 11848
		space,
		// Token: 0x04002E49 RID: 11849
		goback,
		// Token: 0x04002E4A RID: 11850
		left,
		// Token: 0x04002E4B RID: 11851
		right,
		// Token: 0x04002E4C RID: 11852
		option4
	}
}
