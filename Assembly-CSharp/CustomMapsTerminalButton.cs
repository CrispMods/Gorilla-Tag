using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

// Token: 0x020006A0 RID: 1696
public class CustomMapsTerminalButton : GorillaPressableButton
{
	// Token: 0x06002A36 RID: 10806 RVA: 0x0011BD24 File Offset: 0x00119F24
	public static string BindingToString(CustomMapsTerminalButton.ModIOKeyboardBindings binding)
	{
		if (binding < CustomMapsTerminalButton.ModIOKeyboardBindings.up || (binding > CustomMapsTerminalButton.ModIOKeyboardBindings.option3 && binding < CustomMapsTerminalButton.ModIOKeyboardBindings.at))
		{
			if (binding >= CustomMapsTerminalButton.ModIOKeyboardBindings.up)
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
			case CustomMapsTerminalButton.ModIOKeyboardBindings.at:
				return "@";
			case CustomMapsTerminalButton.ModIOKeyboardBindings.dash:
				return "-";
			case CustomMapsTerminalButton.ModIOKeyboardBindings.period:
				return ".";
			case CustomMapsTerminalButton.ModIOKeyboardBindings.underscore:
				return "_";
			case CustomMapsTerminalButton.ModIOKeyboardBindings.plus:
				return "+";
			case CustomMapsTerminalButton.ModIOKeyboardBindings.space:
				return " ";
			default:
				return "";
			}
		}
	}

	// Token: 0x06002A37 RID: 10807 RVA: 0x0004C7B8 File Offset: 0x0004A9B8
	public override void Start()
	{
		base.Start();
		this.ResetButtonColor();
	}

	// Token: 0x06002A38 RID: 10808 RVA: 0x0011BDAC File Offset: 0x00119FAC
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

	// Token: 0x06002A39 RID: 10809 RVA: 0x0004C7C6 File Offset: 0x0004A9C6
	private void ResetButtonColor()
	{
		if (this.buttonRenderer != null)
		{
			this.buttonRenderer.material = this.unpressedMaterial;
		}
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x0004C7E7 File Offset: 0x0004A9E7
	private IEnumerator PressButtonColorUpdate()
	{
		this.isOn = true;
		this.UpdateColor();
		yield return new WaitForSeconds(this.debounceTime);
		this.isOn = false;
		this.UpdateColor();
		yield break;
	}

	// Token: 0x04002FA6 RID: 12198
	public CustomMapsTerminalButton.ModIOKeyboardBindings modIOBinding;

	// Token: 0x04002FA7 RID: 12199
	private float pressedTime;

	// Token: 0x020006A1 RID: 1697
	public enum ModIOKeyboardBindings
	{
		// Token: 0x04002FA9 RID: 12201
		zero,
		// Token: 0x04002FAA RID: 12202
		one,
		// Token: 0x04002FAB RID: 12203
		two,
		// Token: 0x04002FAC RID: 12204
		three,
		// Token: 0x04002FAD RID: 12205
		four,
		// Token: 0x04002FAE RID: 12206
		five,
		// Token: 0x04002FAF RID: 12207
		six,
		// Token: 0x04002FB0 RID: 12208
		seven,
		// Token: 0x04002FB1 RID: 12209
		eight,
		// Token: 0x04002FB2 RID: 12210
		nine,
		// Token: 0x04002FB3 RID: 12211
		up,
		// Token: 0x04002FB4 RID: 12212
		down,
		// Token: 0x04002FB5 RID: 12213
		delete,
		// Token: 0x04002FB6 RID: 12214
		enter,
		// Token: 0x04002FB7 RID: 12215
		option1,
		// Token: 0x04002FB8 RID: 12216
		option2,
		// Token: 0x04002FB9 RID: 12217
		option3,
		// Token: 0x04002FBA RID: 12218
		A,
		// Token: 0x04002FBB RID: 12219
		B,
		// Token: 0x04002FBC RID: 12220
		C,
		// Token: 0x04002FBD RID: 12221
		D,
		// Token: 0x04002FBE RID: 12222
		E,
		// Token: 0x04002FBF RID: 12223
		F,
		// Token: 0x04002FC0 RID: 12224
		G,
		// Token: 0x04002FC1 RID: 12225
		H,
		// Token: 0x04002FC2 RID: 12226
		I,
		// Token: 0x04002FC3 RID: 12227
		J,
		// Token: 0x04002FC4 RID: 12228
		K,
		// Token: 0x04002FC5 RID: 12229
		L,
		// Token: 0x04002FC6 RID: 12230
		M,
		// Token: 0x04002FC7 RID: 12231
		N,
		// Token: 0x04002FC8 RID: 12232
		O,
		// Token: 0x04002FC9 RID: 12233
		P,
		// Token: 0x04002FCA RID: 12234
		Q,
		// Token: 0x04002FCB RID: 12235
		R,
		// Token: 0x04002FCC RID: 12236
		S,
		// Token: 0x04002FCD RID: 12237
		T,
		// Token: 0x04002FCE RID: 12238
		U,
		// Token: 0x04002FCF RID: 12239
		V,
		// Token: 0x04002FD0 RID: 12240
		W,
		// Token: 0x04002FD1 RID: 12241
		X,
		// Token: 0x04002FD2 RID: 12242
		Y,
		// Token: 0x04002FD3 RID: 12243
		Z,
		// Token: 0x04002FD4 RID: 12244
		at,
		// Token: 0x04002FD5 RID: 12245
		dash,
		// Token: 0x04002FD6 RID: 12246
		period,
		// Token: 0x04002FD7 RID: 12247
		underscore,
		// Token: 0x04002FD8 RID: 12248
		plus,
		// Token: 0x04002FD9 RID: 12249
		space,
		// Token: 0x04002FDA RID: 12250
		goback,
		// Token: 0x04002FDB RID: 12251
		left,
		// Token: 0x04002FDC RID: 12252
		right,
		// Token: 0x04002FDD RID: 12253
		option4,
		// Token: 0x04002FDE RID: 12254
		sort,
		// Token: 0x04002FDF RID: 12255
		sub,
		// Token: 0x04002FE0 RID: 12256
		map
	}
}
