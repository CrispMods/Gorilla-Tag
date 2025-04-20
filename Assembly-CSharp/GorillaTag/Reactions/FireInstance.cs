using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BEE RID: 3054
	public class FireInstance : MonoBehaviour
	{
		// Token: 0x06004D46 RID: 19782 RVA: 0x00062B88 File Offset: 0x00060D88
		protected void Awake()
		{
			FireManager.Register(this);
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x00062B90 File Offset: 0x00060D90
		protected void OnDestroy()
		{
			FireManager.Unregister(this);
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x00062B98 File Offset: 0x00060D98
		protected void OnEnable()
		{
			FireManager.OnEnable(this);
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x00062BA0 File Offset: 0x00060DA0
		protected void OnDisable()
		{
			FireManager.OnDisable(this);
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x00062BA8 File Offset: 0x00060DA8
		protected void OnTriggerEnter(Collider other)
		{
			FireManager.OnTriggerEnter(this, other);
		}

		// Token: 0x04004EAC RID: 20140
		[Header("Scene References")]
		[Tooltip("If not assigned it will try to auto assign to a component on the same GameObject.")]
		[SerializeField]
		internal Collider _collider;

		// Token: 0x04004EAD RID: 20141
		[Tooltip("If not assigned it will try to auto assign to a component on the same GameObject.")]
		[FormerlySerializedAs("_thermalSourceVolume")]
		[SerializeField]
		internal ThermalSourceVolume _thermalVolume;

		// Token: 0x04004EAE RID: 20142
		[SerializeField]
		internal ParticleSystem _particleSystem;

		// Token: 0x04004EAF RID: 20143
		[FormerlySerializedAs("_audioSource")]
		[SerializeField]
		internal AudioSource _loopingAudioSource;

		// Token: 0x04004EB0 RID: 20144
		[Tooltip("The emissive color will be darkened on the materials of these renderers as the fire is extinguished.")]
		[SerializeField]
		internal Renderer[] _emissiveRenderers;

		// Token: 0x04004EB1 RID: 20145
		[Header("Asset References")]
		[SerializeField]
		internal GTDirectAssetRef<AudioClip> _extinguishSound;

		// Token: 0x04004EB2 RID: 20146
		[SerializeField]
		internal float _extinguishSoundVolume = 1f;

		// Token: 0x04004EB3 RID: 20147
		[SerializeField]
		internal GTDirectAssetRef<AudioClip> _igniteSound;

		// Token: 0x04004EB4 RID: 20148
		[SerializeField]
		internal float _igniteSoundVolume = 1f;

		// Token: 0x04004EB5 RID: 20149
		[Header("Values")]
		[SerializeField]
		internal bool _despawnOnExtinguish = true;

		// Token: 0x04004EB6 RID: 20150
		[SerializeField]
		internal float _maxLifetime = 10f;

		// Token: 0x04004EB7 RID: 20151
		[Tooltip("How long it should take to reheat to it's default temperature.")]
		[SerializeField]
		internal float _reheatSpeed = 1f;

		// Token: 0x04004EB8 RID: 20152
		[Tooltip("If you completely extinguish the object, how long should it stay extinguished?")]
		[SerializeField]
		internal float _stayExtinguishedDuration = 1f;

		// Token: 0x04004EB9 RID: 20153
		internal float _defaultTemperature;

		// Token: 0x04004EBA RID: 20154
		internal float _timeSinceExtinguished;

		// Token: 0x04004EBB RID: 20155
		internal float _timeSinceDyingStart;

		// Token: 0x04004EBC RID: 20156
		internal float _timeAlive;

		// Token: 0x04004EBD RID: 20157
		internal float _psDefaultEmissionRate;

		// Token: 0x04004EBE RID: 20158
		internal ParticleSystem.EmissionModule _psEmissionModule;

		// Token: 0x04004EBF RID: 20159
		internal Vector3Int _spatialGridPosition;

		// Token: 0x04004EC0 RID: 20160
		internal bool _isDespawning;

		// Token: 0x04004EC1 RID: 20161
		internal float _deathStateDuration;

		// Token: 0x04004EC2 RID: 20162
		internal MaterialPropertyBlock[] _emiRenderers_matPropBlocks;

		// Token: 0x04004EC3 RID: 20163
		internal Color[] _emiRenderers_defaultColors;
	}
}
