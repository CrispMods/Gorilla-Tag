using System;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag.CosmeticSystem
{
	// Token: 0x02000BF3 RID: 3059
	[Serializable]
	public struct CosmeticInfoV2 : ISerializationCallbackReceiver
	{
		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x0017584C File Offset: 0x00173A4C
		public bool hasHoldableParts
		{
			get
			{
				CosmeticPart[] array = this.holdableParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x0017586C File Offset: 0x00173A6C
		public bool hasWardrobeParts
		{
			get
			{
				CosmeticPart[] array = this.wardrobeParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06004CC6 RID: 19654 RVA: 0x0017588C File Offset: 0x00173A8C
		public bool hasStoreParts
		{
			get
			{
				CosmeticPart[] array = this.storeParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06004CC7 RID: 19655 RVA: 0x001758AC File Offset: 0x00173AAC
		public bool hasFunctionalParts
		{
			get
			{
				CosmeticPart[] array = this.functionalParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06004CC8 RID: 19656 RVA: 0x001758CC File Offset: 0x00173ACC
		public bool hasFirstPersonViewParts
		{
			get
			{
				CosmeticPart[] array = this.firstPersonViewParts;
				return array != null && array.Length > 0;
			}
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x001758EC File Offset: 0x00173AEC
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

		// Token: 0x06004CCA RID: 19658 RVA: 0x000023F4 File Offset: 0x000005F4
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x001759A8 File Offset: 0x00173BA8
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

		// Token: 0x06004CCC RID: 19660 RVA: 0x00175A0C File Offset: 0x00173C0C
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

		// Token: 0x04004EA2 RID: 20130
		public bool enabled;

		// Token: 0x04004EA3 RID: 20131
		[Tooltip("// TODO: (2024-09-27 MattO) season will determine what addressables bundle it will be in and wheter it should be active based on release time of season.\n\nThe assigned season will determine what folder the Cosmetic will go in and how it will be listed in the Cosmetic Browser.")]
		[Delayed]
		public SeasonSO season;

		// Token: 0x04004EA4 RID: 20132
		[Tooltip("Name that is displayed in the store during purchasing.")]
		[Delayed]
		public string displayName;

		// Token: 0x04004EA5 RID: 20133
		[Tooltip("ID used on the PlayFab servers that must be unique. If this does not exist on the playfab servers then an error will be thrown. In notion search for \"Cosmetics - Adding a PlayFab ID\".")]
		[Delayed]
		public string playFabID;

		// Token: 0x04004EA6 RID: 20134
		public Sprite icon;

		// Token: 0x04004EA7 RID: 20135
		[Tooltip("Category determines which category button in the user's wardrobe (which are the two rows of buttons with equivalent names) have to be pressed to access the cosmetic along with others in the same category.")]
		public StringEnum<CosmeticsController.CosmeticCategory> category;

		// Token: 0x04004EA8 RID: 20136
		[Obsolete("(2024-08-13 MattO) Will be removed after holdables array is fully implemented. Check length of `holdableParts` instead.")]
		[HideInInspector]
		public bool isHoldable;

		// Token: 0x04004EA9 RID: 20137
		public bool isThrowable;

		// Token: 0x04004EAA RID: 20138
		[Obsolete("(2024-08-13 MattO) Will be removed after holdables array is fully implemented.")]
		[HideInInspector]
		public bool usesBothHandSlots;

		// Token: 0x04004EAB RID: 20139
		public bool hideWardrobeMannequin;

		// Token: 0x04004EAC RID: 20140
		public const string holdableParts_infoBoxShortMsg = "\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).";

		// Token: 0x04004EAD RID: 20141
		public const string holdableParts_infoBoxDetailedMsg = "\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).\n\nHoldables are prefabs that have Holdable components which are parented to slots in \"Gorilla Player Networked.prefab\". Which slots can be used by the \n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004EAE RID: 20142
		[Space]
		[Tooltip("\"Holdable Parts\" must have a Holdable component (or inherits like TransferrableObject).\n\nHoldables are prefabs that have Holdable components which are parented to slots in \"Gorilla Player Networked.prefab\". Which slots can be used by the \n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] holdableParts;

		// Token: 0x04004EAF RID: 20143
		public const string functionalParts_infoBoxShortMsg = "\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.";

		// Token: 0x04004EB0 RID: 20144
		public const string functionalParts_infoBoxDetailedMsg = "\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.\n\nThese individual parts which also handle the core functionality of the cosmetic. In most cases there will only be one part, there can be multiple parts for cases like rings which might be on both left and right hands.\n\nThese parts will be parented to the bones of  \"Gorilla Player Networked.prefab\" instances which includes the VRRig component.\n\nAny parts attached to the head (like hats) are hidden from local camera view. To make it visible from first person: create a first person version of the prefab and assign it to the \"First Person\" array below.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004EB1 RID: 20145
		[Space]
		[Tooltip("\"Wearable Parts\" will be attached to \"Gorilla Player Networked.prefab\" instances.\n\nThese individual parts which also handle the core functionality of the cosmetic. In most cases there will only be one part, there can be multiple parts for cases like rings which might be on both left and right hands.\n\nThese parts will be parented to the bones of  \"Gorilla Player Networked.prefab\" instances which includes the VRRig component.\n\nAny parts attached to the head (like hats) are hidden from local camera view. To make it visible from first person: create a first person version of the prefab and assign it to the \"First Person\" array below.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] functionalParts;

		// Token: 0x04004EB2 RID: 20146
		public const string wardrobeParts_infoBoxShortMsg = "\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.";

		// Token: 0x04004EB3 RID: 20147
		public const string wardrobeParts_infoBoxDetailedMsg = "\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004EB4 RID: 20148
		[Space]
		[Tooltip("\"Wardrobe Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] wardrobeParts;

		// Token: 0x04004EB5 RID: 20149
		[Space]
		[Tooltip("TODO")]
		public CosmeticPart[] storeParts;

		// Token: 0x04004EB6 RID: 20150
		public const string firstPersonViewParts_infoBoxShortMsg = "\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.";

		// Token: 0x04004EB7 RID: 20151
		public const string firstPersonViewParts_infoBoxDetailedMsg = "\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)";

		// Token: 0x04004EB8 RID: 20152
		[Space]
		[Tooltip("\"First Person View Parts\" will be attached to \"Head Model.prefab\" instances.\n\nThese parts should be static meshes not skinned and not have any scripts attached. They should only be simple visual representations.\n\n(Last Updated 2024-08-07 MattO)")]
		public CosmeticPart[] firstPersonViewParts;

		// Token: 0x04004EB9 RID: 20153
		[Space]
		[Tooltip("TODO COMMENT")]
		public CosmeticAnchorAntiIntersectOffsets anchorAntiIntersectOffsets;

		// Token: 0x04004EBA RID: 20154
		[Space]
		[Tooltip("TODO COMMENT")]
		public CosmeticSO[] setCosmetics;

		// Token: 0x04004EBB RID: 20155
		[NonSerialized]
		public string debugCosmeticSOName;
	}
}
