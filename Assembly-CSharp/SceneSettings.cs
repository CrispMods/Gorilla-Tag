using System;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class SceneSettings : MonoBehaviour
{
	// Token: 0x06001412 RID: 5138 RVA: 0x0006256F File Offset: 0x0006076F
	private void Awake()
	{
		Time.fixedDeltaTime = this.m_fixedTimeStep;
		Physics.gravity = Vector3.down * 9.81f * this.m_gravityScalar;
		Physics.defaultContactOffset = this.m_defaultContactOffset;
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000625A6 File Offset: 0x000607A6
	private void Start()
	{
		SceneSettings.CollidersSetContactOffset(this.m_defaultContactOffset);
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x000625B4 File Offset: 0x000607B4
	private static void CollidersSetContactOffset(float contactOffset)
	{
		Collider[] array = Object.FindObjectsOfType<Collider>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].contactOffset = contactOffset;
		}
	}

	// Token: 0x04001630 RID: 5680
	[Header("Time")]
	[SerializeField]
	private float m_fixedTimeStep = 0.01f;

	// Token: 0x04001631 RID: 5681
	[Header("Physics")]
	[SerializeField]
	private float m_gravityScalar = 0.75f;

	// Token: 0x04001632 RID: 5682
	[SerializeField]
	private float m_defaultContactOffset = 0.001f;
}
