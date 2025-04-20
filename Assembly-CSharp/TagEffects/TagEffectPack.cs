using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B5A RID: 2906
	[CreateAssetMenu(fileName = "New Tag Effect Pack", menuName = "Tag Effect Pack")]
	public class TagEffectPack : ScriptableObject
	{
		// Token: 0x040049DD RID: 18909
		public GameObject thirdPerson;

		// Token: 0x040049DE RID: 18910
		public bool thirdPersonParentEffect = true;

		// Token: 0x040049DF RID: 18911
		public GameObject firstPerson;

		// Token: 0x040049E0 RID: 18912
		public bool firstPersonParentEffect = true;

		// Token: 0x040049E1 RID: 18913
		public GameObject highFive;

		// Token: 0x040049E2 RID: 18914
		public bool highFiveParentEffect;

		// Token: 0x040049E3 RID: 18915
		public GameObject fistBump;

		// Token: 0x040049E4 RID: 18916
		public bool fistBumpParentEffect;

		// Token: 0x040049E5 RID: 18917
		public bool shouldFaceTagger;
	}
}
