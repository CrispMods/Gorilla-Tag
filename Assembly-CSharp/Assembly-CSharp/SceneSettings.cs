using System;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class SceneSettings : MonoBehaviour
{
	// Token: 0x06001415 RID: 5141 RVA: 0x000628F3 File Offset: 0x00060AF3
	private void Awake()
	{
		Time.fixedDeltaTime = this.m_fixedTimeStep;
		Physics.gravity = Vector3.down * 9.81f * this.m_gravityScalar;
		Physics.defaultContactOffset = this.m_defaultContactOffset;
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x0006292A File Offset: 0x00060B2A
	private void Start()
	{
		SceneSettings.CollidersSetContactOffset(this.m_defaultContactOffset);
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x00062938 File Offset: 0x00060B38
	private static void CollidersSetContactOffset(float contactOffset)
	{
		Collider[] array = Object.FindObjectsOfType<Collider>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].contactOffset = contactOffset;
		}
	}

	// Token: 0x04001631 RID: 5681
	[Header("Time")]
	[SerializeField]
	private float m_fixedTimeStep = 0.01f;

	// Token: 0x04001632 RID: 5682
	[Header("Physics")]
	[SerializeField]
	private float m_gravityScalar = 0.75f;

	// Token: 0x04001633 RID: 5683
	[SerializeField]
	private float m_defaultContactOffset = 0.001f;
}
