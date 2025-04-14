using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaTag.Audio;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BC1 RID: 3009
	public class FireManager : ITickSystemPost
	{
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06004C00 RID: 19456 RVA: 0x001719E5 File Offset: 0x0016FBE5
		// (set) Token: 0x06004C01 RID: 19457 RVA: 0x001719EC File Offset: 0x0016FBEC
		[OnEnterPlay_SetNull]
		internal static FireManager instance { get; private set; }

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06004C02 RID: 19458 RVA: 0x001719F4 File Offset: 0x0016FBF4
		// (set) Token: 0x06004C03 RID: 19459 RVA: 0x001719FB File Offset: 0x0016FBFB
		[OnEnterPlay_Set(false)]
		internal static bool hasInstance { get; private set; }

		// Token: 0x06004C04 RID: 19460 RVA: 0x00171A03 File Offset: 0x0016FC03
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

		// Token: 0x06004C05 RID: 19461 RVA: 0x00171A30 File Offset: 0x0016FC30
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

		// Token: 0x06004C06 RID: 19462 RVA: 0x00171C2C File Offset: 0x0016FE2C
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

		// Token: 0x06004C07 RID: 19463 RVA: 0x00171C5C File Offset: 0x0016FE5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector3Int GetSpatialGridPos(Vector3 pos)
		{
			Vector3 vector = pos / 0.2f;
			return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x00171C90 File Offset: 0x0016FE90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void ResetFireValues(FireInstance f)
		{
			f._timeSinceExtinguished = Mathf.Min(f._timeSinceExtinguished, f._stayExtinguishedDuration);
			f._timeSinceDyingStart = 0f;
			f._isDespawning = false;
			f._timeAlive = 0f;
			f._thermalVolume.celsius = f._defaultTemperature;
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x00171CE4 File Offset: 0x0016FEE4
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

		// Token: 0x06004C0A RID: 19466 RVA: 0x00171D54 File Offset: 0x0016FF54
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

		// Token: 0x06004C0B RID: 19467 RVA: 0x00171E2C File Offset: 0x0017002C
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

		// Token: 0x06004C0C RID: 19468 RVA: 0x00171E80 File Offset: 0x00170080
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

		// Token: 0x06004C0D RID: 19469 RVA: 0x00171EB0 File Offset: 0x001700B0
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

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06004C0E RID: 19470 RVA: 0x00171F63 File Offset: 0x00170163
		// (set) Token: 0x06004C0F RID: 19471 RVA: 0x00171F6B File Offset: 0x0017016B
		bool ITickSystemPost.PostTickRunning { get; set; }

		// Token: 0x06004C10 RID: 19472 RVA: 0x00171F74 File Offset: 0x00170174
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

		// Token: 0x04004DD0 RID: 19920
		[OnEnterPlay_Clear]
		private static readonly Dictionary<int, FireInstance> _kGObjInstId_to_fire = new Dictionary<int, FireInstance>(256);

		// Token: 0x04004DD1 RID: 19921
		[OnEnterPlay_Clear]
		private static readonly List<FireInstance> _kEnabledReactions = new List<FireInstance>(256);

		// Token: 0x04004DD2 RID: 19922
		[OnEnterPlay_Clear]
		private static readonly List<FireInstance> _kFiresToDespawn = new List<FireInstance>(256);

		// Token: 0x04004DD3 RID: 19923
		[OnEnterPlay_Clear]
		private static readonly Dictionary<Vector3Int, int> _fireSpatialGrid = new Dictionary<Vector3Int, int>(256);

		// Token: 0x04004DD4 RID: 19924
		private const float _kSpatialGridCellSize = 0.2f;

		// Token: 0x04004DD5 RID: 19925
		private const int _kMaxAudioSources = 8;

		// Token: 0x04004DD6 RID: 19926
		[OnEnterPlay_Set(0)]
		private static int _activeAudioSources;

		// Token: 0x04004DD7 RID: 19927
		private static readonly int shaderProp_EmissionColor = Shader.PropertyToID("_EmissionColor");
	}
}
