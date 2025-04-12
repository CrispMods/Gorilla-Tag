using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class MenagerieDepositBox : MonoBehaviour
{
	// Token: 0x060002E4 RID: 740 RVA: 0x000743C4 File Offset: 0x000725C4
	public void OnTriggerEnter(Collider other)
	{
		MenagerieCritter component = other.transform.parent.parent.GetComponent<MenagerieCritter>();
		if (component.IsNotNull())
		{
			MenagerieCritter menagerieCritter = component;
			menagerieCritter.OnReleased = (Action<MenagerieCritter>)Delegate.Combine(menagerieCritter.OnReleased, this.OnCritterInserted);
		}
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0007440C File Offset: 0x0007260C
	public void OnTriggerExit(Collider other)
	{
		MenagerieCritter component = other.transform.parent.GetComponent<MenagerieCritter>();
		if (component.IsNotNull())
		{
			MenagerieCritter menagerieCritter = component;
			menagerieCritter.OnReleased = (Action<MenagerieCritter>)Delegate.Remove(menagerieCritter.OnReleased, this.OnCritterInserted);
		}
	}

	// Token: 0x0400037E RID: 894
	public Action<MenagerieCritter> OnCritterInserted;
}
