using System;
using GT_CustomMapSupportRuntime;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x02000A07 RID: 2567
	public class CMSTrigger : MonoBehaviour
	{
		// Token: 0x06004025 RID: 16421 RVA: 0x00059EE5 File Offset: 0x000580E5
		public void OnEnable()
		{
			if (this.onEnableTriggerDelay > 0.0)
			{
				this.enabledTime = (double)Time.time;
			}
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00059F04 File Offset: 0x00058104
		public byte GetID()
		{
			return this.id;
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x0016BFF0 File Offset: 0x0016A1F0
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
			this.onEnableTriggerDelay = settings.onEnableTriggerDelay;
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
				CMSSerializer.RegisterTrigger(base.gameObject.scene.name, this);
			}
			Collider[] components = base.gameObject.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].isTrigger = true;
			}
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00059F0C File Offset: 0x0005810C
		public void OnTriggerEnter(Collider triggeringCollider)
		{
			if (this.ValidateCollider(triggeringCollider) && this.CanTrigger())
			{
				this.OnTriggerActivation(triggeringCollider);
			}
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x0016C1C4 File Offset: 0x0016A3C4
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

		// Token: 0x0600402A RID: 16426 RVA: 0x0016C258 File Offset: 0x0016A458
		private bool ValidateCollider(Collider other)
		{
			GameObject gameObject = other.gameObject;
			bool flag = gameObject == GorillaTagger.Instance.headCollider.gameObject && (this.triggeredBy == TriggerSource.Head || this.triggeredBy == TriggerSource.HeadOrBody);
			bool flag2;
			if (GorillaTagger.Instance.bodyCollider.enabled)
			{
				flag2 = (gameObject == GorillaTagger.Instance.bodyCollider.gameObject && (this.triggeredBy == TriggerSource.Body || this.triggeredBy == TriggerSource.HeadOrBody));
			}
			else
			{
				flag2 = (gameObject == VRRig.LocalRig.gameObject && (this.triggeredBy == TriggerSource.Body || this.triggeredBy == TriggerSource.HeadOrBody));
			}
			bool flag3 = (gameObject == GorillaTagger.Instance.leftHandTriggerCollider.gameObject || gameObject == GorillaTagger.Instance.rightHandTriggerCollider.gameObject) && this.triggeredBy == TriggerSource.Hands;
			return flag || flag2 || flag3;
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x00059F26 File Offset: 0x00058126
		private void OnTriggerActivation(Collider activatingCollider)
		{
			if (this.syncedToAllPlayers)
			{
				CMSSerializer.RequestTrigger(this.id);
				return;
			}
			this.Trigger(-1.0, true, false);
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x0016C34C File Offset: 0x0016A54C
		public bool CanTrigger()
		{
			if (this.numAllowedTriggers > 0 && this.numTimesTriggered >= this.numAllowedTriggers)
			{
				return false;
			}
			if (this.onEnableTriggerDelay > 0.0 && (double)Time.time - this.enabledTime < this.onEnableTriggerDelay)
			{
				return false;
			}
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

		// Token: 0x0600402D RID: 16429 RVA: 0x0016C418 File Offset: 0x0016A618
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

		// Token: 0x0600402E RID: 16430 RVA: 0x0016C4BC File Offset: 0x0016A6BC
		public void ResetTrigger(bool onlyResetTriggerCount = false)
		{
			if (!onlyResetTriggerCount)
			{
				this.lastTriggerTime = -1.0;
			}
			this.numTimesTriggered = 0;
			Collider[] components = base.gameObject.GetComponents<Collider>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].enabled = true;
			}
			CMSSerializer.ResetTrigger(this.id);
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x0016C510 File Offset: 0x0016A710
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

		// Token: 0x06004030 RID: 16432 RVA: 0x00059F4D File Offset: 0x0005814D
		public void SetLastTriggerTime(double value)
		{
			this.lastTriggerTime = value;
		}

		// Token: 0x04004114 RID: 16660
		public const byte INVALID_TRIGGER_ID = 255;

		// Token: 0x04004115 RID: 16661
		public const double MAX_PHOTON_SERVER_TIME = 4294967.295;

		// Token: 0x04004116 RID: 16662
		public const float MINIMUM_VALIDATION_DISTANCE = 2f;

		// Token: 0x04004117 RID: 16663
		public bool syncedToAllPlayers;

		// Token: 0x04004118 RID: 16664
		public float validationDistanceSquared;

		// Token: 0x04004119 RID: 16665
		public TriggerSource triggeredBy = TriggerSource.HeadOrBody;

		// Token: 0x0400411A RID: 16666
		public double onEnableTriggerDelay;

		// Token: 0x0400411B RID: 16667
		public double generalRetriggerDelay;

		// Token: 0x0400411C RID: 16668
		public bool retriggerAfterDuration;

		// Token: 0x0400411D RID: 16669
		public double retriggerStayDuration = 2.0;

		// Token: 0x0400411E RID: 16670
		public byte numAllowedTriggers;

		// Token: 0x0400411F RID: 16671
		private byte numTimesTriggered;

		// Token: 0x04004120 RID: 16672
		private double lastTriggerTime = -1.0;

		// Token: 0x04004121 RID: 16673
		private double enabledTime = -1.0;

		// Token: 0x04004122 RID: 16674
		public byte id = byte.MaxValue;
	}
}
