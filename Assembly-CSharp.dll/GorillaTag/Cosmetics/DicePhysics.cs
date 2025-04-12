using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C1F RID: 3103
	public class DicePhysics : MonoBehaviour
	{
		// Token: 0x06004D60 RID: 19808 RVA: 0x001A95B4 File Offset: 0x001A77B4
		public int GetRandomSide()
		{
			DicePhysics.DiceType diceType = this.diceType;
			if (diceType != DicePhysics.DiceType.D6)
			{
				if (this.forceLandingSide)
				{
					return Mathf.Clamp(this.forcedLandingSide, 1, 20);
				}
				int value;
				if (this.CheckCosmeticRollOverride(out value))
				{
					return Mathf.Clamp(value, 1, 20);
				}
				return UnityEngine.Random.Range(1, 21);
			}
			else
			{
				if (this.forceLandingSide)
				{
					return Mathf.Clamp(this.forcedLandingSide, 1, 6);
				}
				int value2;
				if (this.CheckCosmeticRollOverride(out value2))
				{
					return Mathf.Clamp(value2, 1, 6);
				}
				return UnityEngine.Random.Range(1, 7);
			}
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x001A9634 File Offset: 0x001A7834
		public Vector3 GetSideDirection(int side)
		{
			DicePhysics.DiceType diceType = this.diceType;
			if (diceType != DicePhysics.DiceType.D6)
			{
				int num = Mathf.Clamp(side - 1, 0, 19);
				return this.d20SideDirections[num];
			}
			int num2 = Mathf.Clamp(side - 1, 0, 5);
			return this.d6SideDirections[num2];
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x001A9680 File Offset: 0x001A7880
		public void StartThrow(DiceHoldable holdable, Vector3 startPosition, Vector3 velocity, float playerScale, int side, double startTime)
		{
			this.holdableParent = holdable;
			base.transform.parent = null;
			base.transform.position = startPosition;
			base.transform.localScale = Vector3.one * playerScale;
			this.rb.isKinematic = false;
			this.rb.useGravity = true;
			this.rb.velocity = velocity;
			if (!this.allowPickupFromGround && this.interactionPoint != null)
			{
				this.interactionPoint.enabled = false;
			}
			this.throwStartTime = ((startTime > 0.0) ? startTime : ((double)Time.time));
			this.throwSettledTime = -1.0;
			this.scale = playerScale;
			this.landingSide = Mathf.Clamp(side, 1, 20);
			this.prevVelocity = Vector3.zero;
			velocity = Vector3.zero;
			base.enabled = true;
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x001A9768 File Offset: 0x001A7968
		public void EndThrow()
		{
			this.rb.isKinematic = true;
			this.rb.velocity = Vector3.zero;
			if (this.holdableParent != null)
			{
				base.transform.parent = this.holdableParent.transform;
			}
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			base.transform.localScale = Vector3.one;
			this.scale = 1f;
			this.throwStartTime = -1.0;
			if (this.interactionPoint != null)
			{
				this.interactionPoint.enabled = true;
			}
			this.onRollFinished.Invoke();
			base.enabled = false;
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x001A982C File Offset: 0x001A7A2C
		private void FixedUpdate()
		{
			double num = PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time);
			float num2 = (float)(num - this.throwStartTime);
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 0.1f * this.scale, this.surfaceLayers.value, QueryTriggerInteraction.Ignore))
			{
				Vector3 normal = raycastHit.normal;
				Vector3 sideDirection = this.GetSideDirection(this.landingSide);
				Vector3 vector = base.transform.rotation * sideDirection;
				Vector3 normalized = Vector3.Cross(vector, normal).normalized;
				float num3 = Vector3.SignedAngle(vector, normal, normalized);
				float num4 = Mathf.Sign(num3) * this.angleDeltaVsStrengthCurve.Evaluate(Mathf.Clamp01(Mathf.Abs(num3) / 180f));
				float num5 = this.landingTimeVsStrengthCurve.Evaluate(Mathf.Clamp01(num2 / this.landingTime));
				float magnitude = this.rb.velocity.magnitude;
				float num6 = Mathf.Clamp01(1f - Mathf.Min(magnitude, 1f));
				float num7 = Mathf.Max(num5, num6);
				Vector3 torque = this.strength * (num7 * num4 * normalized) - this.damping * this.rb.angularVelocity;
				this.rb.AddTorque(torque, ForceMode.Acceleration);
				if (!this.rb.isKinematic && magnitude < 0.01f && num3 < 2f)
				{
					this.rb.isKinematic = true;
					this.throwSettledTime = num;
					this.InvokeLandingEffects(this.landingSide);
				}
				else if (!this.rb.isKinematic && num2 > this.landingTime)
				{
					this.rb.isKinematic = true;
					this.throwSettledTime = num;
					base.transform.rotation = Quaternion.FromToRotation(vector, normal) * base.transform.rotation;
					this.InvokeLandingEffects(this.landingSide);
				}
			}
			if (num2 > this.landingTime + this.postLandingTime || (this.rb.isKinematic && (float)(num - this.throwSettledTime) > this.postLandingTime))
			{
				this.EndThrow();
			}
			this.prevVelocity = this.velocity;
			this.velocity = this.rb.velocity;
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x001A9A78 File Offset: 0x001A7C78
		private void OnCollisionEnter(Collision collision)
		{
			float magnitude = collision.impulse.magnitude;
			if (magnitude > 0.001f)
			{
				Vector3 vector = Vector3.Reflect(this.prevVelocity, collision.impulse / magnitude);
				this.rb.velocity = vector * this.bounceAmplification;
			}
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x001A9ACC File Offset: 0x001A7CCC
		private void InvokeLandingEffects(int side)
		{
			DicePhysics.DiceType diceType = this.diceType;
			if (diceType != DicePhysics.DiceType.D6)
			{
				if (side == 20)
				{
					this.onBestRoll.Invoke();
					return;
				}
				if (side == 1)
				{
					this.onWorstRoll.Invoke();
					return;
				}
			}
			else
			{
				if (side == 6)
				{
					this.onBestRoll.Invoke();
					return;
				}
				if (side == 1)
				{
					this.onWorstRoll.Invoke();
				}
			}
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x001A9B28 File Offset: 0x001A7D28
		private bool CheckCosmeticRollOverride(out int rollSide)
		{
			if (this.cosmeticRollOverrides.Length != 0)
			{
				if (this.cachedLocalRig == null)
				{
					RigContainer rigContainer;
					if (PhotonNetwork.InRoom && VRRigCache.Instance.TryGetVrrig(PhotonNetwork.LocalPlayer, out rigContainer) && rigContainer.Rig != null)
					{
						this.cachedLocalRig = rigContainer.Rig;
					}
					else
					{
						this.cachedLocalRig = GorillaTagger.Instance.offlineVRRig;
					}
				}
				if (this.cachedLocalRig != null)
				{
					int num = -1;
					for (int i = 0; i < this.cosmeticRollOverrides.Length; i++)
					{
						if (this.cosmeticRollOverrides[i].cosmeticName != null && this.cachedLocalRig.cosmeticSet != null && this.cachedLocalRig.cosmeticSet.HasItem(this.cosmeticRollOverrides[i].cosmeticName) && (!this.cosmeticRollOverrides[i].requireHolding || (EquipmentInteractor.instance.leftHandHeldEquipment != null && EquipmentInteractor.instance.leftHandHeldEquipment.name == this.cosmeticRollOverrides[i].cosmeticName) || (EquipmentInteractor.instance.rightHandHeldEquipment != null && EquipmentInteractor.instance.rightHandHeldEquipment.name == this.cosmeticRollOverrides[i].cosmeticName)) && this.cosmeticRollOverrides[i].landingSide > num)
						{
							num = this.cosmeticRollOverrides[i].landingSide;
						}
					}
					if (num > 0)
					{
						rollSide = num;
						return true;
					}
				}
			}
			rollSide = 0;
			return false;
		}

		// Token: 0x04004FD9 RID: 20441
		[SerializeField]
		private DicePhysics.DiceType diceType = DicePhysics.DiceType.D20;

		// Token: 0x04004FDA RID: 20442
		[SerializeField]
		private float landingTime = 5f;

		// Token: 0x04004FDB RID: 20443
		[SerializeField]
		private float postLandingTime = 2f;

		// Token: 0x04004FDC RID: 20444
		[SerializeField]
		private LayerMask surfaceLayers;

		// Token: 0x04004FDD RID: 20445
		[SerializeField]
		private AnimationCurve angleDeltaVsStrengthCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004FDE RID: 20446
		[SerializeField]
		private AnimationCurve landingTimeVsStrengthCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04004FDF RID: 20447
		[SerializeField]
		private float strength = 1f;

		// Token: 0x04004FE0 RID: 20448
		[SerializeField]
		private float damping = 0.5f;

		// Token: 0x04004FE1 RID: 20449
		[SerializeField]
		private bool forceLandingSide;

		// Token: 0x04004FE2 RID: 20450
		[SerializeField]
		private int forcedLandingSide = 20;

		// Token: 0x04004FE3 RID: 20451
		[SerializeField]
		private bool allowPickupFromGround = true;

		// Token: 0x04004FE4 RID: 20452
		[SerializeField]
		private float bounceAmplification = 1f;

		// Token: 0x04004FE5 RID: 20453
		[SerializeField]
		private DicePhysics.CosmeticRollOverride[] cosmeticRollOverrides;

		// Token: 0x04004FE6 RID: 20454
		[SerializeField]
		private UnityEvent onBestRoll;

		// Token: 0x04004FE7 RID: 20455
		[SerializeField]
		private UnityEvent onWorstRoll;

		// Token: 0x04004FE8 RID: 20456
		[SerializeField]
		private UnityEvent onRollFinished;

		// Token: 0x04004FE9 RID: 20457
		[SerializeField]
		private Rigidbody rb;

		// Token: 0x04004FEA RID: 20458
		[SerializeField]
		private InteractionPoint interactionPoint;

		// Token: 0x04004FEB RID: 20459
		private VRRig cachedLocalRig;

		// Token: 0x04004FEC RID: 20460
		private DiceHoldable holdableParent;

		// Token: 0x04004FED RID: 20461
		private double throwStartTime = -1.0;

		// Token: 0x04004FEE RID: 20462
		private double throwSettledTime = -1.0;

		// Token: 0x04004FEF RID: 20463
		private int landingSide;

		// Token: 0x04004FF0 RID: 20464
		private float scale;

		// Token: 0x04004FF1 RID: 20465
		private Vector3 prevVelocity = Vector3.zero;

		// Token: 0x04004FF2 RID: 20466
		private Vector3 velocity = Vector3.zero;

		// Token: 0x04004FF3 RID: 20467
		private const float a = 38.833332f;

		// Token: 0x04004FF4 RID: 20468
		private const float b = 77.66666f;

		// Token: 0x04004FF5 RID: 20469
		private Vector3[] d20SideDirections = new Vector3[]
		{
			Quaternion.AngleAxis(144f, Vector3.up) * Quaternion.AngleAxis(38.833332f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(324f, -Vector3.up) * Quaternion.AngleAxis(38.833332f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(288f, Vector3.up) * Quaternion.AngleAxis(38.833332f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(180f, -Vector3.up) * Quaternion.AngleAxis(38.833332f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(252f, -Vector3.up) * Quaternion.AngleAxis(77.66666f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(108f, -Vector3.up) * Quaternion.AngleAxis(77.66666f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(72f, Vector3.up) * Quaternion.AngleAxis(38.833332f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(36f, -Vector3.up) * Quaternion.AngleAxis(77.66666f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(216f, Vector3.up) * Quaternion.AngleAxis(77.66666f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(0f, Vector3.up) * Quaternion.AngleAxis(77.66666f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(180f, -Vector3.up) * Quaternion.AngleAxis(77.66666f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(324f, -Vector3.up) * Quaternion.AngleAxis(77.66666f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(144f, Vector3.up) * Quaternion.AngleAxis(77.66666f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(108f, -Vector3.up) * Quaternion.AngleAxis(38.833332f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(72f, Vector3.up) * Quaternion.AngleAxis(77.66666f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(288f, Vector3.up) * Quaternion.AngleAxis(77.66666f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(0f, Vector3.up) * Quaternion.AngleAxis(38.833332f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(252f, -Vector3.up) * Quaternion.AngleAxis(38.833332f, -Vector3.forward) * Vector3.up,
			Quaternion.AngleAxis(216f, Vector3.up) * Quaternion.AngleAxis(38.833332f, Vector3.forward) * -Vector3.up,
			Quaternion.AngleAxis(36f, -Vector3.up) * Quaternion.AngleAxis(38.833332f, -Vector3.forward) * Vector3.up
		};

		// Token: 0x04004FF6 RID: 20470
		private Vector3[] d6SideDirections = new Vector3[]
		{
			new Vector3(0f, -1f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, 1f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 1f, 0f)
		};

		// Token: 0x02000C20 RID: 3104
		private enum DiceType
		{
			// Token: 0x04004FF8 RID: 20472
			D6,
			// Token: 0x04004FF9 RID: 20473
			D20
		}

		// Token: 0x02000C21 RID: 3105
		[Serializable]
		private struct CosmeticRollOverride
		{
			// Token: 0x04004FFA RID: 20474
			public string cosmeticName;

			// Token: 0x04004FFB RID: 20475
			public int landingSide;

			// Token: 0x04004FFC RID: 20476
			public bool requireHolding;
		}
	}
}
