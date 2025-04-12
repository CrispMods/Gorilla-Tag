using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009BE RID: 2494
	[CreateAssetMenu(fileName = "GorillaCaveCrystalSetup", menuName = "ScriptableObjects/GorillaCaveCrystalSetup", order = 0)]
	public class GorillaCaveCrystalSetup : ScriptableObject
	{
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x000578ED File Offset: 0x00055AED
		public static GorillaCaveCrystalSetup Instance
		{
			get
			{
				return GorillaCaveCrystalSetup.gInstance;
			}
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x000578F4 File Offset: 0x00055AF4
		private void OnEnable()
		{
			if (GorillaCaveCrystalSetup.gInstance == null)
			{
				GorillaCaveCrystalSetup.gInstance = this;
			}
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x00161624 File Offset: 0x0015F824
		public GorillaCaveCrystalSetup.CrystalDef[] GetCrystalDefs()
		{
			return (from f in typeof(GorillaCaveCrystalSetup).GetRuntimeFields()
			where f != null && f.FieldType == typeof(GorillaCaveCrystalSetup.CrystalDef)
			select (GorillaCaveCrystalSetup.CrystalDef)f.GetValue(this)).ToArray<GorillaCaveCrystalSetup.CrystalDef>();
		}

		// Token: 0x04003F4A RID: 16202
		public Material SharedBase;

		// Token: 0x04003F4B RID: 16203
		public Texture2D CrystalAlbedo;

		// Token: 0x04003F4C RID: 16204
		public Texture2D CrystalDarkAlbedo;

		// Token: 0x04003F4D RID: 16205
		public GorillaCaveCrystalSetup.CrystalDef Red;

		// Token: 0x04003F4E RID: 16206
		public GorillaCaveCrystalSetup.CrystalDef Orange;

		// Token: 0x04003F4F RID: 16207
		public GorillaCaveCrystalSetup.CrystalDef Yellow;

		// Token: 0x04003F50 RID: 16208
		public GorillaCaveCrystalSetup.CrystalDef Green;

		// Token: 0x04003F51 RID: 16209
		public GorillaCaveCrystalSetup.CrystalDef Teal;

		// Token: 0x04003F52 RID: 16210
		public GorillaCaveCrystalSetup.CrystalDef DarkBlue;

		// Token: 0x04003F53 RID: 16211
		public GorillaCaveCrystalSetup.CrystalDef Pink;

		// Token: 0x04003F54 RID: 16212
		public GorillaCaveCrystalSetup.CrystalDef Dark;

		// Token: 0x04003F55 RID: 16213
		public GorillaCaveCrystalSetup.CrystalDef DarkLight;

		// Token: 0x04003F56 RID: 16214
		public GorillaCaveCrystalSetup.CrystalDef DarkLightUnderWater;

		// Token: 0x04003F57 RID: 16215
		[SerializeField]
		[TextArea(4, 10)]
		private string _notes;

		// Token: 0x04003F58 RID: 16216
		[Space]
		[SerializeField]
		private GameObject _target;

		// Token: 0x04003F59 RID: 16217
		private static GorillaCaveCrystalSetup gInstance;

		// Token: 0x04003F5A RID: 16218
		private static GorillaCaveCrystalSetup.CrystalDef[] gCrystalDefs;

		// Token: 0x020009BF RID: 2495
		[Serializable]
		public class CrystalDef
		{
			// Token: 0x04003F5B RID: 16219
			public Material keyMaterial;

			// Token: 0x04003F5C RID: 16220
			public CrystalVisualsPreset visualPreset;

			// Token: 0x04003F5D RID: 16221
			[Space]
			public int low;

			// Token: 0x04003F5E RID: 16222
			public int mid;

			// Token: 0x04003F5F RID: 16223
			public int high;
		}
	}
}
