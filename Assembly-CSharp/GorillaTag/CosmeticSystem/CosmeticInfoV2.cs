using System;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF0 RID: 3056
	[Serializable]
	public struct CosmeticInfoV2 : ISerializationCallbackReceiver
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x00175284 File Offset: 0x00173484
		public bool hasHoldableParts
		{
			get
			{
				CosmeticPart[] array = this.holdableParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x001752A4 File Offset: 0x001734A4
		public bool hasWardrobeParts
		{
			get
			{
				CosmeticPart[] array = this.wardrobeParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x001752C4 File Offset: 0x001734C4
		public bool hasStoreParts
		{
			get
			{
				CosmeticPart[] array = this.storeParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x001752E4 File Offset: 0x001734E4
		public bool hasFunctionalParts
		{
			get
			{
				CosmeticPart[] array = this.functionalParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x00175304 File Offset: 0x00173504
		public bool hasFirstPersonViewParts
		{
			get
			{
				CosmeticPart[] array = this.firstPersonViewParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x00175324 File Offset: 0x00173524
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

		// Token: 0x06004CBE RID: 19646 RVA: 0x000023F4 File Offset: 0x000005F4
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x001753E0 File Offset: 0x001735E0
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

		// Token: 0x06004CC0 RID: 19648 RVA: 0x00175444 File Offset: 0x00173644
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

		// Token: 0x04004E90 RID: 20112
		public bool enabled;

		// Token: 0x04004E91 RID: 20113
		[Tooltip("// TODO: (2024-09-27 MattO) season will determine what addressables bundle it will be in and wheter it should be active based on release time of season.\n\nThe assigned season will determine what folder the Cosmetic will go in and how it will be listed in the Cosmetic Browser.")]
		[Delayed]
		public SeasonSO season;

		// Token: 0x04004E92 RID: 20114
		[Tooltip("Name that is displayed in the store during purchasing.")]
		[Delayed]
		public string displayName;

		// Token: 0x04004E93 RID: 20115
		[Tooltip("ID used on the PlayFab servers that must be unique. If this does not exist on the playfab servers then an error will be thrown. In notion search for \"Cosmetics - Adding a PlayFab ID\".")]
		[Delayed]
		public string playFabID;

		// Token: 0x04004E94 RID: 20116
		public Sprite icon;

		// Token: 0x04004E95 RID: 20117
		[Tooltip("Category determines which category button in the user's wardrobe (which are the two rows of buttons with equivalent names) have to be pressed to access the cosmetic along with others in the same category.")]
		public StringEnum<CosmeticsController.CosmeticCategory> category;

		// Token: 0x04004E96 RID: 20118
		[Obsolete("(2024-08-13 MattO) Will be removed after holdables array is fully implemented. Check length of `holdableParts` instead.")]
		[HideInInspector]
		public bool isHoldable;

		// Token: 0x04004E97 RID: 20119
		public bool isThrowable;

		// Token: 0x04004E98 RID: 20120
		[Obsolete("(2024-08-13 MattO) Will be removed after holdables array is fully implemented.")]
		[HideInInspector]
		public bool usesBothHandSlots;

		// Token: 0x04004E99 RID: 20121
		public bool hideWardrobeMannequin;

		// Token: 0x04004E9A RID: 20122
		public const string holdableParts_infoBoxShortMsg = "\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).";

		// Token: 0x04004E9B RID: 20123
		public const string holdableParts_infoBoxDetailedMsg = "\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).\n\nHoldables are prefabs that have Holdable components which are parented to slots in \"Gorilla Player Networked.prefab\". Which slots can be used by the \n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004E9C RID: 20124
		[Space]
		[Tooltip("\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).\n\nHoldables are prefabs that have Holdable components which are parented to slots in \"Gorilla Player Networked.prefab\". Which slots can be used by the \n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] holdableParts;

		// Token: 0x04004E9D RID: 20125
		public const string functionalParts_infoBoxShortMsg = "\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.";

		// Token: 0x04004E9E RID: 20126
		public const string functionalParts_infoBoxDetailedMsg = "\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.\n\nThese individual parts which also handle the core functionality of the cosmetic. In most cases there will only be one part, there can be multiple parts for cases like rings which might be on both left and right hands.\n\nThese parts will be parented to the bones of  \"Gorilla Player Networked.prefab\" instances which includes the VRRig component.\n\nAny parts attached to the head (like hats) are hidden from local camera view. To make it visible from first person: create a first person version of the prefab and assign it to the \"First Person\" array below.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004E9F RID: 20127
		[Space]
		[Tooltip("\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.\n\nThese individual parts which also handle the core functionality of the cosmetic. In most cases there will only be one part, there can be multiple parts for cases like rings which might be on both left and right hands.\n\nThese parts will be parented to the bones of  \"Gorilla Player Networked.prefab\" instances which includes the VRRig component.\n\nAny parts attached to the head (like hats) are hidden from local camera view. To make it visible from first person: create a first person version of the prefab and assign it to the \"First Person\" array below.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] functionalParts;

		// Token: 0x04004EA0 RID: 20128
		public const string wardrobeParts_infoBoxShortMsg = "\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.";

		// Token: 0x04004EA1 RID: 20129
		public const string wardrobeParts_infoBoxDetailedMsg = "\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004EA2 RID: 20130
		[Space]
		[Tooltip("\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] wardrobeParts;

		// Token: 0x04004EA3 RID: 20131
		[Space]
		[Tooltip("TODO")]
		public CosmeticPart[] storeParts;

		// Token: 0x04004EA4 RID: 20132
		public const string firstPersonViewParts_infoBoxShortMsg = "\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.";

		// Token: 0x04004EA5 RID: 20133
		public const string firstPersonViewParts_infoBoxDetailedMsg = "\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004EA6 RID: 20134
		[Space]
		[Tooltip("\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] firstPersonViewParts;

		// Token: 0x04004EA7 RID: 20135
		[Space]
		[Tooltip("TODO COMMENT")]
		public CosmeticAnchorAntiIntersectOffsets anchorAntiIntersectOffsets;

		// Token: 0x04004EA8 RID: 20136
		[Space]
		[Tooltip("TODO COMMENT")]
		public CosmeticSO[] setCosmetics;

		// Token: 0x04004EA9 RID: 20137
		[NonSerialized]
		public string debugCosmeticSOName;
	}
}
