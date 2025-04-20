using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009E1 RID: 2529
	[CreateAssetMenu(fileName = "GorillaCaveCrystalSetup", menuName = "ScriptableObjects/GorillaCaveCrystalSetup", order = 0)]
	public class GorillaCaveCrystalSetup : ScriptableObject
	{
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06003F10 RID: 16144 RVA: 0x00059184 File Offset: 0x00057384
		public static GorillaCaveCrystalSetup Instance
		{
			get
			{
				return GorillaCaveCrystalSetup.gInstance;
			}
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0005918B File Offset: 0x0005738B
		private void OnEnable()
		{
			if (GorillaCaveCrystalSetup.gInstance == null)
			{
				GorillaCaveCrystalSetup.gInstance = this;
			}
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x00167648 File Offset: 0x00165848
		public GorillaCaveCrystalSetup.CrystalDef[] GetCrystalDefs()
		{
			return (from f in typeof(GorillaCaveCrystalSetup).GetRuntimeFields()
			where f != null && f.FieldType == typeof(GorillaCaveCrystalSetup.CrystalDef)
			select (GorillaCaveCrystalSetup.CrystalDef)f.GetValue(this)).ToArray<GorillaCaveCrystalSetup.CrystalDef>();
		}

		// Token: 0x04004012 RID: 16402
		public Material SharedBase;

		// Token: 0x04004013 RID: 16403
		public Texture2D CrystalAlbedo;

		// Token: 0x04004014 RID: 16404
		public Texture2D CrystalDarkAlbedo;

		// Token: 0x04004015 RID: 16405
		public GorillaCaveCrystalSetup.CrystalDef Red;

		// Token: 0x04004016 RID: 16406
		public GorillaCaveCrystalSetup.CrystalDef Orange;

		// Token: 0x04004017 RID: 16407
		public GorillaCaveCrystalSetup.CrystalDef Yellow;

		// Token: 0x04004018 RID: 16408
		public GorillaCaveCrystalSetup.CrystalDef Green;

		// Token: 0x04004019 RID: 16409
		public GorillaCaveCrystalSetup.CrystalDef Teal;

		// Token: 0x0400401A RID: 16410
		public GorillaCaveCrystalSetup.CrystalDef DarkBlue;

		// Token: 0x0400401B RID: 16411
		public GorillaCaveCrystalSetup.CrystalDef Pink;

		// Token: 0x0400401C RID: 16412
		public GorillaCaveCrystalSetup.CrystalDef Dark;

		// Token: 0x0400401D RID: 16413
		public GorillaCaveCrystalSetup.CrystalDef DarkLight;

		// Token: 0x0400401E RID: 16414
		public GorillaCaveCrystalSetup.CrystalDef DarkLightUnderWater;

		// Token: 0x0400401F RID: 16415
		[SerializeField]
		[TextArea(4, 10)]
		private string _notes;

		// Token: 0x04004020 RID: 16416
		[Space]
		[SerializeField]
		private GameObject _target;

		// Token: 0x04004021 RID: 16417
		private static GorillaCaveCrystalSetup gInstance;

		// Token: 0x04004022 RID: 16418
		private static GorillaCaveCrystalSetup.CrystalDef[] gCrystalDefs;

		// Token: 0x020009E2 RID: 2530
		[Serializable]
		public class CrystalDef
		{
			// Token: 0x04004023 RID: 16419
			public Material keyMaterial;

			// Token: 0x04004024 RID: 16420
			public CrystalVisualsPreset visualPreset;

			// Token: 0x04004025 RID: 16421
			[Space]
			public int low;

			// Token: 0x04004026 RID: 16422
			public int mid;

			// Token: 0x04004027 RID: 16423
			public int high;
		}
	}
}
