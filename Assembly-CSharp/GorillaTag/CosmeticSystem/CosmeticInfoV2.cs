using System;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000C1E RID: 3102
	[Serializable]
	public struct CosmeticInfoV2 : ISerializationCallbackReceiver
	{
		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06004E04 RID: 19972 RVA: 0x001AD854 File Offset: 0x001ABA54
		public bool hasHoldableParts
		{
			get
			{
				CosmeticPart[] array = this.holdableParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06004E05 RID: 19973 RVA: 0x001AD874 File Offset: 0x001ABA74
		public bool hasWardrobeParts
		{
			get
			{
				CosmeticPart[] array = this.wardrobeParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06004E06 RID: 19974 RVA: 0x001AD894 File Offset: 0x001ABA94
		public bool hasStoreParts
		{
			get
			{
				CosmeticPart[] array = this.storeParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06004E07 RID: 19975 RVA: 0x001AD8B4 File Offset: 0x001ABAB4
		public bool hasFunctionalParts
		{
			get
			{
				CosmeticPart[] array = this.functionalParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06004E08 RID: 19976 RVA: 0x001AD8D4 File Offset: 0x001ABAD4
		public bool hasFirstPersonViewParts
		{
			get
			{
				CosmeticPart[] array = this.firstPersonViewParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x001AD8F4 File Offset: 0x001ABAF4
		public CosmeticInfoV2(string displayName)
		{
			this.enabled = true;
			this.season = null;
			this.displayName = displayName;
			this.playFabID = "";
			this.category = CosmeticsController.CosmeticCategory.None;
			this.icon = null;
			this.isHoldable = false;
			this.isThrowable = false;
			this.usesBothHandSlots = false;
			this.hideWardrobeMannequin = false;
			this.holdableParts = new CosmeticPart[0];
			this.functionalParts = new CosmeticPart[0];
			this.wardrobeParts = new CosmeticPart[0];
			this.storeParts = new CosmeticPart[0];
			this.firstPersonViewParts = new CosmeticPart[0];
			this.setCosmetics = new CosmeticSO[0];
			this.anchorAntiIntersectOffsets = default(CosmeticAnchorAntiIntersectOffsets);
			this.debugCosmeticSOName = "__UNINITIALIZED__";
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x00030607 File Offset: 0x0002E807
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x001AD9B0 File Offset: 0x001ABBB0
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this._OnAfterDeserialize_InitializePartsArray(ref this.holdableParts, ECosmeticPartType.Holdable);
			this._OnAfterDeserialize_InitializePartsArray(ref this.functionalParts, ECosmeticPartType.Functional);
			this._OnAfterDeserialize_InitializePartsArray(ref this.wardrobeParts, ECosmeticPartType.Wardrobe);
			this._OnAfterDeserialize_InitializePartsArray(ref this.storeParts, ECosmeticPartType.Store);
			this._OnAfterDeserialize_InitializePartsArray(ref this.firstPersonViewParts, ECosmeticPartType.FirstPerson);
			if (this.setCosmetics == null)
			{
				this.setCosmetics = Array.Empty<CosmeticSO>();
			}
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x001ADA14 File Offset: 0x001ABC14
		private void _OnAfterDeserialize_InitializePartsArray(ref CosmeticPart[] parts, ECosmeticPartType partType)
		{
			for (int i = 0; i < parts.Length; i++)
			{
				parts[i].partType = partType;
				ref CosmeticAttachInfo[] ptr = ref parts[i].attachAnchors;
				if (ptr == null)
				{
					ptr = Array.Empty<CosmeticAttachInfo>();
				}
			}
		}

		// Token: 0x04004F86 RID: 20358
		public bool enabled;

		// Token: 0x04004F87 RID: 20359
		[Tooltip("// TODO: (2024-09-27 MattO) season will determine what addressables bundle it will be in and wheter it should be active based on release time of season.\n\nThe assigned season will determine what folder the Cosmetic will go in and how it will be listed in the Cosmetic Browser.")]
		[Delayed]
		public SeasonSO season;

		// Token: 0x04004F88 RID: 20360
		[Tooltip("Name that is displayed in the store during purchasing.")]
		[Delayed]
		public string displayName;

		// Token: 0x04004F89 RID: 20361
		[Tooltip("ID used on the PlayFab servers that must be unique. If this does not exist on the playfab servers then an error will be thrown. In notion search for \"Cosmetics - Adding a PlayFab ID\".")]
		[Delayed]
		public string playFabID;

		// Token: 0x04004F8A RID: 20362
		public Sprite icon;

		// Token: 0x04004F8B RID: 20363
		[Tooltip("Category determines which category button in the user's wardrobe (which are the two rows of buttons with equivalent names) have to be pressed to access the cosmetic along with others in the same category.")]
		public StringEnum<CosmeticsController.CosmeticCategory> category;

		// Token: 0x04004F8C RID: 20364
		[Obsolete("(2024-08-13 MattO) Will be removed after holdables array is fully implemented. Check length of `holdableParts` instead.")]
		[HideInInspector]
		public bool isHoldable;

		// Token: 0x04004F8D RID: 20365
		public bool isThrowable;

		// Token: 0x04004F8E RID: 20366
		[Obsolete("(2024-08-13 MattO) Will be removed after holdables array is fully implemented.")]
		[HideInInspector]
		public bool usesBothHandSlots;

		// Token: 0x04004F8F RID: 20367
		public bool hideWardrobeMannequin;

		// Token: 0x04004F90 RID: 20368
		public const string holdableParts_infoBoxShortMsg = "\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).";

		// Token: 0x04004F91 RID: 20369
		public const string holdableParts_infoBoxDetailedMsg = "\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).\n\nHoldables are prefabs that have Holdable components which are parented to slots in \"Gorilla Player Networked.prefab\". Which slots can be used by the \n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004F92 RID: 20370
		[Space]
		[Tooltip("\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).\n\nHoldables are prefabs that have Holdable components which are parented to slots in \"Gorilla Player Networked.prefab\". Which slots can be used by the \n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] holdableParts;

		// Token: 0x04004F93 RID: 20371
		public const string functionalParts_infoBoxShortMsg = "\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.";

		// Token: 0x04004F94 RID: 20372
		public const string functionalParts_infoBoxDetailedMsg = "\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.\n\nThese individual parts which also handle the core functionality of the cosmetic. In most cases there will only be one part, there can be multiple parts for cases like rings which might be on both left and right hands.\n\nThese parts will be parented to the bones of  \"Gorilla Player Networked.prefab\" instances which includes the VRRig component.\n\nAny parts attached to the head (like hats) are hidden from local camera view. To make it visible from first person: create a first person version of the prefab and assign it to the \"First Person\" array below.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004F95 RID: 20373
		[Space]
		[Tooltip("\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.\n\nThese individual parts which also handle the core functionality of the cosmetic. In most cases there will only be one part, there can be multiple parts for cases like rings which might be on both left and right hands.\n\nThese parts will be parented to the bones of  \"Gorilla Player Networked.prefab\" instances which includes the VRRig component.\n\nAny parts attached to the head (like hats) are hidden from local camera view. To make it visible from first person: create a first person version of the prefab and assign it to the \"First Person\" array below.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] functionalParts;

		// Token: 0x04004F96 RID: 20374
		public const string wardrobeParts_infoBoxShortMsg = "\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.";

		// Token: 0x04004F97 RID: 20375
		public const string wardrobeParts_infoBoxDetailedMsg = "\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004F98 RID: 20376
		[Space]
		[Tooltip("\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] wardrobeParts;

		// Token: 0x04004F99 RID: 20377
		[Space]
		[Tooltip("TODO")]
		public CosmeticPart[] storeParts;

		// Token: 0x04004F9A RID: 20378
		public const string firstPersonViewParts_infoBoxShortMsg = "\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.";

		// Token: 0x04004F9B RID: 20379
		public const string firstPersonViewParts_infoBoxDetailedMsg = "\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004F9C RID: 20380
		[Space]
		[Tooltip("\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] firstPersonViewParts;

		// Token: 0x04004F9D RID: 20381
		[Space]
		[Tooltip("TODO COMMENT")]
		public CosmeticAnchorAntiIntersectOffsets anchorAntiIntersectOffsets;

		// Token: 0x04004F9E RID: 20382
		[Space]
		[Tooltip("TODO COMMENT")]
		public CosmeticSO[] setCosmetics;

		// Token: 0x04004F9F RID: 20383
		[NonSerialized]
		public string debugCosmeticSOName;
	}
}
