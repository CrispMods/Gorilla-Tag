using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BC3 RID: 3011
	public class FireInstance : MonoBehaviour
	{
		// Token: 0x06004C06 RID: 19462 RVA: 0x000611C7 File Offset: 0x0005F3C7
		protected void Awake()
		{
			FireManager.Register(this);
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x000611CF File Offset: 0x0005F3CF
		protected void OnDestroy()
		{
			FireManager.Unregister(this);
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x000611D7 File Offset: 0x0005F3D7
		protected void OnEnable()
		{
			FireManager.OnEnable(this);
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x000611DF File Offset: 0x0005F3DF
		protected void OnDisable()
		{
			FireManager.OnDisable(this);
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x000611E7 File Offset: 0x0005F3E7
		protected void OnTriggerEnter(Collider other)
		{
			FireManager.OnTriggerEnter(this, other);
		}

		// Token: 0x04004DC8 RID: 19912
		[Header("Scene References")]
		[Tooltip("If not assigned it will try to auto assign to a component on the same GameObject.")]
		[SerializeField]
		internal Collider _collider;

		// Token: 0x04004DC9 RID: 19913
		[Tooltip("If not assigned it will try to auto assign to a component on the same GameObject.")]
		[FormerlySerializedAs("_thermalSourceVolume")]
		[SerializeField]
		internal ThermalSourceVolume _thermalVolume;

		// Token: 0x04004DCA RID: 19914
		[SerializeField]
		internal ParticleSystem _particleSystem;

		// Token: 0x04004DCB RID: 19915
		[FormerlySerializedAs("_audioSource")]
		[SerializeField]
		internal AudioSource _loopingAudioSource;

		// Token: 0x04004DCC RID: 19916
		[Tooltip("The emissive color will be darkened on the materials of these renderers as the fire is extinguished.")]
		[SerializeField]
		internal Renderer[] _emissiveRenderers;

		// Token: 0x04004DCD RID: 19917
		[Header("Asset References")]
		[SerializeField]
		internal GTDirectAssetRef<AudioClip> _extinguishSound;

		// Token: 0x04004DCE RID: 19918
		[SerializeField]
		internal float _extinguishSoundVolume = 1f;

		// Token: 0x04004DCF RID: 19919
		[SerializeField]
		internal GTDirectAssetRef<AudioClip> _igniteSound;

		// Token: 0x04004DD0 RID: 19920
		[SerializeField]
		internal float _igniteSoundVolume = 1f;

		// Token: 0x04004DD1 RID: 19921
		[Header("Values")]
		[SerializeField]
		internal bool _despawnOnExtinguish = true;

		// Token: 0x04004DD2 RID: 19922
		[SerializeField]
		internal float _maxLifetime = 10f;

		// Token: 0x04004DD3 RID: 19923
		[Tooltip("How long it should take to reheat to it's default temperature.")]
		[SerializeField]
		internal float _reheatSpeed = 1f;

		// Token: 0x04004DD4 RID: 19924
		[Tooltip("If you completely extinguish the object, how long should it stay extinguished?")]
		[SerializeField]
		internal float _stayExtinguishedDuration = 1f;

		// Token: 0x04004DD5 RID: 19925
		internal float _defaultTemperature;

		// Token: 0x04004DD6 RID: 19926
		internal float _timeSinceExtinguished;

		// Token: 0x04004DD7 RID: 19927
		internal float _timeSinceDyingStart;

		// Token: 0x04004DD8 RID: 19928
		internal float _timeAlive;

		// Token: 0x04004DD9 RID: 19929
		internal float _psDefaultEmissionRate;

		// Token: 0x04004DDA RID: 19930
		internal ParticleSystem.EmissionModule _psEmissionModule;

		// Token: 0x04004DDB RID: 19931
		internal Vector3Int _spatialGridPosition;

		// Token: 0x04004DDC RID: 19932
		internal bool _isDespawning;

		// Token: 0x04004DDD RID: 19933
		internal float _deathStateDuration;

		// Token: 0x04004DDE RID: 19934
		internal MaterialPropertyBlock[] _emiRenderers_matPropBlocks;

		// Token: 0x04004DDF RID: 19935
		internal Color[] _emiRenderers_defaultColors;
	}
}
