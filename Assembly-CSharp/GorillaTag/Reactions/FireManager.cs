using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaTag.Audio;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BEF RID: 3055
	public class FireManager : ITickSystemPost
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06004D4C RID: 19788 RVA: 0x00062BB1 File Offset: 0x00060DB1
		// (set) Token: 0x06004D4D RID: 19789 RVA: 0x00062BB8 File Offset: 0x00060DB8
		[OnEnterPlay_SetNull]
		internal static FireManager instance { get; private set; }

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06004D4E RID: 19790 RVA: 0x00062BC0 File Offset: 0x00060DC0
		// (set) Token: 0x06004D4F RID: 19791 RVA: 0x00062BC7 File Offset: 0x00060DC7
		[OnEnterPlay_Set(false)]
		internal static bool hasInstance { get; private set; }

		// Token: 0x06004D50 RID: 19792 RVA: 0x00062BCF File Offset: 0x00060DCF
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Initialize()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			if (FireManager.hasInstance)
			{
				return;
			}
			FireManager.instance = new FireManager();
			FireManager.hasInstance = true;
			TickSystem<object>.AddPostTickCallback(FireManager.instance);
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x001AA528 File Offset: 0x001A8728
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void Register(FireInstance f)
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			int instanceID = f.gameObject.GetInstanceID();
			if (!FireManager._kGObjInstId_to_fire.TryAdd(instanceID, f))
			{
				if (f == null)
				{
					Debug.LogError("FireManager: You tried to register null!", f);
					return;
				}
				Debug.LogError("FireManager: \"" + f.name + "\" was attempted to be registered more than once!", f);
			}
			f.GetComponentAndSetFieldIfNullElseLogAndDisable(ref f._collider, "_collider", "Collider", "Disabling.", "Register");
			f.GetComponentAndSetFieldIfNullElseLogAndDisable(ref f._thermalVolume, "_thermalVolume", "ThermalSourceVolume", "Disabling.", "Register");
			f.GetComponentAndSetFieldIfNullElseLogAndDisable(ref f._particleSystem, "_particleSystem", "ParticleSystem", "Disabling.", "Register");
			f.GetComponentAndSetFieldIfNullElseLogAndDisable(ref f._loopingAudioSource, "_loopingAudioSource", "AudioSource", "Disabling.", "Register");
			f.DisableIfNull(f._extinguishSound.obj, "_extinguishSound", "AudioClip", "Register");
			f.DisableIfNull(f._igniteSound.obj, "_igniteSound", "AudioClip", "Register");
			f._defaultTemperature = f._thermalVolume.celsius;
			f._timeSinceExtinguished = -f._stayExtinguishedDuration;
			f._psEmissionModule = f._particleSystem.emission;
			f._psDefaultEmissionRate = f._psEmissionModule.rateOverTime.constant;
			f._deathStateDuration = 0f;
			if (f._emissiveRenderers != null)
			{
				f._emiRenderers_matPropBlocks = new MaterialPropertyBlock[f._emissiveRenderers.Length];
				f._emiRenderers_defaultColors = new Color[f._emissiveRenderers.Length];
				for (int i = 0; i < f._emissiveRenderers.Length; i++)
				{
					f._emiRenderers_matPropBlocks[i] = new MaterialPropertyBlock();
					f._emissiveRenderers[i].GetPropertyBlock(f._emiRenderers_matPropBlocks[i]);
					f._emiRenderers_defaultColors[i] = f._emiRenderers_matPropBlocks[i].GetColor(FireManager.shaderProp_EmissionColor);
				}
			}
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x001AA724 File Offset: 0x001A8924
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void Unregister(FireInstance reactable)
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			int instanceID = reactable.gameObject.GetInstanceID();
			FireManager._kGObjInstId_to_fire.Remove(instanceID);
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x001AA754 File Offset: 0x001A8954
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector3Int GetSpatialGridPos(Vector3 pos)
		{
			Vector3 vector = pos / 0.2f;
			return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x001AA788 File Offset: 0x001A8988
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void ResetFireValues(FireInstance f)
		{
			f._timeSinceExtinguished = Mathf.Min(f._timeSinceExtinguished, f._stayExtinguishedDuration);
			f._timeSinceDyingStart = 0f;
			f._isDespawning = false;
			f._timeAlive = 0f;
			f._thermalVolume.celsius = f._defaultTemperature;
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x001AA7DC File Offset: 0x001A89DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void SpawnFire(SinglePool pool, Vector3 pos, Vector3 normal, float scale)
		{
			int key;
			if (FireManager._fireSpatialGrid.TryGetValue(FireManager.GetSpatialGridPos(pos), out key))
			{
				FireManager.ResetFireValues(FireManager._kGObjInstId_to_fire[key]);
				return;
			}
			GameObject gameObject = pool.Instantiate(false);
			gameObject.transform.position = pos;
			gameObject.transform.up = normal;
			gameObject.transform.localScale = Vector3.one * scale;
			gameObject.SetActive(true);
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x001AA84C File Offset: 0x001A8A4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void OnEnable(FireInstance f)
		{
			if (ApplicationQuittingState.IsQuitting || ObjectPools.instance == null || !ObjectPools.instance.initialized)
			{
				return;
			}
			FireManager.ResetFireValues(f);
			f._spatialGridPosition = FireManager.GetSpatialGridPos(f.transform.position);
			FireManager._fireSpatialGrid.Add(f._spatialGridPosition, f.gameObject.GetInstanceID());
			FireManager._kEnabledReactions.Add(f);
			if (GTAudioOneShot.isInitialized && Time.realtimeSinceStartup > 10f)
			{
				GTAudioOneShot.Play(f._igniteSound, f.transform.position, f._igniteSoundVolume, 1f);
			}
			if (8 > FireManager._activeAudioSources)
			{
				FireManager._activeAudioSources++;
				f._loopingAudioSource.enabled = true;
				return;
			}
			f._loopingAudioSource.enabled = false;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x001AA924 File Offset: 0x001A8B24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void OnDisable(FireInstance f)
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			FireManager._kEnabledReactions.Remove(f);
			FireManager._fireSpatialGrid.Remove(f._spatialGridPosition);
			FireManager._activeAudioSources = Mathf.Min(FireManager._activeAudioSources - (f._loopingAudioSource.enabled ? 1 : 0), 0);
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x00062BFB File Offset: 0x00060DFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void OnTriggerEnter(FireInstance f, Collider other)
		{
			if (f._isDespawning || ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			if (other.gameObject.layer == 4)
			{
				FireManager.Extinguish(f.gameObject, float.MaxValue);
			}
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x001AA978 File Offset: 0x001A8B78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void Extinguish(GameObject gObj, float extinguishAmount)
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			FireInstance fireInstance;
			if (!FireManager._kGObjInstId_to_fire.TryGetValue(gObj.GetInstanceID(), out fireInstance))
			{
				return;
			}
			float num = fireInstance._thermalVolume.celsius - extinguishAmount;
			if (num <= 0f && fireInstance._thermalVolume.celsius > 0.001f)
			{
				fireInstance._thermalVolume.celsius = Mathf.Max(num, 0f);
				fireInstance._timeSinceExtinguished = 0f;
				GTAudioOneShot.Play(fireInstance._extinguishSound, fireInstance.transform.position, fireInstance._extinguishSoundVolume, 1f);
				if (fireInstance._despawnOnExtinguish)
				{
					fireInstance._isDespawning = true;
					fireInstance._timeSinceDyingStart = 0f;
				}
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06004D5A RID: 19802 RVA: 0x00062C2B File Offset: 0x00060E2B
		// (set) Token: 0x06004D5B RID: 19803 RVA: 0x00062C33 File Offset: 0x00060E33
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004D5C RID: 19804 RVA: 0x001AAA2C File Offset: 0x001A8C2C
		void ITickSystemPost.PostTick()
		{
			if (ApplicationQuittingState.IsQuitting)
			{
				return;
			}
			foreach (FireInstance fireInstance in FireManager._kEnabledReactions)
			{
				fireInstance._timeAlive += Time.unscaledDeltaTime;
				bool flag = fireInstance._timeSinceExtinguished < fireInstance._stayExtinguishedDuration;
				fireInstance._timeSinceExtinguished += Time.unscaledDeltaTime;
				bool flag2 = fireInstance._timeSinceExtinguished < fireInstance._stayExtinguishedDuration;
				if (fireInstance._isDespawning)
				{
					fireInstance._timeSinceDyingStart += Time.unscaledDeltaTime;
					if (fireInstance._timeSinceDyingStart >= fireInstance._deathStateDuration || fireInstance._thermalVolume.celsius < -9999f)
					{
						FireManager._kFiresToDespawn.Add(fireInstance);
					}
				}
				if (!fireInstance._isDespawning && fireInstance._despawnOnExtinguish && fireInstance._timeAlive > fireInstance._maxLifetime)
				{
					fireInstance._isDespawning = true;
					fireInstance._timeSinceDyingStart = 0f;
					GTAudioOneShot.Play(fireInstance._extinguishSound, fireInstance.transform.position, fireInstance._extinguishSoundVolume, 1f);
				}
				if (!fireInstance._isDespawning && flag != flag2)
				{
					if (flag2)
					{
						if (fireInstance._despawnOnExtinguish)
						{
							fireInstance._isDespawning = true;
							fireInstance._timeSinceDyingStart = 0f;
						}
						GTAudioOneShot.Play(fireInstance._extinguishSound, fireInstance.transform.position, fireInstance._extinguishSoundVolume, 1f);
					}
					else
					{
						GTAudioOneShot.Play(fireInstance._igniteSound, fireInstance.transform.position, fireInstance._igniteSoundVolume, 1f);
					}
				}
				float num = fireInstance._thermalVolume.celsius + fireInstance._reheatSpeed * Time.unscaledDeltaTime;
				if (fireInstance._isDespawning)
				{
					if (fireInstance._deathStateDuration <= 0f)
					{
						num = 0f;
					}
					else
					{
						num = Mathf.Lerp(fireInstance._thermalVolume.celsius, 0f, fireInstance._timeSinceDyingStart / fireInstance._deathStateDuration);
					}
				}
				num = ((num > fireInstance._defaultTemperature) ? fireInstance._defaultTemperature : num);
				fireInstance._thermalVolume.celsius = num;
				float num2 = num / fireInstance._defaultTemperature;
				fireInstance._loopingAudioSource.volume = num2;
				for (int i = 0; i < fireInstance._emissiveRenderers.Length; i++)
				{
					fireInstance._emiRenderers_matPropBlocks[i].SetColor(FireManager.shaderProp_EmissionColor, fireInstance._emiRenderers_defaultColors[i] * num2);
				}
			}
			foreach (FireInstance fireInstance2 in FireManager._kFiresToDespawn)
			{
				ObjectPools.instance.Destroy(fireInstance2.gameObject);
			}
			FireManager._kFiresToDespawn.Clear();
		}

		// Token: 0x04004EC6 RID: 20166
		[OnEnterPlay_Clear]
		private static readonly Dictionary<int, FireInstance> _kGObjInstId_to_fire = new Dictionary<int, FireInstance>(256);

		// Token: 0x04004EC7 RID: 20167
		[OnEnterPlay_Clear]
		private static readonly List<FireInstance> _kEnabledReactions = new List<FireInstance>(256);

		// Token: 0x04004EC8 RID: 20168
		[OnEnterPlay_Clear]
		private static readonly List<FireInstance> _kFiresToDespawn = new List<FireInstance>(256);

		// Token: 0x04004EC9 RID: 20169
		[OnEnterPlay_Clear]
		private static readonly Dictionary<Vector3Int, int> _fireSpatialGrid = new Dictionary<Vector3Int, int>(256);

		// Token: 0x04004ECA RID: 20170
		private const float _kSpatialGridCellSize = 0.2f;

		// Token: 0x04004ECB RID: 20171
		private const int _kMaxAudioSources = 8;

		// Token: 0x04004ECC RID: 20172
		[OnEnterPlay_Set(0)]
		private static int _activeAudioSources;

		// Token: 0x04004ECD RID: 20173
		private static readonly int shaderProp_EmissionColor = Shader.PropertyToID("_EmissionColor");
	}
}
