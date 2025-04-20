using System;
using System.Collections;
using System.Collections.Generic;
using GorillaGameModes;
using GorillaNetworking;
using UnityEngine;

namespace TagEffects
{
	// Token: 0x02000B5B RID: 2907
	public class TagEffectsLibrary : MonoBehaviour
	{
		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x0600489C RID: 18588 RVA: 0x0005F40A File Offset: 0x0005D60A
		public static float FistBumpSpeedThreshold
		{
			get
			{
				return TagEffectsLibrary._instance.fistBumpSpeedThreshold;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x0600489D RID: 18589 RVA: 0x0005F416 File Offset: 0x0005D616
		public static float HighFiveSpeedThreshold
		{
			get
			{
				return TagEffectsLibrary._instance.highFiveSpeedThreshold;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x0600489E RID: 18590 RVA: 0x0005F422 File Offset: 0x0005D622
		public static bool DebugMode
		{
			get
			{
				return TagEffectsLibrary._instance.debugMode;
			}
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x0005F42E File Offset: 0x0005D62E
		private void Awake()
		{
			if (TagEffectsLibrary._instance != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			TagEffectsLibrary._instance = this;
			this.tagEffectsPool = new Dictionary<string, Queue<GameObjectOnDisableDispatcher>>();
			this.tagEffectsComboLookUp = new Dictionary<TagEffectsCombo, TagEffectPack[]>();
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x0018FD44 File Offset: 0x0018DF44
		public static void PlayEffect(Transform target, bool isLeftHand, float rigScale, TagEffectsLibrary.EffectType effectType, TagEffectPack playerCosmeticTagEffectPack, TagEffectPack otherPlayerCosmeticTagEffectPack, Quaternion rotation)
		{
			if (TagEffectsLibrary._instance == null)
			{
				return;
			}
			ModeTagEffect modeTagEffect = null;
			TagEffectPack tagEffectPack = null;
			GameModeType item = (GameMode.ActiveGameMode != null) ? GameMode.ActiveGameMode.GameType() : GameModeType.Casual;
			for (int i = 0; i < TagEffectsLibrary._instance.defaultTagEffects.Length; i++)
			{
				if (TagEffectsLibrary._instance.defaultTagEffects[i] != null && TagEffectsLibrary._instance.defaultTagEffects[i].Modes.Contains(item))
				{
					modeTagEffect = TagEffectsLibrary._instance.defaultTagEffects[i];
					tagEffectPack = modeTagEffect.tagEffect;
					break;
				}
			}
			if (tagEffectPack == null)
			{
				return;
			}
			GameObject firstPerson = tagEffectPack.firstPerson;
			GameObject thirdPerson = tagEffectPack.thirdPerson;
			GameObject fistBump = tagEffectPack.fistBump;
			GameObject highFive = tagEffectPack.highFive;
			bool firstPersonParentEffect = tagEffectPack.firstPersonParentEffect;
			bool thirdPersonParentEffect = tagEffectPack.thirdPersonParentEffect;
			bool flag = tagEffectPack.fistBumpParentEffect;
			bool highFiveParentEffect = tagEffectPack.highFiveParentEffect;
			if (playerCosmeticTagEffectPack != null)
			{
				TagEffectPack tagEffectPack2 = TagEffectsLibrary.comboLookup(playerCosmeticTagEffectPack, otherPlayerCosmeticTagEffectPack);
				if (!modeTagEffect.blockFistBumpOverride && playerCosmeticTagEffectPack.fistBump != null)
				{
					fistBump = tagEffectPack2.fistBump;
					flag = tagEffectPack2.firstPersonParentEffect;
				}
				if (!modeTagEffect.blockHiveFiveOverride && playerCosmeticTagEffectPack.highFive != null)
				{
					highFive = tagEffectPack2.highFive;
					highFiveParentEffect = tagEffectPack2.highFiveParentEffect;
				}
			}
			if (otherPlayerCosmeticTagEffectPack != null)
			{
				if (!modeTagEffect.blockTagOverride && otherPlayerCosmeticTagEffectPack.firstPerson != null)
				{
					firstPerson = otherPlayerCosmeticTagEffectPack.firstPerson;
					firstPersonParentEffect = otherPlayerCosmeticTagEffectPack.firstPersonParentEffect;
				}
				if (!modeTagEffect.blockTagOverride && otherPlayerCosmeticTagEffectPack.thirdPerson != null)
				{
					thirdPerson = otherPlayerCosmeticTagEffectPack.thirdPerson;
					thirdPersonParentEffect = otherPlayerCosmeticTagEffectPack.thirdPersonParentEffect;
				}
			}
			switch (effectType)
			{
			case TagEffectsLibrary.EffectType.FIRST_PERSON:
				TagEffectsLibrary.placeEffects(firstPerson, target, firstPersonParentEffect ? 1f : rigScale, false, firstPersonParentEffect, rotation);
				return;
			case TagEffectsLibrary.EffectType.THIRD_PERSON:
				TagEffectsLibrary.placeEffects(thirdPerson, target, thirdPersonParentEffect ? 1f : rigScale, false, thirdPersonParentEffect, rotation);
				return;
			case TagEffectsLibrary.EffectType.HIGH_FIVE:
				TagEffectsLibrary.placeEffects(highFive, target, highFiveParentEffect ? 1f : rigScale, isLeftHand, highFiveParentEffect, rotation);
				return;
			case TagEffectsLibrary.EffectType.FIST_BUMP:
				TagEffectsLibrary.placeEffects(fistBump, target, flag ? 1f : rigScale, isLeftHand, flag, rotation);
				return;
			default:
				return;
			}
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x0018FF64 File Offset: 0x0018E164
		private static TagEffectPack comboLookup(TagEffectPack playerCosmeticTagEffectPack, TagEffectPack otherPlayerCosmeticTagEffectPack)
		{
			if (otherPlayerCosmeticTagEffectPack == null)
			{
				return playerCosmeticTagEffectPack;
			}
			TagEffectsCombo tagEffectsCombo = new TagEffectsCombo();
			tagEffectsCombo.inputA = playerCosmeticTagEffectPack;
			tagEffectsCombo.inputB = otherPlayerCosmeticTagEffectPack;
			TagEffectPack[] array;
			if (!TagEffectsLibrary._instance.tagEffectsComboLookUp.TryGetValue(tagEffectsCombo, out array))
			{
				return playerCosmeticTagEffectPack;
			}
			int num = 0;
			if (GorillaComputer.instance != null)
			{
				num = GorillaComputer.instance.GetServerTime().Second;
			}
			return array[num % array.Length];
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x0018FFD4 File Offset: 0x0018E1D4
		public static void placeEffects(GameObject prefab, Transform target, float scale, bool flipZAxis, bool parentEffect, Quaternion rotation)
		{
			if (prefab == null)
			{
				return;
			}
			Queue<GameObjectOnDisableDispatcher> queue;
			if (!TagEffectsLibrary._instance.tagEffectsPool.TryGetValue(prefab.name, out queue))
			{
				queue = new Queue<GameObjectOnDisableDispatcher>();
				TagEffectsLibrary._instance.tagEffectsPool.Add(prefab.name, queue);
			}
			if (queue.Count == 0 || (queue.Peek().gameObject.activeInHierarchy && queue.Count < 12))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, target.transform.position, rotation, parentEffect ? target : TagEffectsLibrary._instance.transform);
				gameObject.name = prefab.name;
				gameObject.transform.localScale = (flipZAxis ? new Vector3(scale, scale, -scale) : (Vector3.one * scale));
				GameObjectOnDisableDispatcher gameObjectOnDisableDispatcher;
				if (!gameObject.TryGetComponent<GameObjectOnDisableDispatcher>(out gameObjectOnDisableDispatcher))
				{
					gameObjectOnDisableDispatcher = gameObject.AddComponent<GameObjectOnDisableDispatcher>();
				}
				gameObjectOnDisableDispatcher.OnDisabled += TagEffectsLibrary.NewGameObjectOnDisableDispatcher_OnDisabled;
				gameObject.SetActive(true);
				queue.Enqueue(gameObjectOnDisableDispatcher);
				return;
			}
			GameObjectOnDisableDispatcher recycledGameObject = queue.Dequeue();
			TagEffectsLibrary._instance.StartCoroutine(TagEffectsLibrary._instance.RecycleGameObject(recycledGameObject, target, scale, flipZAxis, parentEffect));
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x0005F465 File Offset: 0x0005D665
		private static void NewGameObjectOnDisableDispatcher_OnDisabled(GameObjectOnDisableDispatcher goodd)
		{
			TagEffectsLibrary._instance.StartCoroutine(TagEffectsLibrary._instance.ReclaimDisabled(goodd.transform));
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x0005F482 File Offset: 0x0005D682
		private IEnumerator RecycleGameObject(GameObjectOnDisableDispatcher recycledGameObject, Transform target, float scale, bool flipZAxis, bool parentEffect)
		{
			if (recycledGameObject.gameObject.activeInHierarchy)
			{
				recycledGameObject.gameObject.SetActive(false);
				recycledGameObject.OnDisabled -= TagEffectsLibrary.NewGameObjectOnDisableDispatcher_OnDisabled;
				yield return null;
			}
			recycledGameObject.transform.position = target.transform.position;
			recycledGameObject.transform.rotation = target.transform.rotation;
			recycledGameObject.transform.localScale = (flipZAxis ? new Vector3(scale, scale, -scale) : (Vector3.one * scale));
			recycledGameObject.transform.parent = (parentEffect ? target : TagEffectsLibrary._instance.transform);
			Queue<GameObjectOnDisableDispatcher> queue;
			if (TagEffectsLibrary._instance.tagEffectsPool.TryGetValue(recycledGameObject.gameObject.name, out queue))
			{
				recycledGameObject.gameObject.SetActive(true);
				queue.Enqueue(recycledGameObject);
			}
			yield break;
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x0005F4AF File Offset: 0x0005D6AF
		private IEnumerator ReclaimDisabled(Transform transform)
		{
			yield return null;
			transform.parent = TagEffectsLibrary._instance.transform;
			yield break;
		}

		// Token: 0x040049E6 RID: 18918
		private const int OBJECT_QUEUE_LIMIT = 12;

		// Token: 0x040049E7 RID: 18919
		[OnEnterPlay_SetNull]
		private static TagEffectsLibrary _instance;

		// Token: 0x040049E8 RID: 18920
		[SerializeField]
		private float fistBumpSpeedThreshold = 1f;

		// Token: 0x040049E9 RID: 18921
		[SerializeField]
		private float highFiveSpeedThreshold = 1f;

		// Token: 0x040049EA RID: 18922
		[SerializeField]
		private ModeTagEffect[] defaultTagEffects;

		// Token: 0x040049EB RID: 18923
		[SerializeField]
		private TagEffectsComboResult[] tagEffectsCombos;

		// Token: 0x040049EC RID: 18924
		[SerializeField]
		private bool debugMode;

		// Token: 0x040049ED RID: 18925
		private Dictionary<string, Queue<GameObjectOnDisableDispatcher>> tagEffectsPool;

		// Token: 0x040049EE RID: 18926
		private Dictionary<TagEffectsCombo, TagEffectPack[]> tagEffectsComboLookUp;

		// Token: 0x02000B5C RID: 2908
		public enum EffectType
		{
			// Token: 0x040049F0 RID: 18928
			FIRST_PERSON,
			// Token: 0x040049F1 RID: 18929
			THIRD_PERSON,
			// Token: 0x040049F2 RID: 18930
			HIGH_FIVE,
			// Token: 0x040049F3 RID: 18931
			FIST_BUMP
		}
	}
}
