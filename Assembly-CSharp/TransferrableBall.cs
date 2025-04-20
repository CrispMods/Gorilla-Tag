using System;
using System.Collections.Generic;
using CjLib;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000402 RID: 1026
public class TransferrableBall : TransferrableObject
{
	// Token: 0x0600192D RID: 6445 RVA: 0x000CEF54 File Offset: 0x000CD154
	public override void TriggeredLateUpdate()
	{
		base.TriggeredLateUpdate();
		if (Time.time - this.hitSoundSpamLastHitTime > this.hitSoundSpamCooldownResetTime)
		{
			this.hitSoundSpamCount = 0;
		}
		bool flag = false;
		bool flag2 = false;
		float num = 1f;
		bool flag3 = this.leftHandOverlapping;
		bool flag4 = this.rightHandOverlapping;
		GTPlayer instance = GTPlayer.Instance;
		bool flag5 = false;
		foreach (KeyValuePair<GorillaHandClimber, int> keyValuePair in this.handClimberMap)
		{
			if (keyValuePair.Value > 0)
			{
				flag2 = true;
				Vector3 a = Vector3.zero;
				bool flag6 = keyValuePair.Key.xrNode == XRNode.LeftHand;
				Vector3 vector;
				Vector3 a2;
				if (flag6)
				{
					Vector3 position = instance.leftHandFollower.position;
					Quaternion rotation = instance.leftHandFollower.rotation;
					float num2;
					this.leftHandOverlapping = this.CheckCollisionWithHand(position, rotation, rotation * Vector3.right, out vector, out a2, out num2);
					if (this.leftHandOverlapping)
					{
						a = instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0.15f, false);
					}
					else if ((position - base.transform.position).sqrMagnitude > num * num)
					{
						this.handClimberMap[keyValuePair.Key] = 0;
						continue;
					}
				}
				else
				{
					Vector3 position2 = instance.rightHandFollower.position;
					Quaternion rotation2 = instance.rightHandFollower.rotation;
					float num2;
					this.rightHandOverlapping = this.CheckCollisionWithHand(position2, rotation2, rotation2 * -Vector3.right, out vector, out a2, out num2);
					if (this.rightHandOverlapping)
					{
						a = instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0.15f, false);
					}
					else if ((position2 - base.transform.position).sqrMagnitude > num * num)
					{
						this.handClimberMap[keyValuePair.Key] = 0;
						continue;
					}
				}
				if ((this.leftHandOverlapping || this.rightHandOverlapping) && (this.currentState == TransferrableObject.PositionState.None || this.currentState == TransferrableObject.PositionState.Dropped))
				{
					if (this.applyFrictionHolding)
					{
						if (flag6 && this.leftHandOverlapping)
						{
							if (!flag3)
							{
								Vector3 normalized = (instance.leftHandFollower.position - base.transform.position).normalized;
								Vector3 position3 = normalized * this.ballRadius + base.transform.position;
								this.frictionHoldLocalPosLeft = base.transform.InverseTransformPoint(position3);
								this.frictionHoldLocalRotLeft = Quaternion.LookRotation(normalized, instance.leftHandFollower.forward);
							}
							Vector3 vector2 = base.transform.TransformPoint(this.frictionHoldLocalPosLeft);
							this.frictionHoldLocalRotLeft = Quaternion.LookRotation(vector2 - base.transform.position, instance.leftHandFollower.forward);
							if (this.debugDraw)
							{
								Quaternion rotation3 = this.frictionHoldLocalRotLeft * Quaternion.AngleAxis(90f, Vector3.right);
								DebugUtil.DrawRect(vector2, rotation3, new Vector2(0.08f, 0.15f), Color.green, false, DebugUtil.Style.Wireframe);
								Vector3 normalized2 = (instance.leftHandFollower.position - base.transform.position).normalized;
								Vector3 center = normalized2 * this.ballRadius + base.transform.position;
								Quaternion rotation4 = Quaternion.LookRotation(normalized2, instance.leftHandFollower.forward) * Quaternion.AngleAxis(90f, Vector3.right);
								DebugUtil.DrawRect(center, rotation4, new Vector2(0.08f, 0.15f), Color.yellow, false, DebugUtil.Style.Wireframe);
							}
						}
						else if (!flag6 && this.rightHandOverlapping)
						{
							if (!flag4)
							{
								Vector3 normalized3 = (instance.rightHandFollower.position - base.transform.position).normalized;
								Vector3 position4 = normalized3 * this.ballRadius + base.transform.position;
								this.frictionHoldLocalPosRight = base.transform.InverseTransformPoint(position4);
								this.frictionHoldLocalRotRight = Quaternion.LookRotation(normalized3, instance.rightHandFollower.forward);
							}
							Vector3 vector3 = base.transform.TransformPoint(this.frictionHoldLocalPosRight);
							this.frictionHoldLocalRotRight = Quaternion.LookRotation(vector3 - base.transform.position, instance.rightHandFollower.forward);
							if (this.debugDraw)
							{
								Quaternion rotation5 = this.frictionHoldLocalRotRight * Quaternion.AngleAxis(90f, Vector3.right);
								DebugUtil.DrawRect(vector3, rotation5, new Vector2(0.08f, 0.15f), Color.green, false, DebugUtil.Style.Wireframe);
								Vector3 normalized4 = (instance.rightHandFollower.position - base.transform.position).normalized;
								Vector3 center2 = normalized4 * this.ballRadius + base.transform.position;
								Quaternion rotation6 = Quaternion.LookRotation(normalized4, instance.rightHandFollower.forward) * Quaternion.AngleAxis(90f, Vector3.right);
								DebugUtil.DrawRect(center2, rotation6, new Vector2(0.08f, 0.15f), Color.yellow, false, DebugUtil.Style.Wireframe);
							}
						}
					}
					bool flag7 = (flag6 && this.leftHandOverlapping && !flag3) || (!flag6 && this.rightHandOverlapping && !flag4);
					if (!flag5 && flag7)
					{
						Vector3 vector4 = flag6 ? instance.leftHandFollower.position : instance.rightHandFollower.position;
						float magnitude = a.magnitude;
						Vector3 a3 = a / magnitude;
						Vector3 b = -(vector4 - base.transform.position).normalized;
						Vector3 hitDir = (a3 + b) * 0.5f;
						flag5 = this.ApplyHit(vector4, hitDir, magnitude);
					}
					if (!flag5)
					{
						Vector3 vector5 = flag6 ? instance.leftHandFollower.position : instance.rightHandFollower.position;
						Vector3 a4 = vector5 - base.transform.position;
						float magnitude2 = a4.magnitude;
						float num3 = this.ballRadius - a4.magnitude;
						if (num3 > 0f)
						{
							Vector3 vector6 = -(a4 / magnitude2) * num3;
							this.rigidbodyInstance.AddForce(-(a4 / magnitude2) * this.depenetrationSpeed * Time.deltaTime, ForceMode.VelocityChange);
							if (this.collisionContactsCount == 0)
							{
								this.rigidbodyInstance.MovePosition(base.transform.position + vector6 * this.depenetrationBias);
							}
							if (this.debugDraw)
							{
								DebugUtil.DrawLine(vector5, vector5 - vector6, Color.magenta, false);
							}
						}
					}
					if (this.debugDraw)
					{
						DebugUtil.DrawSphere(vector, 0.01f, 6, 6, Color.green, true, DebugUtil.Style.SolidColor);
						DebugUtil.DrawArrow(vector, vector - a2 * 0.05f, 0.01f, Color.green, true, DebugUtil.Style.Wireframe);
					}
				}
				flag = (flag || this.leftHandOverlapping || this.rightHandOverlapping);
			}
		}
		bool flag8 = this.headOverlapping;
		this.headOverlapping = false;
		if (this.allowHeadButting && !flag5 && this.playerHeadCollider != null)
		{
			Vector3 hitPoint;
			Vector3 vector7;
			float num4;
			this.headOverlapping = this.CheckCollisionWithHead(this.playerHeadCollider, out hitPoint, out vector7, out num4);
			Vector3 averagedVelocity = instance.AveragedVelocity;
			float magnitude3 = averagedVelocity.magnitude;
			if (this.headOverlapping && !flag8 && (double)magnitude3 > 0.0001)
			{
				Vector3 hitDir2 = averagedVelocity / magnitude3;
				flag5 = this.ApplyHit(hitPoint, hitDir2, magnitude3 * this.headButtHitMultiplier);
			}
			else if ((this.playerHeadCollider.transform.position - base.transform.position).sqrMagnitude > num * num)
			{
				this.playerHeadCollider = null;
			}
		}
		if (this.debugDraw && this.onGround)
		{
			DebugUtil.DrawLine(this.groundContact.point, this.groundContact.point + this.groundContact.normal * 0.2f, Color.yellow, false);
			DebugUtil.DrawRect(this.groundContact.point, Quaternion.LookRotation(this.groundContact.normal) * Quaternion.AngleAxis(90f, Vector3.right), Vector2.one * 0.2f, Color.yellow, false, DebugUtil.Style.Wireframe);
		}
		if (flag2 && this.debugDraw)
		{
			DebugUtil.DrawSphereTripleCircles(base.transform.position, this.ballRadius, 16, flag ? Color.green : Color.white, true, DebugUtil.Style.Wireframe);
			for (int i = 0; i < this.collisionContactsCount; i++)
			{
				ContactPoint contactPoint = this.collisionContacts[i];
				DebugUtil.DrawArrow(contactPoint.point, contactPoint.point + contactPoint.normal * 0.2f, 0.02f, Color.red, false, DebugUtil.Style.Wireframe);
			}
		}
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x000CF8AC File Offset: 0x000CDAAC
	private void TakeOwnershipAndEnablePhysics()
	{
		this.currentState = TransferrableObject.PositionState.Dropped;
		this.rigidbodyInstance.isKinematic = false;
		if (this.worldShareableInstance != null)
		{
			if (!this.worldShareableInstance.guard.isTrulyMine)
			{
				this.worldShareableInstance.guard.RequestOwnershipImmediately(delegate
				{
				});
			}
			this.worldShareableInstance.transferableObjectState = this.currentState;
		}
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000CF930 File Offset: 0x000CDB30
	private bool CheckCollisionWithHand(Vector3 handCenter, Quaternion handRotation, Vector3 palmForward, out Vector3 hitPoint, out Vector3 hitNormal, out float penetrationDist)
	{
		Vector3 position = base.transform.position;
		bool flag = false;
		hitPoint = position;
		hitNormal = Vector3.forward;
		penetrationDist = 0f;
		Vector3 lhs = position - handCenter;
		Vector3 vector = position - Vector3.Dot(lhs, palmForward) * palmForward;
		Vector3 vector2 = vector;
		if ((vector - handCenter).sqrMagnitude > this.handRadius * this.handRadius)
		{
			vector2 = handCenter + (vector - handCenter).normalized * this.handRadius;
		}
		if ((vector2 - position).sqrMagnitude < this.ballRadius * this.ballRadius)
		{
			flag = true;
			hitNormal = (position - vector2).normalized;
			hitPoint = position - hitNormal * this.ballRadius;
			penetrationDist = this.ballRadius - (vector2 - position).magnitude;
		}
		if (this.debugDraw)
		{
			Color color = flag ? Color.green : Color.white;
			DebugUtil.DrawCircle(handCenter, handRotation * Quaternion.AngleAxis(90f, Vector3.forward), this.handRadius, 16, color, true, DebugUtil.Style.Wireframe);
			DebugUtil.DrawArrow(handCenter, handCenter + palmForward * 0.05f, 0.01f, color, true, DebugUtil.Style.Wireframe);
		}
		return flag;
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x000CFAA0 File Offset: 0x000CDCA0
	private bool CheckCollisionWithHead(SphereCollider headCollider, out Vector3 hitPoint, out Vector3 hitNormal, out float penetrationDist)
	{
		Vector3 a = base.transform.position - headCollider.transform.position;
		float sqrMagnitude = a.sqrMagnitude;
		float num = this.ballRadius + this.headButtRadius;
		if (sqrMagnitude < num * num)
		{
			float num2 = Mathf.Sqrt(sqrMagnitude);
			hitNormal = a / num2;
			penetrationDist = num - num2;
			hitPoint = headCollider.transform.position + hitNormal * this.headButtRadius;
			return true;
		}
		hitNormal = Vector3.forward;
		hitPoint = Vector3.zero;
		penetrationDist = 0f;
		return false;
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x000CFB48 File Offset: 0x000CDD48
	private bool ApplyHit(Vector3 hitPoint, Vector3 hitDir, float hitSpeed)
	{
		bool result = false;
		this.TakeOwnershipAndEnablePhysics();
		float num = 0f;
		Vector3 vector = Vector3.zero;
		if (hitSpeed > 0.0001f)
		{
			float num2 = Vector3.Dot(this.rigidbodyInstance.velocity, hitDir);
			float num3 = hitSpeed - num2;
			if (num3 > 0f)
			{
				num = num3;
				vector = num * hitDir;
			}
		}
		Vector3 normalized = (hitPoint - base.transform.position).normalized;
		float num4 = Vector3.Dot(this.rigidbodyInstance.velocity, -normalized);
		if (num4 < 0f)
		{
			float d = Mathf.Lerp(this.reflectOffHandAmountOutputMinMax.x, this.reflectOffHandAmountOutputMinMax.y, Mathf.InverseLerp(this.reflectOffHandSpeedInputMinMax.x, this.reflectOffHandSpeedInputMinMax.y, num4));
			this.rigidbodyInstance.velocity = d * Vector3.Reflect(this.rigidbodyInstance.velocity, -normalized);
		}
		if (num > this.hitSpeedThreshold)
		{
			result = true;
			float num5 = this.hitMultiplierCurve.Evaluate(Mathf.InverseLerp(this.hitSpeedToHitMultiplierMinMax.x, this.hitSpeedToHitMultiplierMinMax.y, num));
			if (this.onGround)
			{
				if (Vector3.Dot(vector, this.groundContact.normal) < 0f)
				{
					vector = Vector3.Reflect(vector, this.groundContact.normal);
				}
				Vector3 vector2 = vector / num;
				if (Vector3.Dot(vector2, this.groundContact.normal) < 0.707f)
				{
					vector = num * (vector2 + this.groundContact.normal) * 0.5f;
				}
			}
			this.rigidbodyInstance.AddForce(Vector3.ClampMagnitude(vector * num5, this.maxHitSpeed), ForceMode.VelocityChange);
			Vector3 rhs = hitDir * hitSpeed - Vector3.Dot(hitDir * hitSpeed, normalized) * normalized;
			Vector3 normalized2 = Vector3.Cross(normalized, rhs).normalized;
			float num6 = Vector3.Dot(this.rigidbodyInstance.angularVelocity, normalized2);
			float num7 = rhs.magnitude / this.ballRadius - num6;
			if (num7 > 0f)
			{
				this.rigidbodyInstance.AddTorque(num5 * this.hitTorqueMultiplier * num7 * normalized2, ForceMode.VelocityChange);
			}
		}
		this.PlayHitSound(num * this.handHitAudioMultiplier);
		return result;
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x000CFD9C File Offset: 0x000CDF9C
	private void PlayHitSound(float hitSpeed)
	{
		float t = Mathf.InverseLerp(this.hitSpeedToAudioMinMax.x, this.hitSpeedToAudioMinMax.y, hitSpeed);
		float value = Mathf.Lerp(this.hitSoundVolumeMinMax.x, this.hitSoundVolumeMinMax.y, t);
		float value2 = Mathf.Lerp(this.hitSoundPitchMinMax.x, this.hitSoundPitchMinMax.y, t);
		if (this.hitSoundBank != null && this.hitSoundSpamCount < this.hitSoundSpamLimit)
		{
			this.hitSoundSpamLastHitTime = Time.time;
			this.hitSoundSpamCount++;
			this.hitSoundBank.Play(new float?(value), new float?(value2));
		}
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x000410C9 File Offset: 0x0003F2C9
	private void FixedUpdate()
	{
		this.collisionContactsCount = 0;
		this.onGround = false;
		this.rigidbodyInstance.AddForce(-Physics.gravity * this.gravityCounterAmount, ForceMode.Acceleration);
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x000CFE4C File Offset: 0x000CE04C
	private void OnTriggerEnter(Collider other)
	{
		GorillaHandClimber component = other.GetComponent<GorillaHandClimber>();
		if (!(component != null))
		{
			if (other.CompareTag(this.gorillaHeadTriggerTag))
			{
				this.playerHeadCollider = (other as SphereCollider);
			}
			return;
		}
		int num;
		if (this.handClimberMap.TryGetValue(component, out num))
		{
			this.handClimberMap[component] = Mathf.Min(num + 1, 2);
			return;
		}
		this.handClimberMap.Add(component, 1);
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x000CFEB8 File Offset: 0x000CE0B8
	private void OnTriggerExit(Collider other)
	{
		GorillaHandClimber component = other.GetComponent<GorillaHandClimber>();
		if (component != null)
		{
			int num;
			if (this.handClimberMap.TryGetValue(component, out num))
			{
				this.handClimberMap[component] = Mathf.Max(num - 1, 0);
				return;
			}
		}
		else if (other.CompareTag(this.gorillaHeadTriggerTag))
		{
			this.playerHeadCollider = null;
		}
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x000CFF10 File Offset: 0x000CE110
	private void OnCollisionEnter(Collision collision)
	{
		this.PlayHitSound(collision.relativeVelocity.magnitude);
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x000CFF34 File Offset: 0x000CE134
	private void OnCollisionStay(Collision collision)
	{
		this.collisionContactsCount = collision.GetContacts(this.collisionContacts);
		float num = -1f;
		for (int i = 0; i < this.collisionContactsCount; i++)
		{
			float num2 = Vector3.Dot(this.collisionContacts[i].normal, Vector3.up);
			if (num2 > num)
			{
				this.groundContact = this.collisionContacts[i];
				num = num2;
			}
		}
		float num3 = 0.5f;
		this.onGround = (num > num3);
	}

	// Token: 0x04001BE6 RID: 7142
	[Header("Transferrable Ball")]
	public float ballRadius = 0.1f;

	// Token: 0x04001BE7 RID: 7143
	public float depenetrationSpeed = 5f;

	// Token: 0x04001BE8 RID: 7144
	[Range(0f, 1f)]
	public float hitSpeedThreshold = 0.8f;

	// Token: 0x04001BE9 RID: 7145
	public float maxHitSpeed = 10f;

	// Token: 0x04001BEA RID: 7146
	public Vector2 hitSpeedToHitMultiplierMinMax = Vector2.one;

	// Token: 0x04001BEB RID: 7147
	public AnimationCurve hitMultiplierCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04001BEC RID: 7148
	public float hitTorqueMultiplier = 0.5f;

	// Token: 0x04001BED RID: 7149
	public float reflectOffHandAmount = 0.5f;

	// Token: 0x04001BEE RID: 7150
	public float minHitSpeedThreshold = 0.2f;

	// Token: 0x04001BEF RID: 7151
	public float surfaceGripDistance = 0.02f;

	// Token: 0x04001BF0 RID: 7152
	public Vector2 reflectOffHandSpeedInputMinMax = Vector2.one;

	// Token: 0x04001BF1 RID: 7153
	public Vector2 reflectOffHandAmountOutputMinMax = Vector2.one;

	// Token: 0x04001BF2 RID: 7154
	public SoundBankPlayer hitSoundBank;

	// Token: 0x04001BF3 RID: 7155
	public Vector2 hitSpeedToAudioMinMax = Vector2.one;

	// Token: 0x04001BF4 RID: 7156
	public float handHitAudioMultiplier = 2f;

	// Token: 0x04001BF5 RID: 7157
	public Vector2 hitSoundPitchMinMax = Vector2.one;

	// Token: 0x04001BF6 RID: 7158
	public Vector2 hitSoundVolumeMinMax = Vector2.one;

	// Token: 0x04001BF7 RID: 7159
	public bool allowHeadButting = true;

	// Token: 0x04001BF8 RID: 7160
	public float headButtRadius = 0.1f;

	// Token: 0x04001BF9 RID: 7161
	public float headButtHitMultiplier = 1.5f;

	// Token: 0x04001BFA RID: 7162
	public float gravityCounterAmount;

	// Token: 0x04001BFB RID: 7163
	public bool debugDraw;

	// Token: 0x04001BFC RID: 7164
	private Dictionary<GorillaHandClimber, int> handClimberMap = new Dictionary<GorillaHandClimber, int>();

	// Token: 0x04001BFD RID: 7165
	private SphereCollider playerHeadCollider;

	// Token: 0x04001BFE RID: 7166
	private ContactPoint[] collisionContacts = new ContactPoint[8];

	// Token: 0x04001BFF RID: 7167
	private int collisionContactsCount;

	// Token: 0x04001C00 RID: 7168
	private float handRadius = 0.1f;

	// Token: 0x04001C01 RID: 7169
	private float depenetrationBias = 1f;

	// Token: 0x04001C02 RID: 7170
	private bool leftHandOverlapping;

	// Token: 0x04001C03 RID: 7171
	private bool rightHandOverlapping;

	// Token: 0x04001C04 RID: 7172
	private bool headOverlapping;

	// Token: 0x04001C05 RID: 7173
	private bool onGround;

	// Token: 0x04001C06 RID: 7174
	private ContactPoint groundContact;

	// Token: 0x04001C07 RID: 7175
	private bool applyFrictionHolding;

	// Token: 0x04001C08 RID: 7176
	private Vector3 frictionHoldLocalPosLeft;

	// Token: 0x04001C09 RID: 7177
	private Quaternion frictionHoldLocalRotLeft;

	// Token: 0x04001C0A RID: 7178
	private Vector3 frictionHoldLocalPosRight;

	// Token: 0x04001C0B RID: 7179
	private Quaternion frictionHoldLocalRotRight;

	// Token: 0x04001C0C RID: 7180
	private float hitSoundSpamLastHitTime;

	// Token: 0x04001C0D RID: 7181
	private int hitSoundSpamCount;

	// Token: 0x04001C0E RID: 7182
	private int hitSoundSpamLimit = 5;

	// Token: 0x04001C0F RID: 7183
	private float hitSoundSpamCooldownResetTime = 0.2f;

	// Token: 0x04001C10 RID: 7184
	private string gorillaHeadTriggerTag = "PlayerHeadTrigger";
}
