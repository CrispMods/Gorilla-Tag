using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000997 RID: 2455
	public class Mole : Tappable
	{
		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06003C0F RID: 15375 RVA: 0x00152C14 File Offset: 0x00150E14
		// (remove) Token: 0x06003C10 RID: 15376 RVA: 0x00152C4C File Offset: 0x00150E4C
		public event Mole.MoleTapEvent OnTapped;

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06003C11 RID: 15377 RVA: 0x00057332 File Offset: 0x00055532
		// (set) Token: 0x06003C12 RID: 15378 RVA: 0x0005733A File Offset: 0x0005553A
		public bool IsLeftSideMole { get; set; }

		// Token: 0x06003C13 RID: 15379 RVA: 0x00152C84 File Offset: 0x00150E84
		private void Awake()
		{
			this.currentState = Mole.MoleState.Hidden;
			Vector3 position = base.transform.position;
			this.origin = (this.target = position);
			this.visiblePosition = new Vector3(position.x, position.y + this.positionOffset, position.z);
			this.hiddenPosition = new Vector3(position.x, position.y - this.positionOffset, position.z);
			this.travelTime = this.normalTravelTime;
			this.animCurve = (this.normalAnimCurve = AnimationCurves.EaseInOutQuad);
			this.hitAnimCurve = AnimationCurves.EaseOutBack;
			for (int i = 0; i < this.moleTypes.Length; i++)
			{
				if (this.moleTypes[i].isHazard)
				{
					this.hazardMoles.Add(i);
				}
				else
				{
					this.safeMoles.Add(i);
				}
			}
			this.randomMolePickedIndex = -1;
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x00152D6C File Offset: 0x00150F6C
		public void InvokeUpdate()
		{
			if (this.currentState == Mole.MoleState.Ready)
			{
				return;
			}
			switch (this.currentState)
			{
			case Mole.MoleState.Reset:
			case Mole.MoleState.Hidden:
				this.currentState = Mole.MoleState.Ready;
				break;
			case Mole.MoleState.TransitionToVisible:
			case Mole.MoleState.TransitionToHidden:
			{
				float num = this.animCurve.Evaluate(Mathf.Clamp01((Time.time - this.animStartTime) / this.travelTime));
				base.transform.position = Vector3.Lerp(this.origin, this.target, num);
				if (num >= 1f)
				{
					this.currentState++;
				}
				break;
			}
			}
			if (Time.time - this.currentTime >= this.showMoleDuration && this.currentState > Mole.MoleState.Ready && this.currentState < Mole.MoleState.TransitionToHidden)
			{
				this.HideMole(false);
			}
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x00057343 File Offset: 0x00055543
		public bool CanPickMole()
		{
			return this.currentState == Mole.MoleState.Ready;
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x00152E38 File Offset: 0x00151038
		public void ShowMole(float _showMoleDuration, int randomMoleTypeIndex)
		{
			if (randomMoleTypeIndex >= this.moleTypes.Length || randomMoleTypeIndex < 0)
			{
				return;
			}
			this.randomMolePickedIndex = randomMoleTypeIndex;
			for (int i = 0; i < this.moleTypes.Length; i++)
			{
				this.moleTypes[i].gameObject.SetActive(i == randomMoleTypeIndex);
				if (this.moleTypes[i].monkeMoleDefaultMaterial != null)
				{
					this.moleTypes[i].MeshRenderer.material = this.moleTypes[i].monkeMoleDefaultMaterial;
				}
			}
			this.showMoleDuration = _showMoleDuration;
			this.origin = base.transform.position;
			this.target = this.visiblePosition;
			this.animCurve = this.normalAnimCurve;
			this.currentState = Mole.MoleState.TransitionToVisible;
			this.animStartTime = (this.currentTime = Time.time);
			this.travelTime = this.normalTravelTime;
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x00152F10 File Offset: 0x00151110
		public void HideMole(bool isHit = false)
		{
			if (this.currentState < Mole.MoleState.TransitionToVisible || this.currentState > Mole.MoleState.Visible)
			{
				return;
			}
			this.origin = base.transform.position;
			this.target = this.hiddenPosition;
			this.animCurve = (isHit ? this.hitAnimCurve : this.normalAnimCurve);
			this.animStartTime = Time.time;
			this.travelTime = (isHit ? this.hitTravelTime : this.normalTravelTime);
			this.currentState = Mole.MoleState.TransitionToHidden;
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x00152F90 File Offset: 0x00151190
		public bool CanTap()
		{
			Mole.MoleState moleState = this.currentState;
			return moleState == Mole.MoleState.TransitionToVisible || moleState == Mole.MoleState.Visible;
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x0005734E File Offset: 0x0005554E
		public override bool CanTap(bool isLeftHand)
		{
			return this.CanTap();
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x00152FB8 File Offset: 0x001511B8
		public override void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped info)
		{
			if (!this.CanTap())
			{
				return;
			}
			bool flag = info.Sender.ActorNumber == NetworkSystem.Instance.LocalPlayerID;
			bool isLeft = flag && GorillaTagger.Instance.lastLeftTap >= GorillaTagger.Instance.lastRightTap;
			MoleTypes moleTypes = null;
			if (this.randomMolePickedIndex >= 0 && this.randomMolePickedIndex < this.moleTypes.Length)
			{
				moleTypes = this.moleTypes[this.randomMolePickedIndex];
			}
			if (moleTypes != null)
			{
				Mole.MoleTapEvent onTapped = this.OnTapped;
				if (onTapped == null)
				{
					return;
				}
				onTapped(moleTypes, base.transform.position, flag, isLeft);
			}
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x00057356 File Offset: 0x00055556
		public void ResetPosition()
		{
			base.transform.position = this.hiddenPosition;
			this.currentState = Mole.MoleState.Reset;
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x00057370 File Offset: 0x00055570
		public int GetMoleTypeIndex(bool useHazardMole)
		{
			if (!useHazardMole)
			{
				return this.safeMoles[UnityEngine.Random.Range(0, this.safeMoles.Count)];
			}
			return this.hazardMoles[UnityEngine.Random.Range(0, this.hazardMoles.Count)];
		}

		// Token: 0x04003CD6 RID: 15574
		public float positionOffset = 0.2f;

		// Token: 0x04003CD7 RID: 15575
		public MoleTypes[] moleTypes;

		// Token: 0x04003CD8 RID: 15576
		private float showMoleDuration;

		// Token: 0x04003CD9 RID: 15577
		private Vector3 visiblePosition;

		// Token: 0x04003CDA RID: 15578
		private Vector3 hiddenPosition;

		// Token: 0x04003CDB RID: 15579
		private float currentTime;

		// Token: 0x04003CDC RID: 15580
		private float animStartTime;

		// Token: 0x04003CDD RID: 15581
		private float travelTime;

		// Token: 0x04003CDE RID: 15582
		private float normalTravelTime = 0.3f;

		// Token: 0x04003CDF RID: 15583
		private float hitTravelTime = 0.2f;

		// Token: 0x04003CE0 RID: 15584
		private AnimationCurve animCurve;

		// Token: 0x04003CE1 RID: 15585
		private AnimationCurve normalAnimCurve;

		// Token: 0x04003CE2 RID: 15586
		private AnimationCurve hitAnimCurve;

		// Token: 0x04003CE3 RID: 15587
		private Mole.MoleState currentState;

		// Token: 0x04003CE4 RID: 15588
		private Vector3 origin;

		// Token: 0x04003CE5 RID: 15589
		private Vector3 target;

		// Token: 0x04003CE6 RID: 15590
		private int randomMolePickedIndex;

		// Token: 0x04003CE8 RID: 15592
		public CallLimiter rpcCooldown;

		// Token: 0x04003CE9 RID: 15593
		private int moleScore;

		// Token: 0x04003CEA RID: 15594
		private List<int> safeMoles = new List<int>();

		// Token: 0x04003CEB RID: 15595
		private List<int> hazardMoles = new List<int>();

		// Token: 0x02000998 RID: 2456
		// (Invoke) Token: 0x06003C1F RID: 15391
		public delegate void MoleTapEvent(MoleTypes moleType, Vector3 position, bool isLocalTap, bool isLeft);

		// Token: 0x02000999 RID: 2457
		public enum MoleState
		{
			// Token: 0x04003CEE RID: 15598
			Reset,
			// Token: 0x04003CEF RID: 15599
			Ready,
			// Token: 0x04003CF0 RID: 15600
			TransitionToVisible,
			// Token: 0x04003CF1 RID: 15601
			Visible,
			// Token: 0x04003CF2 RID: 15602
			TransitionToHidden,
			// Token: 0x04003CF3 RID: 15603
			Hidden
		}
	}
}
