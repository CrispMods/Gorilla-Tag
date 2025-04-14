using System;

namespace GorillaTagScripts
{
	// Token: 0x0200098C RID: 2444
	public class BuilderOptionButton : GorillaPressableButton
	{
		// Token: 0x06003BDC RID: 15324 RVA: 0x00113D17 File Offset: 0x00111F17
		public override void Start()
		{
			base.Start();
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x000023F4 File Offset: 0x000005F4
		private void OnDestroy()
		{
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x00113D1F File Offset: 0x00111F1F
		public void Setup(Action<BuilderOptionButton, bool> onPressed)
		{
			this.onPressed = onPressed;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00113D28 File Offset: 0x00111F28
		public override void ButtonActivationWithHand(bool isLeftHand)
		{
			Action<BuilderOptionButton, bool> action = this.onPressed;
			if (action == null)
			{
				return;
			}
			action(this, isLeftHand);
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x00113D3C File Offset: 0x00111F3C
		public void SetPressed(bool pressed)
		{
			this.buttonRenderer.material = (pressed ? this.pressedMaterial : this.unpressedMaterial);
		}

		// Token: 0x04003D0B RID: 15627
		private new Action<BuilderOptionButton, bool> onPressed;
	}
}
