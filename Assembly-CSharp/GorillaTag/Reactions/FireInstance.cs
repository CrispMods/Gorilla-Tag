using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BC0 RID: 3008
	public class FireInstance : MonoBehaviour
	{
		// Token: 0x06004BFA RID: 19450 RVA: 0x00171969 File Offset: 0x0016FB69
		protected void Awake()
		{
			FireManager.Register(this);
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x00171971 File Offset: 0x0016FB71
		protected void OnDestroy()
		{
			FireManager.Unregister(this);
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00171979 File Offset: 0x0016FB79
		protected void OnEnable()
		{
			FireManager.OnEnable(this);
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x00171981 File Offset: 0x0016FB81
		protected void OnDisable()
		{
			FireManager.OnDisable(this);
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00171989 File Offset: 0x0016FB89
		protected void OnTriggerEnter(Collider other)
		{
			FireManager.OnTriggerEnter(this, other);
		}

		// Token: 0x04004DB6 RID: 19894
		[Header("Scene References")]
		[Tooltip("If not assigned it will try to auto assign to a component on the same GameObject.")]
		[SerializeField]
		internal Collider _collider;

		// Token: 0x04004DB7 RID: 19895
		[Tooltip("If not assigned it will try to auto assign to a component on the same GameObject.")]
		[FormerlySerializedAs("_thermalSourceVolume")]
		[SerializeField]
		internal ThermalSourceVolume _thermalVolume;

		// Token: 0x04004DB8 RID: 19896
		[SerializeField]
		internal ParticleSystem _particleSystem;

		// Token: 0x04004DB9 RID: 19897
		[FormerlySerializedAs("_audioSource")]
		[SerializeField]
		internal AudioSource _loopingAudioSource;

		// Token: 0x04004DBA RID: 19898
		[Tooltip("The emissive color will be darkened on the materials of these renderers as the fire is extinguished.")]
		[SerializeField]
		internal Renderer[] _emissiveRenderers;

		// Token: 0x04004DBB RID: 19899
		[Header("Asset References")]
		[SerializeField]
		internal GTDirectAssetRef<AudioClip> _extinguishSound;

		// Token: 0x04004DBC RID: 19900
		[SerializeField]
		internal float _extinguishSoundVolume = 1f;

		// Token: 0x04004DBD RID: 19901
		[SerializeField]
		internal GTDirectAssetRef<AudioClip> _igniteSound;

		// Token: 0x04004DBE RID: 19902
		[SerializeField]
		internal float _igniteSoundVolume = 1f;

		// Token: 0x04004DBF RID: 19903
		[Header("Values")]
		[SerializeField]
		internal bool _despawnOnExtinguish = true;

		// Token: 0x04004DC0 RID: 19904
		[SerializeField]
		internal float _maxLifetime = 10f;

		// Token: 0x04004DC1 RID: 19905
		[Tooltip("How long it should take to reheat to it's default temperature.")]
		[SerializeField]
		internal float _reheatSpeed = 1f;

		// Token: 0x04004DC2 RID: 19906
		[Tooltip("If you completely extinguish the object, how long should it stay extinguished?")]
		[SerializeField]
		internal float _stayExtinguishedDuration = 1f;

		// Token: 0x04004DC3 RID: 19907
		internal float _defaultTemperature;

		// Token: 0x04004DC4 RID: 19908
		internal float _timeSinceExtinguished;

		// Token: 0x04004DC5 RID: 19909
		internal float _timeSinceDyingStart;

		// Token: 0x04004DC6 RID: 19910
		internal float _timeAlive;

		// Token: 0x04004DC7 RID: 19911
		internal float _psDefaultEmissionRate;

		// Token: 0x04004DC8 RID: 19912
		internal ParticleSystem.EmissionModule _psEmissionModule;

		// Token: 0x04004DC9 RID: 19913
		internal Vector3Int _spatialGridPosition;

		// Token: 0x04004DCA RID: 19914
		internal bool _isDespawning;

		// Token: 0x04004DCB RID: 19915
		internal float _deathStateDuration;

		// Token: 0x04004DCC RID: 19916
		internal MaterialPropertyBlock[] _emiRenderers_matPropBlocks;

		// Token: 0x04004DCD RID: 19917
		internal Color[] _emiRenderers_defaultColors;
	}
}
