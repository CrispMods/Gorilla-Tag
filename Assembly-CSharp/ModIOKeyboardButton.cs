using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000677 RID: 1655
public class ModIOKeyboardButton : GorillaPressableButton
{
	// Token: 0x06002906 RID: 10502 RVA: 0x000C9C8C File Offset: 0x000C7E8C
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

	// Token: 0x06002907 RID: 10503 RVA: 0x000C9D11 File Offset: 0x000C7F11
	public override void Start()
	{
		base.Start();
		this.ResetButtonColor();
	}

	// Token: 0x06002908 RID: 10504 RVA: 0x000C9D20 File Offset: 0x000C7F20
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

	// Token: 0x06002909 RID: 10505 RVA: 0x000C9DDF File Offset: 0x000C7FDF
	private void ResetButtonColor()
	{
		if (this.buttonRenderer != null)
		{
			this.buttonRenderer.material = this.unpressedMaterial;
		}
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000C9E00 File Offset: 0x000C8000
	private IEnumerator PressButtonColorUpdate()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.debounceTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002E0F RID: 11791
	public ModIOKeyboardButton.ModIOKeyboardBindings modIOBinding;

	// Token: 0x04002E10 RID: 11792
	private float pressedTime;

	// Token: 0x02000678 RID: 1656
	public enum ModIOKeyboardBindings
	{
		// Token: 0x04002E12 RID: 11794
		zero,
		// Token: 0x04002E13 RID: 11795
		one,
		// Token: 0x04002E14 RID: 11796
		two,
		// Token: 0x04002E15 RID: 11797
		three,
		// Token: 0x04002E16 RID: 11798
		four,
		// Token: 0x04002E17 RID: 11799
		five,
		// Token: 0x04002E18 RID: 11800
		six,
		// Token: 0x04002E19 RID: 11801
		seven,
		// Token: 0x04002E1A RID: 11802
		eight,
		// Token: 0x04002E1B RID: 11803
		nine,
		// Token: 0x04002E1C RID: 11804
		up,
		// Token: 0x04002E1D RID: 11805
		down,
		// Token: 0x04002E1E RID: 11806
		delete,
		// Token: 0x04002E1F RID: 11807
		enter,
		// Token: 0x04002E20 RID: 11808
		option1,
		// Token: 0x04002E21 RID: 11809
		option2,
		// Token: 0x04002E22 RID: 11810
		option3,
		// Token: 0x04002E23 RID: 11811
		A,
		// Token: 0x04002E24 RID: 11812
		B,
		// Token: 0x04002E25 RID: 11813
		C,
		// Token: 0x04002E26 RID: 11814
		D,
		// Token: 0x04002E27 RID: 11815
		E,
		// Token: 0x04002E28 RID: 11816
		F,
		// Token: 0x04002E29 RID: 11817
		G,
		// Token: 0x04002E2A RID: 11818
		H,
		// Token: 0x04002E2B RID: 11819
		I,
		// Token: 0x04002E2C RID: 11820
		J,
		// Token: 0x04002E2D RID: 11821
		K,
		// Token: 0x04002E2E RID: 11822
		L,
		// Token: 0x04002E2F RID: 11823
		M,
		// Token: 0x04002E30 RID: 11824
		N,
		// Token: 0x04002E31 RID: 11825
		O,
		// Token: 0x04002E32 RID: 11826
		P,
		// Token: 0x04002E33 RID: 11827
		Q,
		// Token: 0x04002E34 RID: 11828
		R,
		// Token: 0x04002E35 RID: 11829
		S,
		// Token: 0x04002E36 RID: 11830
		T,
		// Token: 0x04002E37 RID: 11831
		U,
		// Token: 0x04002E38 RID: 11832
		V,
		// Token: 0x04002E39 RID: 11833
		W,
		// Token: 0x04002E3A RID: 11834
		X,
		// Token: 0x04002E3B RID: 11835
		Y,
		// Token: 0x04002E3C RID: 11836
		Z,
		// Token: 0x04002E3D RID: 11837
		at,
		// Token: 0x04002E3E RID: 11838
		dash,
		// Token: 0x04002E3F RID: 11839
		period,
		// Token: 0x04002E40 RID: 11840
		underscore,
		// Token: 0x04002E41 RID: 11841
		plus,
		// Token: 0x04002E42 RID: 11842
		space,
		// Token: 0x04002E43 RID: 11843
		goback,
		// Token: 0x04002E44 RID: 11844
		left,
		// Token: 0x04002E45 RID: 11845
		right,
		// Token: 0x04002E46 RID: 11846
		option4
	}
}
