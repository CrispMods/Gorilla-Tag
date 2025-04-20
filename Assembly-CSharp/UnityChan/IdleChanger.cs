using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityChan
{
	// Token: 0x02000CD9 RID: 3289
	public class IdleChanger : MonoBehaviour
	{
		// Token: 0x0600532A RID: 21290 RVA: 0x00065E94 File Offset: 0x00064094
		private void Start()
		{
			this.currentState = this.UnityChanA.GetCurrentAnimatorStateInfo(0);
			this.previousState = this.currentState;
			base.StartCoroutine("RandomChange");
			this.kb = Keyboard.current;
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x001C7260 File Offset: 0x001C5460
		private void Update()
		{
			if (this.kb.upArrowKey.wasPressedThisFrame || this.kb.spaceKey.wasPressedThisFrame)
			{
				this.UnityChanA.SetBool("Next", true);
				this.UnityChanB.SetBool("Next", true);
			}
			if (this.kb.downArrowKey.wasPressedThisFrame)
			{
				this.UnityChanA.SetBool("Back", true);
				this.UnityChanB.SetBool("Back", true);
			}
			if (this.UnityChanA.GetBool("Next"))
			{
				this.currentState = this.UnityChanA.GetCurrentAnimatorStateInfo(0);
				if (this.previousState.fullPathHash != this.currentState.fullPathHash)
				{
					this.UnityChanA.SetBool("Next", false);
					this.UnityChanB.SetBool("Next", false);
					this.previousState = this.currentState;
				}
			}
			if (this.UnityChanA.GetBool("Back"))
			{
				this.currentState = this.UnityChanA.GetCurrentAnimatorStateInfo(0);
				if (this.previousState.fullPathHash != this.currentState.fullPathHash)
				{
					this.UnityChanA.SetBool("Back", false);
					this.UnityChanB.SetBool("Back", false);
					this.previousState = this.currentState;
				}
			}
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x001C73BC File Offset: 0x001C55BC
		private void OnGUI()
		{
			if (this.isGUI)
			{
				GUI.Box(new Rect((float)(Screen.width - 110), 10f, 100f, 90f), "Change Motion");
				if (GUI.Button(new Rect((float)(Screen.width - 100), 40f, 80f, 20f), "Next"))
				{
					this.UnityChanA.SetBool("Next", true);
					this.UnityChanB.SetBool("Next", true);
				}
				if (GUI.Button(new Rect((float)(Screen.width - 100), 70f, 80f, 20f), "Back"))
				{
					this.UnityChanA.SetBool("Back", true);
					this.UnityChanB.SetBool("Back", true);
				}
			}
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x00065ECB File Offset: 0x000640CB
		private IEnumerator RandomChange()
		{
			for (;;)
			{
				if (this._random)
				{
					float num = UnityEngine.Random.Range(0f, 1f);
					if (num < this._threshold)
					{
						this.UnityChanA.SetBool("Back", true);
						this.UnityChanB.SetBool("Back", true);
					}
					else if (num >= this._threshold)
					{
						this.UnityChanA.SetBool("Next", true);
						this.UnityChanB.SetBool("Next", true);
					}
				}
				yield return new WaitForSeconds(this._interval);
			}
			yield break;
		}

		// Token: 0x040054F2 RID: 21746
		private AnimatorStateInfo currentState;

		// Token: 0x040054F3 RID: 21747
		private AnimatorStateInfo previousState;

		// Token: 0x040054F4 RID: 21748
		public bool _random;

		// Token: 0x040054F5 RID: 21749
		public float _threshold = 0.5f;

		// Token: 0x040054F6 RID: 21750
		public float _interval = 10f;

		// Token: 0x040054F7 RID: 21751
		public bool isGUI = true;

		// Token: 0x040054F8 RID: 21752
		public Animator UnityChanA;

		// Token: 0x040054F9 RID: 21753
		public Animator UnityChanB;

		// Token: 0x040054FA RID: 21754
		private Keyboard kb;
	}
}
