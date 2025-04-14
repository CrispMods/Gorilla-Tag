using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B2D RID: 2861
	[CreateAssetMenu(fileName = "New Tag Effect Pack", menuName = "Tag Effect Pack")]
	public class TagEffectPack : ScriptableObject
	{
		// Token: 0x040048E8 RID: 18664
		public GameObject thirdPerson;

		// Token: 0x040048E9 RID: 18665
		public bool thirdPersonParentEffect = true;

		// Token: 0x040048EA RID: 18666
		public GameObject firstPerson;

		// Token: 0x040048EB RID: 18667
		public bool firstPersonParentEffect = true;

		// Token: 0x040048EC RID: 18668
		public GameObject highFive;

		// Token: 0x040048ED RID: 18669
		public bool highFiveParentEffect;

		// Token: 0x040048EE RID: 18670
		public GameObject fistBump;

		// Token: 0x040048EF RID: 18671
		public bool fistBumpParentEffect;

		// Token: 0x040048F0 RID: 18672
		public bool shouldFaceTagger;
	}
}
