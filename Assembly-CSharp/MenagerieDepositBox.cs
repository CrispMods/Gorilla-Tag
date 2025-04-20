using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class MenagerieDepositBox : MonoBehaviour
{
	// Token: 0x06000311 RID: 785 RVA: 0x00076AEC File Offset: 0x00074CEC
	public void OnTriggerEnter(Collider other)
	{
		MenagerieCritter component = other.transform.parent.parent.GetComponent<MenagerieCritter>();
		if (component.IsNotNull())
		{
			MenagerieCritter menagerieCritter = component;
			menagerieCritter.OnReleased = (Action<MenagerieCritter>)Delegate.Combine(menagerieCritter.OnReleased, this.OnCritterInserted);
		}
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00076B34 File Offset: 0x00074D34
	public void OnTriggerExit(Collider other)
	{
		MenagerieCritter component = other.transform.parent.GetComponent<MenagerieCritter>();
		if (component.IsNotNull())
		{
			MenagerieCritter menagerieCritter = component;
			menagerieCritter.OnReleased = (Action<MenagerieCritter>)Delegate.Remove(menagerieCritter.OnReleased, this.OnCritterInserted);
		}
	}

	// Token: 0x040003AF RID: 943
	public Action<MenagerieCritter> OnCritterInserted;
}
