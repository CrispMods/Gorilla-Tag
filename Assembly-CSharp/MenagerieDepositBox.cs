using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class MenagerieDepositBox : MonoBehaviour
{
	// Token: 0x060002E2 RID: 738 RVA: 0x0001228C File Offset: 0x0001048C
	public void OnTriggerEnter(Collider other)
	{
		MenagerieCritter component = other.transform.parent.parent.GetComponent<MenagerieCritter>();
		if (component.IsNotNull())
		{
			MenagerieCritter menagerieCritter = component;
			menagerieCritter.OnReleased = (Action<MenagerieCritter>)Delegate.Combine(menagerieCritter.OnReleased, this.OnCritterInserted);
		}
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000122D4 File Offset: 0x000104D4
	public void OnTriggerExit(Collider other)
	{
		MenagerieCritter component = other.transform.parent.GetComponent<MenagerieCritter>();
		if (component.IsNotNull())
		{
			MenagerieCritter menagerieCritter = component;
			menagerieCritter.OnReleased = (Action<MenagerieCritter>)Delegate.Remove(menagerieCritter.OnReleased, this.OnCritterInserted);
		}
	}

	// Token: 0x0400037D RID: 893
	public Action<MenagerieCritter> OnCritterInserted;
}
