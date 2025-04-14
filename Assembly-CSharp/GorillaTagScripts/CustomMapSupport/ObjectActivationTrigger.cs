using System;
using System.Collections.Generic;
using GorillaExtensions;
using GT_CustomMapSupportRuntime;
using UnityEngine;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009EF RID: 2543
	public class ObjectActivationTrigger : CustomMapTrigger
	{
		// Token: 0x06003F8D RID: 16269 RVA: 0x0012D368 File Offset: 0x0012B568
		public override void CopyTriggerSettings(TriggerSettings settings)
		{
			if (settings.GetType() == typeof(ObjectActivationTriggerSettings))
			{
				ObjectActivationTriggerSettings objectActivationTriggerSettings = (ObjectActivationTriggerSettings)settings;
				this.objectsToActivate = objectActivationTriggerSettings.objectsToActivate;
				this.objectsToDeactivate = objectActivationTriggerSettings.objectsToDeactivate;
				this.triggersToReset = objectActivationTriggerSettings.triggersToReset;
			}
			for (int i = this.objectsToActivate.Count - 1; i >= 0; i--)
			{
				if (this.objectsToActivate[i] == null)
				{
					this.objectsToActivate.RemoveAt(i);
				}
			}
			for (int j = this.objectsToDeactivate.Count - 1; j >= 0; j--)
			{
				if (this.objectsToDeactivate[j] == null)
				{
					this.objectsToDeactivate.RemoveAt(j);
				}
			}
			for (int k = this.triggersToReset.Count - 1; k >= 0; k--)
			{
				if (this.triggersToReset[k] == null)
				{
					this.triggersToReset.RemoveAt(k);
				}
			}
			base.CopyTriggerSettings(settings);
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0012D468 File Offset: 0x0012B668
		public override void Trigger(double triggerTime = -1.0, bool originatedLocally = false, bool ignoreTriggerCount = false)
		{
			base.Trigger(triggerTime, originatedLocally, ignoreTriggerCount);
			foreach (GameObject gameObject in this.objectsToDeactivate)
			{
				if (gameObject.IsNotNull())
				{
					gameObject.SetActive(false);
				}
			}
			foreach (GameObject gameObject2 in this.objectsToActivate)
			{
				if (gameObject2.IsNotNull())
				{
					gameObject2.SetActive(true);
				}
			}
			foreach (GameObject gameObject3 in this.triggersToReset)
			{
				if (!gameObject3.IsNull())
				{
					CustomMapTrigger component = gameObject3.GetComponent<CustomMapTrigger>();
					if (component.IsNotNull())
					{
						component.Reset();
					}
				}
			}
		}

		// Token: 0x04004097 RID: 16535
		public List<GameObject> objectsToActivate = new List<GameObject>();

		// Token: 0x04004098 RID: 16536
		public List<GameObject> objectsToDeactivate = new List<GameObject>();

		// Token: 0x04004099 RID: 16537
		public List<GameObject> triggersToReset = new List<GameObject>();
	}
}
