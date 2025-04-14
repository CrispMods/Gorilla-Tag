using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000974 RID: 2420
	public class Mole : Tappable
	{
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06003B03 RID: 15107 RVA: 0x0010FA68 File Offset: 0x0010DC68
		// (remove) Token: 0x06003B04 RID: 15108 RVA: 0x0010FAA0 File Offset: 0x0010DCA0
		public event Mole.MoleTapEvent OnTapped;

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06003B05 RID: 15109 RVA: 0x0010FAD5 File Offset: 0x0010DCD5
		// (set) Token: 0x06003B06 RID: 15110 RVA: 0x0010FADD File Offset: 0x0010DCDD
		public bool IsLeftSideMole { get; set; }

		// Token: 0x06003B07 RID: 15111 RVA: 0x0010FAE8 File Offset: 0x0010DCE8
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

		// Token: 0x06003B08 RID: 15112 RVA: 0x0010FBD0 File Offset: 0x0010DDD0
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

		// Token: 0x06003B09 RID: 15113 RVA: 0x0010FC9B File Offset: 0x0010DE9B
		public bool CanPickMole()
		{
			return this.currentState == Mole.MoleState.Ready;
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x0010FCA8 File Offset: 0x0010DEA8
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

		// Token: 0x06003B0B RID: 15115 RVA: 0x0010FD80 File Offset: 0x0010DF80
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

		// Token: 0x06003B0C RID: 15116 RVA: 0x0010FE00 File Offset: 0x0010E000
		public bool CanTap()
		{
			Mole.MoleState moleState = this.currentState;
			return moleState == Mole.MoleState.TransitionToVisible || moleState == Mole.MoleState.Visible;
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x0010FE25 File Offset: 0x0010E025
		public override bool CanTap(bool isLeftHand)
		{
			return this.CanTap();
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x0010FE30 File Offset: 0x0010E030
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

		// Token: 0x06003B0F RID: 15119 RVA: 0x0010FECF File Offset: 0x0010E0CF
		public void ResetPosition()
		{
			base.transform.position = this.hiddenPosition;
			this.currentState = Mole.MoleState.Reset;
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x0010FEE9 File Offset: 0x0010E0E9
		public int GetMoleTypeIndex(bool useHazardMole)
		{
			if (!useHazardMole)
			{
				return this.safeMoles[Random.Range(0, this.safeMoles.Count)];
			}
			return this.hazardMoles[Random.Range(0, this.hazardMoles.Count)];
		}

		// Token: 0x04003C0E RID: 15374
		public float positionOffset = 0.2f;

		// Token: 0x04003C0F RID: 15375
		public MoleTypes[] moleTypes;

		// Token: 0x04003C10 RID: 15376
		private float showMoleDuration;

		// Token: 0x04003C11 RID: 15377
		private Vector3 visiblePosition;

		// Token: 0x04003C12 RID: 15378
		private Vector3 hiddenPosition;

		// Token: 0x04003C13 RID: 15379
		private float currentTime;

		// Token: 0x04003C14 RID: 15380
		private float animStartTime;

		// Token: 0x04003C15 RID: 15381
		private float travelTime;

		// Token: 0x04003C16 RID: 15382
		private float normalTravelTime = 0.3f;

		// Token: 0x04003C17 RID: 15383
		private float hitTravelTime = 0.2f;

		// Token: 0x04003C18 RID: 15384
		private AnimationCurve animCurve;

		// Token: 0x04003C19 RID: 15385
		private AnimationCurve normalAnimCurve;

		// Token: 0x04003C1A RID: 15386
		private AnimationCurve hitAnimCurve;

		// Token: 0x04003C1B RID: 15387
		private Mole.MoleState currentState;

		// Token: 0x04003C1C RID: 15388
		private Vector3 origin;

		// Token: 0x04003C1D RID: 15389
		private Vector3 target;

		// Token: 0x04003C1E RID: 15390
		private int randomMolePickedIndex;

		// Token: 0x04003C20 RID: 15392
		public CallLimiter rpcCooldown;

		// Token: 0x04003C21 RID: 15393
		private int moleScore;

		// Token: 0x04003C22 RID: 15394
		private List<int> safeMoles = new List<int>();

		// Token: 0x04003C23 RID: 15395
		private List<int> hazardMoles = new List<int>();

		// Token: 0x02000975 RID: 2421
		// (Invoke) Token: 0x06003B13 RID: 15123
		public delegate void MoleTapEvent(MoleTypes moleType, Vector3 position, bool isLocalTap, bool isLeft);

		// Token: 0x02000976 RID: 2422
		public enum MoleState
		{
			// Token: 0x04003C26 RID: 15398
			Reset,
			// Token: 0x04003C27 RID: 15399
			Ready,
			// Token: 0x04003C28 RID: 15400
			TransitionToVisible,
			// Token: 0x04003C29 RID: 15401
			Visible,
			// Token: 0x04003C2A RID: 15402
			TransitionToHidden,
			// Token: 0x04003C2B RID: 15403
			Hidden
		}
	}
}
