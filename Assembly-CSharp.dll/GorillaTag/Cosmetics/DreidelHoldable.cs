using System;
using GorillaExtensions;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C2F RID: 3119
	public class DreidelHoldable : TransferrableObject
	{
		// Token: 0x06004DCD RID: 19917 RVA: 0x001ADE80 File Offset: 0x001AC080
		internal override void OnEnable()
		{
			base.OnEnable();
			if (this._events == null)
			{
				this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
				NetPlayer netPlayer = (base.myOnlineRig != null) ? base.myOnlineRig.creator : ((base.myRig != null) ? ((base.myRig.creator != null) ? base.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
				if (netPlayer != null)
				{
					this._events.Init(netPlayer);
				}
				else
				{
					Debug.LogError("Failed to get a reference to the Photon Player needed to hook up the cosmetic event");
				}
			}
			if (this._events != null)
			{
				this._events.Activate += this.OnDreidelSpin;
			}
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x001ADF50 File Offset: 0x001AC150
		internal override void OnDisable()
		{
			base.OnDisable();
			if (this._events != null)
			{
				this._events.Activate -= this.OnDreidelSpin;
				UnityEngine.Object.Destroy(this._events);
				this._events = null;
			}
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x001ADFA8 File Offset: 0x001AC1A8
		private void OnDreidelSpin(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
		{
			GorillaNot.IncrementRPCCall(info, "OnDreidelSpin");
			if (sender != target)
			{
				return;
			}
			if (info.senderID != this.ownerRig.creator.ActorNumber)
			{
				return;
			}
			Vector3 surfacePoint = (Vector3)args[0];
			Vector3 surfaceNormal = (Vector3)args[1];
			float num = (float)args[2];
			double num2 = (double)args[6];
			float num3 = 10000f;
			if (surfacePoint.IsValid(num3))
			{
				float num4 = 10000f;
				if (surfaceNormal.IsValid(num4) && float.IsFinite(num) && double.IsFinite(num2))
				{
					bool counterClockwise = (bool)args[3];
					Dreidel.Side side = (Dreidel.Side)args[4];
					Dreidel.Variation variation = (Dreidel.Variation)args[5];
					this.StartSpinLocal(surfacePoint, surfaceNormal, num, counterClockwise, side, variation, num2);
					return;
				}
			}
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x00062023 File Offset: 0x00060223
		public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
		{
			base.OnGrab(pointGrabbed, grabbingHand);
			if (this.dreidelAnimation != null)
			{
				this.dreidelAnimation.TryCheckForSurfaces();
			}
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x00062047 File Offset: 0x00060247
		public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
		{
			if (!base.OnRelease(zoneReleased, releasingHand))
			{
				return false;
			}
			if (this.dreidelAnimation != null)
			{
				this.dreidelAnimation.TrySetIdle();
			}
			return true;
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x001AE068 File Offset: 0x001AC268
		public override void OnActivate()
		{
			base.OnActivate();
			Vector3 vector;
			Vector3 vector2;
			float num;
			Dreidel.Side side;
			Dreidel.Variation variation;
			double num2;
			if (this.dreidelAnimation != null && this.dreidelAnimation.TryGetSpinStartData(out vector, out vector2, out num, out side, out variation, out num2))
			{
				bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand;
				if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
				{
					object[] args = new object[]
					{
						vector,
						vector2,
						num,
						flag,
						(int)side,
						(int)variation,
						num2
					};
					this._events.Activate.RaiseAll(args);
					return;
				}
				this.StartSpinLocal(vector, vector2, num, flag, side, variation, num2);
			}
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x00062070 File Offset: 0x00060270
		private void StartSpinLocal(Vector3 surfacePoint, Vector3 surfaceNormal, float duration, bool counterClockwise, Dreidel.Side side, Dreidel.Variation variation, double startTime)
		{
			if (this.dreidelAnimation != null)
			{
				this.dreidelAnimation.SetSpinStartData(surfacePoint, surfaceNormal, duration, counterClockwise, side, variation, startTime);
				this.dreidelAnimation.Spin();
			}
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x001AE148 File Offset: 0x001AC348
		public void DebugSpinDreidel()
		{
			Transform transform = GTPlayer.Instance.headCollider.transform;
			Vector3 origin = transform.position + transform.forward * 0.5f;
			float maxDistance = 2f;
			RaycastHit raycastHit;
			if (Physics.Raycast(origin, Vector3.down, out raycastHit, maxDistance, GTPlayer.Instance.locomotionEnabledLayers.value, QueryTriggerInteraction.Ignore))
			{
				Vector3 point = raycastHit.point;
				Vector3 normal = raycastHit.normal;
				float num = UnityEngine.Random.Range(7f, 10f);
				Dreidel.Side side = (Dreidel.Side)UnityEngine.Random.Range(0, 4);
				Dreidel.Variation variation = (Dreidel.Variation)UnityEngine.Random.Range(0, 5);
				bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand;
				double num2 = PhotonNetwork.InRoom ? PhotonNetwork.Time : -1.0;
				if (PhotonNetwork.InRoom && this._events != null && this._events.Activate != null)
				{
					object[] args = new object[]
					{
						point,
						normal,
						num,
						flag,
						(int)side,
						(int)variation,
						num2
					};
					this._events.Activate.RaiseAll(args);
					return;
				}
				this.StartSpinLocal(point, normal, num, flag, side, variation, num2);
			}
		}

		// Token: 0x040050DC RID: 20700
		[SerializeField]
		private Dreidel dreidelAnimation;

		// Token: 0x040050DD RID: 20701
		private RubberDuckEvents _events;
	}
}
