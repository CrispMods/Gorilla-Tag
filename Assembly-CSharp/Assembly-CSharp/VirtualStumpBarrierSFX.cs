﻿using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200061C RID: 1564
public class VirtualStumpBarrierSFX : MonoBehaviour
{
	// Token: 0x060026FA RID: 9978 RVA: 0x000C00B4 File Offset: 0x000BE2B4
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			this.PlaySFX();
			return;
		}
		VRRig vrrig;
		if (other.gameObject.TryGetComponent<VRRig>(out vrrig) && !vrrig.isLocal)
		{
			bool value = other.gameObject.transform.position.z < base.gameObject.transform.position.z;
			this.trackedGameObjects.Add(other.gameObject, value);
			this.OnTriggerStay(other);
		}
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000C0144 File Offset: 0x000BE344
	public void OnTriggerStay(Collider other)
	{
		bool flag;
		if (!this.trackedGameObjects.TryGetValue(other.gameObject, out flag))
		{
			return;
		}
		bool flag2 = other.gameObject.transform.position.z < base.gameObject.transform.position.z;
		if (flag != flag2)
		{
			this.PlaySFX();
			this.trackedGameObjects.Remove(other.gameObject);
		}
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x000C01B0 File Offset: 0x000BE3B0
	public void OnTriggerExit(Collider other)
	{
		bool flag;
		if (this.trackedGameObjects.TryGetValue(other.gameObject, out flag))
		{
			bool flag2 = other.gameObject.transform.position.z < base.gameObject.transform.position.z;
			if (flag != flag2)
			{
				this.PlaySFX();
			}
			this.trackedGameObjects.Remove(other.gameObject);
		}
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x000C021C File Offset: 0x000BE41C
	public void PlaySFX()
	{
		if (this.barrierAudioSource.IsNull())
		{
			return;
		}
		if (this.PassThroughBarrierSoundClips.IsNullOrEmpty<AudioClip>())
		{
			return;
		}
		this.barrierAudioSource.clip = this.PassThroughBarrierSoundClips[Random.Range(0, this.PassThroughBarrierSoundClips.Count)];
		this.barrierAudioSource.Play();
	}

	// Token: 0x04002AD8 RID: 10968
	[SerializeField]
	private AudioSource barrierAudioSource;

	// Token: 0x04002AD9 RID: 10969
	[FormerlySerializedAs("teleportingPlayerSoundClips")]
	[SerializeField]
	private List<AudioClip> PassThroughBarrierSoundClips = new List<AudioClip>();

	// Token: 0x04002ADA RID: 10970
	private Dictionary<GameObject, bool> trackedGameObjects = new Dictionary<GameObject, bool>();
}
