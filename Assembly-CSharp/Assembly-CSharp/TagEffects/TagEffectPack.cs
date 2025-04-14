using System;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B30 RID: 2864
	[CreateAssetMenu(fileName = "New Tag Effect Pack", menuName = "Tag Effect Pack")]
	public class TagEffectPack : ScriptableObject
	{
		// Token: 0x040048FA RID: 18682
		public GameObject thirdPerson;

		// Token: 0x040048FB RID: 18683
		public bool thirdPersonParentEffect = true;

		// Token: 0x040048FC RID: 18684
		public GameObject firstPerson;

		// Token: 0x040048FD RID: 18685
		public bool firstPersonParentEffect = true;

		// Token: 0x040048FE RID: 18686
		public GameObject highFive;

		// Token: 0x040048FF RID: 18687
		public bool highFiveParentEffect;

		// Token: 0x04004900 RID: 18688
		public GameObject fistBump;

		// Token: 0x04004901 RID: 18689
		public bool fistBumpParentEffect;

		// Token: 0x04004902 RID: 18690
		public bool shouldFaceTagger;
	}
}
