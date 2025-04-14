using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200061B RID: 1563
public class VirtualStumpBarrierSFX : MonoBehaviour
{
	// Token: 0x060026F2 RID: 9970 RVA: 0x000BFC34 File Offset: 0x000BDE34
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

	// Token: 0x060026F3 RID: 9971 RVA: 0x000BFCC4 File Offset: 0x000BDEC4
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

	// Token: 0x060026F4 RID: 9972 RVA: 0x000BFD30 File Offset: 0x000BDF30
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

	// Token: 0x060026F5 RID: 9973 RVA: 0x000BFD9C File Offset: 0x000BDF9C
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

	// Token: 0x04002AD2 RID: 10962
	[SerializeField]
	private AudioSource barrierAudioSource;

	// Token: 0x04002AD3 RID: 10963
	[FormerlySerializedAs("teleportingPlayerSoundClips")]
	[SerializeField]
	private List<AudioClip> PassThroughBarrierSoundClips = new List<AudioClip>();

	// Token: 0x04002AD4 RID: 10964
	private Dictionary<GameObject, bool> trackedGameObjects = new Dictionary<GameObject, bool>();
}
