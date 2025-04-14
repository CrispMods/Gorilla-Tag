using System;
using GT_CustomMapSupportRuntime;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009ED RID: 2541
	public class CustomMapTrigger : MonoBehaviour
	{
		// Token: 0x06003F7E RID: 16254 RVA: 0x0012CCE5 File Offset: 0x0012AEE5
		public byte GetID()
		{
			return this.id;
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x0012CCF0 File Offset: 0x0012AEF0
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

		// Token: 0x06003F80 RID: 16256 RVA: 0x0012CEB5 File Offset: 0x0012B0B5
		private void OnTriggerEnter(Collider triggeringCollider)
		{
			if (this.ValidateCollider(triggeringCollider) && this.CanTrigger())
			{
				this.OnTriggerActivation(triggeringCollider);
			}
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0012CED0 File Offset: 0x0012B0D0
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

		// Token: 0x06003F82 RID: 16258 RVA: 0x0012CF64 File Offset: 0x0012B164
		private bool ValidateCollider(Collider other)
		{
			GameObject gameObject = other.gameObject;
			bool flag = gameObject == GorillaTagger.Instance.headCollider.gameObject && (this.triggeredBy == TriggerSource.Head || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag2 = gameObject == GorillaTagger.Instance.bodyCollider.gameObject && (this.triggeredBy == TriggerSource.Body || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag3 = (gameObject == GorillaTagger.Instance.leftHandTriggerCollider.gameObject || gameObject == GorillaTagger.Instance.rightHandTriggerCollider.gameObject) && this.triggeredBy == TriggerSource.Hands;
			return flag || flag2 || flag3;
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x0012D016 File Offset: 0x0012B216
		private void OnTriggerActivation(Collider activatingCollider)
		{
			if (this.syncedToAllPlayers)
			{
				CustomMapSerializer.RequestTrigger(this.id);
				return;
			}
			this.Trigger(-1.0, true, false);
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x0012D040 File Offset: 0x0012B240
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

		// Token: 0x06003F85 RID: 16261 RVA: 0x0012D0C8 File Offset: 0x0012B2C8
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

		// Token: 0x06003F86 RID: 16262 RVA: 0x0012D16C File Offset: 0x0012B36C
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

		// Token: 0x06003F87 RID: 16263 RVA: 0x0012D1C0 File Offset: 0x0012B3C0
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

		// Token: 0x06003F88 RID: 16264 RVA: 0x0012D210 File Offset: 0x0012B410
		public void SetLastTriggerTime(double value)
		{
			this.lastTriggerTime = value;
		}

		// Token: 0x04004088 RID: 16520
		public const byte INVALID_TRIGGER_ID = 255;

		// Token: 0x04004089 RID: 16521
		public const double MAX_PHOTON_SERVER_TIME = 4294967.295;

		// Token: 0x0400408A RID: 16522
		public const float MINIMUM_VALIDATION_DISTANCE = 2f;

		// Token: 0x0400408B RID: 16523
		public bool syncedToAllPlayers;

		// Token: 0x0400408C RID: 16524
		public float validationDistanceSquared;

		// Token: 0x0400408D RID: 16525
		public TriggerSource triggeredBy = TriggerSource.HeadOrBody;

		// Token: 0x0400408E RID: 16526
		public double generalRetriggerDelay;

		// Token: 0x0400408F RID: 16527
		public bool retriggerAfterDuration;

		// Token: 0x04004090 RID: 16528
		public double retriggerStayDuration = 2.0;

		// Token: 0x04004091 RID: 16529
		public byte numAllowedTriggers;

		// Token: 0x04004092 RID: 16530
		private byte numTimesTriggered;

		// Token: 0x04004093 RID: 16531
		private double lastTriggerTime = -1.0;

		// Token: 0x04004094 RID: 16532
		public byte id = byte.MaxValue;
	}
}
