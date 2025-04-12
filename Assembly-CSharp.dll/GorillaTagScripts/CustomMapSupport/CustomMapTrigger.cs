using System;
using GT_CustomMapSupportRuntime;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009F0 RID: 2544
	public class CustomMapTrigger : MonoBehaviour
	{
		// Token: 0x06003F8A RID: 16266 RVA: 0x00058B4D File Offset: 0x00056D4D
		public byte GetID()
		{
			return this.id;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0016735C File Offset: 0x0016555C
		public virtual void CopyTriggerSettings(TriggerSettings settings)
		{
			this.id = settings.triggerId;
			this.triggeredBy = settings.triggeredBy;
			float num = Math.Max(settings.validationDistance, 2f);
			this.validationDistanceSquared = num * num;
			if (this.triggeredBy == TriggerSource.None)
			{
				if (settings.triggeredByHead && !settings.triggeredByBody)
				{
					this.triggeredBy = TriggerSource.Head;
				}
				else if (settings.triggeredByBody && !settings.triggeredByHead)
				{
					this.triggeredBy = TriggerSource.Body;
				}
				else if (settings.triggeredByHands && !settings.triggeredByHead && !settings.triggeredByBody)
				{
					this.triggeredBy = TriggerSource.Hands;
				}
				else
				{
					this.triggeredBy = TriggerSource.HeadOrBody;
				}
			}
			TriggerSource triggerSource = this.triggeredBy;
			if (triggerSource != TriggerSource.Hands)
			{
				if (triggerSource - TriggerSource.Head <= 2)
				{
					base.gameObject.layer = UnityLayer.GorillaTrigger.ToLayerIndex();
				}
			}
			else
			{
				base.gameObject.layer = UnityLayer.GorillaInteractable.ToLayerIndex();
			}
			this.generalRetriggerDelay = settings.generalRetriggerDelay;
			this.retriggerAfterDuration = settings.retriggerAfterDuration;
			if (Math.Abs(settings.retriggerDelay - 2f) > 0.001f && Math.Abs(settings.retriggerStayDuration - 2.0) < 0.001)
			{
				settings.retriggerStayDuration = (double)settings.retriggerDelay;
			}
			this.retriggerStayDuration = Math.Max(this.generalRetriggerDelay, settings.retriggerStayDuration);
			if (this.retriggerStayDuration <= 0.0)
			{
				this.retriggerAfterDuration = false;
			}
			this.numAllowedTriggers = settings.numAllowedTriggers;
			this.syncedToAllPlayers = settings.syncedToAllPlayers_private;
			if (this.syncedToAllPlayers)
			{
				CustomMapSerializer.RegisterTrigger(base.gameObject.scene.name, this);
			}
			Collider[] components = base.gameObject.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].isTrigger = true;
			}
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x00058B55 File Offset: 0x00056D55
		private void OnTriggerEnter(Collider triggeringCollider)
		{
			if (this.ValidateCollider(triggeringCollider) && this.CanTrigger())
			{
				this.OnTriggerActivation(triggeringCollider);
			}
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x00167524 File Offset: 0x00165724
		private void OnTriggerStay(Collider other)
		{
			if (!this.retriggerAfterDuration)
			{
				return;
			}
			if (this.ValidateCollider(other))
			{
				if (NetworkSystem.Instance.InRoom)
				{
					if (PhotonNetwork.Time - this.lastTriggerTime > -3.0)
					{
						this.lastTriggerTime = -(4294967.295 - this.lastTriggerTime);
					}
					if (this.lastTriggerTime + this.retriggerStayDuration <= PhotonNetwork.Time)
					{
						this.OnTriggerActivation(other);
						return;
					}
				}
				else if (this.lastTriggerTime + this.retriggerStayDuration <= (double)Time.time)
				{
					this.OnTriggerActivation(other);
				}
			}
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x001675B8 File Offset: 0x001657B8
		private bool ValidateCollider(Collider other)
		{
			GameObject gameObject = other.gameObject;
			bool flag = gameObject == GorillaTagger.Instance.headCollider.gameObject && (this.triggeredBy == TriggerSource.Head || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag2 = gameObject == GorillaTagger.Instance.bodyCollider.gameObject && (this.triggeredBy == TriggerSource.Body || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag3 = (gameObject == GorillaTagger.Instance.leftHandTriggerCollider.gameObject || gameObject == GorillaTagger.Instance.rightHandTriggerCollider.gameObject) && this.triggeredBy == TriggerSource.Hands;
			return flag || flag2 || flag3;
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x00058B6F File Offset: 0x00056D6F
		private void OnTriggerActivation(Collider activatingCollider)
		{
			if (this.syncedToAllPlayers)
			{
				CustomMapSerializer.RequestTrigger(this.id);
				return;
			}
			this.Trigger(-1.0, true, false);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0016766C File Offset: 0x0016586C
		public bool CanTrigger()
		{
			if (this.generalRetriggerDelay <= 0.0)
			{
				return true;
			}
			if (NetworkSystem.Instance.InRoom)
			{
				if (PhotonNetwork.Time - this.lastTriggerTime < -1.0)
				{
					this.lastTriggerTime = -(4294967.295 - this.lastTriggerTime);
				}
				if (this.lastTriggerTime + this.generalRetriggerDelay <= PhotonNetwork.Time)
				{
					return true;
				}
			}
			else if (this.lastTriggerTime + this.generalRetriggerDelay <= (double)Time.time)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x001676F4 File Offset: 0x001658F4
		public virtual void Trigger(double triggerTime = -1.0, bool originatedLocally = false, bool ignoreTriggerCount = false)
		{
			if (!ignoreTriggerCount)
			{
				if (this.numAllowedTriggers > 0 && this.numTimesTriggered >= this.numAllowedTriggers)
				{
					return;
				}
				this.numTimesTriggered += 1;
			}
			if (NetworkSystem.Instance.InRoom)
			{
				if (triggerTime < 0.0)
				{
					triggerTime = PhotonNetwork.Time;
				}
			}
			else if (originatedLocally)
			{
				triggerTime = (double)Time.time;
			}
			this.lastTriggerTime = triggerTime;
			if (this.numAllowedTriggers > 0 && this.numTimesTriggered >= this.numAllowedTriggers)
			{
				Collider[] components = base.gameObject.GetComponents<Collider>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].enabled = false;
				}
			}
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x00167798 File Offset: 0x00165998
		public virtual void Reset()
		{
			this.lastTriggerTime = -1.0;
			this.numTimesTriggered = 0;
			Collider[] components = base.gameObject.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].enabled = true;
			}
			CustomMapSerializer.ResetTrigger(this.id);
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x001677EC File Offset: 0x001659EC
		public void SetTriggerCount(byte value)
		{
			this.numTimesTriggered = Math.Min(value, this.numAllowedTriggers);
			if (this.numTimesTriggered >= this.numAllowedTriggers)
			{
				Collider[] components = base.gameObject.GetComponents<Collider>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].enabled = false;
				}
			}
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x00058B96 File Offset: 0x00056D96
		public void SetLastTriggerTime(double value)
		{
			this.lastTriggerTime = value;
		}

		// Token: 0x0400409A RID: 16538
		public const byte INVALID_TRIGGER_ID = 255;

		// Token: 0x0400409B RID: 16539
		public const double MAX_PHOTON_SERVER_TIME = 4294967.295;

		// Token: 0x0400409C RID: 16540
		public const float MINIMUM_VALIDATION_DISTANCE = 2f;

		// Token: 0x0400409D RID: 16541
		public bool syncedToAllPlayers;

		// Token: 0x0400409E RID: 16542
		public float validationDistanceSquared;

		// Token: 0x0400409F RID: 16543
		public TriggerSource triggeredBy = TriggerSource.HeadOrBody;

		// Token: 0x040040A0 RID: 16544
		public double generalRetriggerDelay;

		// Token: 0x040040A1 RID: 16545
		public bool retriggerAfterDuration;

		// Token: 0x040040A2 RID: 16546
		public double retriggerStayDuration = 2.0;

		// Token: 0x040040A3 RID: 16547
		public byte numAllowedTriggers;

		// Token: 0x040040A4 RID: 16548
		private byte numTimesTriggered;

		// Token: 0x040040A5 RID: 16549
		private double lastTriggerTime = -1.0;

		// Token: 0x040040A6 RID: 16550
		public byte id = byte.MaxValue;
	}
}
