using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000971 RID: 2417
	public class Mole : Tappable
	{
		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06003AF7 RID: 15095 RVA: 0x0010F4A0 File Offset: 0x0010D6A0
		// (remove) Token: 0x06003AF8 RID: 15096 RVA: 0x0010F4D8 File Offset: 0x0010D6D8
		public event Mole.MoleTapEvent OnTapped;

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x0010F50D File Offset: 0x0010D70D
		// (set) Token: 0x06003AFA RID: 15098 RVA: 0x0010F515 File Offset: 0x0010D715
		public bool IsLeftSideMole { get; set; }

		// Token: 0x06003AFB RID: 15099 RVA: 0x0010F520 File Offset: 0x0010D720
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

		// Token: 0x06003AFC RID: 15100 RVA: 0x0010F608 File Offset: 0x0010D808
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

		// Token: 0x06003AFD RID: 15101 RVA: 0x0010F6D3 File Offset: 0x0010D8D3
		public bool CanPickMole()
		{
			return this.currentState == Mole.MoleState.Ready;
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x0010F6E0 File Offset: 0x0010D8E0
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

		// Token: 0x06003AFF RID: 15103 RVA: 0x0010F7B8 File Offset: 0x0010D9B8
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

		// Token: 0x06003B00 RID: 15104 RVA: 0x0010F838 File Offset: 0x0010DA38
		public bool CanTap()
		{
			Mole.MoleState moleState = this.currentState;
			return moleState == Mole.MoleState.TransitionToVisible || moleState == Mole.MoleState.Visible;
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x0010F85D File Offset: 0x0010DA5D
		public override bool CanTap(bool isLeftHand)
		{
			return this.CanTap();
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x0010F868 File Offset: 0x0010DA68
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

		// Token: 0x06003B03 RID: 15107 RVA: 0x0010F907 File Offset: 0x0010DB07
		public void ResetPosition()
		{
			base.transform.position = this.hiddenPosition;
			this.currentState = Mole.MoleState.Reset;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x0010F921 File Offset: 0x0010DB21
		public int GetMoleTypeIndex(bool useHazardMole)
		{
			if (!useHazardMole)
			{
				return this.safeMoles[Random.Range(0, this.safeMoles.Count)];
			}
			return this.hazardMoles[Random.Range(0, this.hazardMoles.Count)];
		}

		// Token: 0x04003BFC RID: 15356
		public float positionOffset = 0.2f;

		// Token: 0x04003BFD RID: 15357
		public MoleTypes[] moleTypes;

		// Token: 0x04003BFE RID: 15358
		private float showMoleDuration;

		// Token: 0x04003BFF RID: 15359
		private Vector3 visiblePosition;

		// Token: 0x04003C00 RID: 15360
		private Vector3 hiddenPosition;

		// Token: 0x04003C01 RID: 15361
		private float currentTime;

		// Token: 0x04003C02 RID: 15362
		private float animStartTime;

		// Token: 0x04003C03 RID: 15363
		private float travelTime;

		// Token: 0x04003C04 RID: 15364
		private float normalTravelTime = 0.3f;

		// Token: 0x04003C05 RID: 15365
		private float hitTravelTime = 0.2f;

		// Token: 0x04003C06 RID: 15366
		private AnimationCurve animCurve;

		// Token: 0x04003C07 RID: 15367
		private AnimationCurve normalAnimCurve;

		// Token: 0x04003C08 RID: 15368
		private AnimationCurve hitAnimCurve;

		// Token: 0x04003C09 RID: 15369
		private Mole.MoleState currentState;

		// Token: 0x04003C0A RID: 15370
		private Vector3 origin;

		// Token: 0x04003C0B RID: 15371
		private Vector3 target;

		// Token: 0x04003C0C RID: 15372
		private int randomMolePickedIndex;

		// Token: 0x04003C0E RID: 15374
		public CallLimiter rpcCooldown;

		// Token: 0x04003C0F RID: 15375
		private int moleScore;

		// Token: 0x04003C10 RID: 15376
		private List<int> safeMoles = new List<int>();

		// Token: 0x04003C11 RID: 15377
		private List<int> hazardMoles = new List<int>();

		// Token: 0x02000972 RID: 2418
		// (Invoke) Token: 0x06003B07 RID: 15111
		public delegate void MoleTapEvent(MoleTypes moleType, Vector3 position, bool isLocalTap, bool isLeft);

		// Token: 0x02000973 RID: 2419
		public enum MoleState
		{
			// Token: 0x04003C14 RID: 15380
			Reset,
			// Token: 0x04003C15 RID: 15381
			Ready,
			// Token: 0x04003C16 RID: 15382
			TransitionToVisible,
			// Token: 0x04003C17 RID: 15383
			Visible,
			// Token: 0x04003C18 RID: 15384
			TransitionToHidden,
			// Token: 0x04003C19 RID: 15385
			Hidden
		}
	}
}
