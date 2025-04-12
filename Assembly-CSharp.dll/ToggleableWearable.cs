using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x02000202 RID: 514
public class ToggleableWearable : MonoBehaviour
{
	// Token: 0x06000C10 RID: 3088 RVA: 0x0009B9E0 File Offset: 0x00099BE0
	protected void Awake()
	{
		this.ownerRig = base.GetComponentInParent<VRRig>();
		if (this.ownerRig == null)
		{
			GorillaTagger componentInParent = base.GetComponentInParent<GorillaTagger>();
			if (componentInParent != null)
			{
				this.ownerRig = componentInParent.offlineVRRig;
				this.ownerIsLocal = (this.ownerRig != null);
			}
		}
		if (this.ownerRig == null)
		{
			Debug.LogError("TriggerToggler: Disabling cannot find VRRig.");
			base.enabled = false;
			return;
		}
		foreach (Renderer renderer in this.renderers)
		{
			if (renderer == null)
			{
				Debug.LogError("TriggerToggler: Disabling because a renderer is null.");
				base.enabled = false;
				break;
			}
			renderer.enabled = this.startOn;
		}
		this.hasAudioSource = (this.audioSource != null);
		this.assignedSlotBitIndex = (int)this.assignedSlot;
		if (this.oneShot)
		{
			this.toggleCooldownRange.x = this.toggleCooldownRange.x + this.animationTransitionDuration;
			this.toggleCooldownRange.y = this.toggleCooldownRange.y + this.animationTransitionDuration;
		}
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x0009BAE8 File Offset: 0x00099CE8
	protected void LateUpdate()
	{
		if (this.ownerIsLocal)
		{
			this.toggleCooldownTimer -= Time.deltaTime;
			Transform transform = base.transform;
			if (Physics.OverlapSphereNonAlloc(transform.TransformPoint(this.triggerOffset), this.triggerRadius * transform.localScale.x, this.colliders, this.layerMask) > 0 && this.toggleCooldownTimer < 0f)
			{
				XRController componentInParent = this.colliders[0].GetComponentInParent<XRController>();
				if (componentInParent != null)
				{
					this.LocalToggle(componentInParent.controllerNode == XRNode.LeftHand, true);
				}
				this.toggleCooldownTimer = UnityEngine.Random.Range(this.toggleCooldownRange.x, this.toggleCooldownRange.y);
			}
		}
		else
		{
			bool flag = (this.ownerRig.WearablePackedStates & 1 << this.assignedSlotBitIndex) != 0;
			if (this.isOn != flag)
			{
				this.SharedSetState(flag, true);
			}
		}
		if (this.oneShot)
		{
			if (this.isOn)
			{
				this.progress = Mathf.MoveTowards(this.progress, 1f, Time.deltaTime / this.animationTransitionDuration);
				if (this.progress == 1f)
				{
					if (this.ownerIsLocal)
					{
						this.LocalToggle(false, false);
					}
					else
					{
						this.SharedSetState(false, false);
					}
					this.progress = 0f;
				}
			}
		}
		else
		{
			this.progress = Mathf.MoveTowards(this.progress, this.isOn ? 1f : 0f, Time.deltaTime / this.animationTransitionDuration);
		}
		Animator[] array = this.animators;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFloat(ToggleableWearable.animParam_Progress, this.progress);
		}
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x0009BCA0 File Offset: 0x00099EA0
	private void LocalToggle(bool isLeftHand, bool playAudio)
	{
		this.ownerRig.WearablePackedStates ^= 1 << this.assignedSlotBitIndex;
		this.SharedSetState((this.ownerRig.WearablePackedStates & 1 << this.assignedSlotBitIndex) != 0, playAudio);
		if (playAudio && GorillaTagger.Instance)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, this.isOn ? this.turnOnVibrationDuration : this.turnOffVibrationDuration, this.isOn ? this.turnOnVibrationStrength : this.turnOffVibrationStrength);
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x0009BD34 File Offset: 0x00099F34
	private void SharedSetState(bool state, bool playAudio)
	{
		this.isOn = state;
		Renderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = this.isOn;
		}
		if (!playAudio || !this.hasAudioSource)
		{
			return;
		}
		AudioClip audioClip = this.isOn ? this.toggleOnSound : this.toggleOffSound;
		if (audioClip == null)
		{
			return;
		}
		if (this.oneShot)
		{
			this.audioSource.clip = audioClip;
			this.audioSource.GTPlay();
			return;
		}
		this.audioSource.GTPlayOneShot(audioClip, 1f);
	}

	// Token: 0x04000E67 RID: 3687
	public Renderer[] renderers;

	// Token: 0x04000E68 RID: 3688
	public Animator[] animators;

	// Token: 0x04000E69 RID: 3689
	public float animationTransitionDuration = 1f;

	// Token: 0x04000E6A RID: 3690
	[Tooltip("Whether the wearable state is toggled on by default.")]
	public bool startOn;

	// Token: 0x04000E6B RID: 3691
	[Tooltip("AudioSource to play toggle sounds.")]
	public AudioSource audioSource;

	// Token: 0x04000E6C RID: 3692
	[Tooltip("Sound to play when toggled on.")]
	public AudioClip toggleOnSound;

	// Token: 0x04000E6D RID: 3693
	[Tooltip("Sound to play when toggled off.")]
	public AudioClip toggleOffSound;

	// Token: 0x04000E6E RID: 3694
	[Tooltip("Layer to check for trigger sphere collisions.")]
	public LayerMask layerMask;

	// Token: 0x04000E6F RID: 3695
	[Tooltip("Radius of the trigger sphere.")]
	public float triggerRadius = 0.2f;

	// Token: 0x04000E70 RID: 3696
	[Tooltip("Position in local space to move the trigger sphere.")]
	public Vector3 triggerOffset = Vector3.zero;

	// Token: 0x04000E71 RID: 3697
	[Tooltip("This is to determine what bit to change in VRRig.WearablesPackedStates.")]
	public VRRig.WearablePackedStateSlots assignedSlot;

	// Token: 0x04000E72 RID: 3698
	[Header("Vibration")]
	public float turnOnVibrationDuration = 0.05f;

	// Token: 0x04000E73 RID: 3699
	public float turnOnVibrationStrength = 0.2f;

	// Token: 0x04000E74 RID: 3700
	public float turnOffVibrationDuration = 0.05f;

	// Token: 0x04000E75 RID: 3701
	public float turnOffVibrationStrength = 0.2f;

	// Token: 0x04000E76 RID: 3702
	private VRRig ownerRig;

	// Token: 0x04000E77 RID: 3703
	private bool ownerIsLocal;

	// Token: 0x04000E78 RID: 3704
	private bool isOn;

	// Token: 0x04000E79 RID: 3705
	[SerializeField]
	private Vector2 toggleCooldownRange = new Vector2(0.2f, 0.2f);

	// Token: 0x04000E7A RID: 3706
	private bool hasAudioSource;

	// Token: 0x04000E7B RID: 3707
	private readonly Collider[] colliders = new Collider[1];

	// Token: 0x04000E7C RID: 3708
	private int framesSinceCooldownAndExitingVolume;

	// Token: 0x04000E7D RID: 3709
	private float toggleCooldownTimer;

	// Token: 0x04000E7E RID: 3710
	private int assignedSlotBitIndex;

	// Token: 0x04000E7F RID: 3711
	private static readonly int animParam_Progress = Animator.StringToHash("Progress");

	// Token: 0x04000E80 RID: 3712
	private float progress;

	// Token: 0x04000E81 RID: 3713
	[SerializeField]
	private bool oneShot;
}
