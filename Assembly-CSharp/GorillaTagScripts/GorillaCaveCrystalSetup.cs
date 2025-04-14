using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009BB RID: 2491
	[CreateAssetMenu(fileName = "GorillaCaveCrystalSetup", menuName = "ScriptableObjects/GorillaCaveCrystalSetup", order = 0)]
	public class GorillaCaveCrystalSetup : ScriptableObject
	{
		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06003DF8 RID: 15864 RVA: 0x00125D2F File Offset: 0x00123F2F
		public static GorillaCaveCrystalSetup Instance
		{
			get
			{
				return GorillaCaveCrystalSetup.gInstance;
			}
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x00125D36 File Offset: 0x00123F36
		private void OnEnable()
		{
			if (GorillaCaveCrystalSetup.gInstance == null)
			{
				GorillaCaveCrystalSetup.gInstance = this;
			}
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x00125D4C File Offset: 0x00123F4C
		public GorillaCaveCrystalSetup.CrystalDef[] GetCrystalDefs()
		{
			return (from f in typeof(GorillaCaveCrystalSetup).GetRuntimeFields()
			where f != null && f.FieldType == typeof(GorillaCaveCrystalSetup.CrystalDef)
			select (GorillaCaveCrystalSetup.CrystalDef)f.GetValue(this)).ToArray<GorillaCaveCrystalSetup.CrystalDef>();
		}

		// Token: 0x04003F38 RID: 16184
		public Material SharedBase;

		// Token: 0x04003F39 RID: 16185
		public Texture2D CrystalAlbedo;

		// Token: 0x04003F3A RID: 16186
		public Texture2D CrystalDarkAlbedo;

		// Token: 0x04003F3B RID: 16187
		public GorillaCaveCrystalSetup.CrystalDef Red;

		// Token: 0x04003F3C RID: 16188
		public GorillaCaveCrystalSetup.CrystalDef Orange;

		// Token: 0x04003F3D RID: 16189
		public GorillaCaveCrystalSetup.CrystalDef Yellow;

		// Token: 0x04003F3E RID: 16190
		public GorillaCaveCrystalSetup.CrystalDef Green;

		// Token: 0x04003F3F RID: 16191
		public GorillaCaveCrystalSetup.CrystalDef Teal;

		// Token: 0x04003F40 RID: 16192
		public GorillaCaveCrystalSetup.CrystalDef DarkBlue;

		// Token: 0x04003F41 RID: 16193
		public GorillaCaveCrystalSetup.CrystalDef Pink;

		// Token: 0x04003F42 RID: 16194
		public GorillaCaveCrystalSetup.CrystalDef Dark;

		// Token: 0x04003F43 RID: 16195
		public GorillaCaveCrystalSetup.CrystalDef DarkLight;

		// Token: 0x04003F44 RID: 16196
		public GorillaCaveCrystalSetup.CrystalDef DarkLightUnderWater;

		// Token: 0x04003F45 RID: 16197
		[SerializeField]
		[TextArea(4, 10)]
		private string _notes;

		// Token: 0x04003F46 RID: 16198
		[Space]
		[SerializeField]
		private GameObject _target;

		// Token: 0x04003F47 RID: 16199
		private static GorillaCaveCrystalSetup gInstance;

		// Token: 0x04003F48 RID: 16200
		private static GorillaCaveCrystalSetup.CrystalDef[] gCrystalDefs;

		// Token: 0x020009BC RID: 2492
		[Serializable]
		public class CrystalDef
		{
			// Token: 0x04003F49 RID: 16201
			public Material keyMaterial;

			// Token: 0x04003F4A RID: 16202
			public CrystalVisualsPreset visualPreset;

			// Token: 0x04003F4B RID: 16203
			[Space]
			public int low;

			// Token: 0x04003F4C RID: 16204
			public int mid;

			// Token: 0x04003F4D RID: 16205
			public int high;
		}
	}
}
