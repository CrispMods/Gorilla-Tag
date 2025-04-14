using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020007D0 RID: 2000
[Serializable]
public class PhotonSignal
{
	// Token: 0x06003140 RID: 12608 RVA: 0x000EDFC8 File Offset: 0x000EC1C8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, info);
		}
	}

	// Token: 0x06003141 RID: 12609 RVA: 0x000EDFF8 File Offset: 0x000EC1F8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, info);
		}
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x000EE024 File Offset: 0x000EC224
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, info);
		}
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x000EE050 File Offset: 0x000EC250
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, info);
		}
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x000EE078 File Offset: 0x000EC278
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, info);
		}
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x000EE0A0 File Offset: 0x000EC2A0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, info);
		}
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x000EE0C3 File Offset: 0x000EC2C3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6>(OnSignalReceived<T1, T2, T3, T4, T5, T6> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, info);
		}
	}

	// Token: 0x06003147 RID: 12615 RVA: 0x000EE0D9 File Offset: 0x000EC2D9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5>(OnSignalReceived<T1, T2, T3, T4, T5> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, info);
		}
	}

	// Token: 0x06003148 RID: 12616 RVA: 0x000EE0ED File Offset: 0x000EC2ED
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4>(OnSignalReceived<T1, T2, T3, T4> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, info);
		}
	}

	// Token: 0x06003149 RID: 12617 RVA: 0x000EE0FF File Offset: 0x000EC2FF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3>(OnSignalReceived<T1, T2, T3> _event, T1 arg1, T2 arg2, T3 arg3, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, info);
		}
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x000EE10F File Offset: 0x000EC30F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2>(OnSignalReceived<T1, T2> _event, T1 arg1, T2 arg2, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, info);
		}
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x000EE11D File Offset: 0x000EC31D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1>(OnSignalReceived<T1> _event, T1 arg1, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, info);
		}
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x000EE12A File Offset: 0x000EC32A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke(OnSignalReceived _event, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(info);
		}
	}

	// Token: 0x0600314D RID: 12621 RVA: 0x000EE138 File Offset: 0x000EC338
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x0600314E RID: 12622 RVA: 0x000EE198 File Offset: 0x000EC398
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x000EE1F8 File Offset: 0x000EC3F8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x000EE254 File Offset: 0x000EC454
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x000EE2B0 File Offset: 0x000EC4B0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x000EE308 File Offset: 0x000EC508
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4, T5, T6, T7>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4, T5, T6, T7>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4, T5, T6, T7>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4, T5, T6, T7> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, arg5, arg6, arg7, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x000EE360 File Offset: 0x000EC560
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4, T5, T6>(OnSignalReceived<T1, T2, T3, T4, T5, T6> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4, T5, T6>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4, T5, T6>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4, T5, T6> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, arg5, arg6, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x000EE3B4 File Offset: 0x000EC5B4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4, T5>(OnSignalReceived<T1, T2, T3, T4, T5> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4, T5>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4, T5>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4, T5> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, arg5, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003155 RID: 12629 RVA: 0x000EE408 File Offset: 0x000EC608
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3, T4>(OnSignalReceived<T1, T2, T3, T4> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3, T4>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3, T4>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3, T4> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, arg4, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x000EE458 File Offset: 0x000EC658
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2, T3>(OnSignalReceived<T1, T2, T3> _event, T1 arg1, T2 arg2, T3 arg3, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2, T3>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2, T3>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2, T3> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, arg3, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x000EE4A8 File Offset: 0x000EC6A8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1, T2>(OnSignalReceived<T1, T2> _event, T1 arg1, T2 arg2, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1, T2>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1, T2>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1, T2> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, arg2, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x000EE4F4 File Offset: 0x000EC6F4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke<T1>(OnSignalReceived<T1> _event, T1 arg1, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived<T1>[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived<T1>>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived<T1> onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(arg1, info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x000EE540 File Offset: 0x000EC740
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _SafeInvoke(OnSignalReceived _event, PhotonSignalInfo info)
	{
		ref readonly OnSignalReceived[] ptr = ref PhotonUtils.FetchDelegatesNonAlloc<OnSignalReceived>(_event);
		for (int i = 0; i < ptr.Length; i++)
		{
			try
			{
				OnSignalReceived onSignalReceived = ptr[i];
				if (onSignalReceived != null)
				{
					onSignalReceived(info);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x0600315A RID: 12634 RVA: 0x000EE58C File Offset: 0x000EC78C
	public bool enabled
	{
		get
		{
			return this._enabled;
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x0600315B RID: 12635 RVA: 0x00002076 File Offset: 0x00000276
	public virtual int argCount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x0600315C RID: 12636 RVA: 0x000EE594 File Offset: 0x000EC794
	// (remove) Token: 0x0600315D RID: 12637 RVA: 0x000EE5C8 File Offset: 0x000EC7C8
	public event OnSignalReceived OnSignal
	{
		add
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived)Delegate.Remove(this._callbacks, value);
			this._callbacks = (OnSignalReceived)Delegate.Combine(this._callbacks, value);
		}
		remove
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived)Delegate.Remove(this._callbacks, value);
		}
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x000EE5E5 File Offset: 0x000EC7E5
	protected PhotonSignal()
	{
		this._refID = PhotonSignal.RefID.Register(this);
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x000EE61D File Offset: 0x000EC81D
	public PhotonSignal(string signalID) : this()
	{
		signalID = ((signalID != null) ? signalID.Trim() : null);
		if (string.IsNullOrWhiteSpace(signalID))
		{
			throw new ArgumentNullException("signalID");
		}
		this._signalID = XXHash32.Compute(signalID, 0U);
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x000EE653 File Offset: 0x000EC853
	public PhotonSignal(int signalID) : this()
	{
		this._signalID = signalID;
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x000EE662 File Offset: 0x000EC862
	public void Raise()
	{
		this.Raise(this._receivers);
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x000EE670 File Offset: 0x000EC870
	public void Raise(ReceiverGroup receivers)
	{
		if (!this._enabled)
		{
			return;
		}
		if (this._mute)
		{
			return;
		}
		RaiseEventOptions raiseEventOptions = PhotonSignal.gGroupToOptions[receivers];
		object[] array = PhotonUtils.FetchScratchArray(2);
		int serverTimestamp = PhotonNetwork.ServerTimestamp;
		array[0] = this._signalID;
		array[1] = serverTimestamp;
		if (this._localOnly || !PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
		{
			PhotonSignalInfo info = new PhotonSignalInfo(PhotonUtils.LocalNetPlayer, serverTimestamp);
			this._Relay(array, info);
			return;
		}
		PhotonNetwork.RaiseEvent(177, array, raiseEventOptions, PhotonSignal.gSendReliable);
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x000EE6FD File Offset: 0x000EC8FD
	public void Enable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += this._EventHandle;
		this._enabled = true;
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x000EE71C File Offset: 0x000EC91C
	public void Disable()
	{
		this._enabled = false;
		PhotonNetwork.NetworkingClient.EventReceived -= this._EventHandle;
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x000EE73C File Offset: 0x000EC93C
	private void _EventHandle(EventData eventData)
	{
		if (!this._enabled)
		{
			return;
		}
		if (this._mute)
		{
			return;
		}
		if (eventData.Code != 177)
		{
			return;
		}
		int sender = eventData.Sender;
		object[] array = eventData.CustomData as object[];
		if (array == null)
		{
			return;
		}
		if (array.Length < 2 + this.argCount)
		{
			return;
		}
		object obj = array[0];
		if (!(obj is int))
		{
			return;
		}
		int num = (int)obj;
		if (num == 0 || num != this._signalID)
		{
			return;
		}
		obj = array[1];
		if (!(obj is int))
		{
			return;
		}
		int timestamp = (int)obj;
		if (!this._limiter.CheckCallTime(Time.time))
		{
			return;
		}
		NetPlayer netPlayer = PhotonUtils.GetNetPlayer(sender);
		PhotonSignalInfo info = new PhotonSignalInfo(netPlayer, timestamp);
		this._Relay(array, info);
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x000EE7FB File Offset: 0x000EC9FB
	protected virtual void _Relay(object[] args, PhotonSignalInfo info)
	{
		if (!this._safeInvoke)
		{
			PhotonSignal._Invoke(this._callbacks, info);
			return;
		}
		PhotonSignal._SafeInvoke(this._callbacks, info);
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x000EE81E File Offset: 0x000ECA1E
	public virtual void ClearListeners()
	{
		this._callbacks = null;
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x000EE827 File Offset: 0x000ECA27
	public virtual void Reset()
	{
		this.ClearListeners();
		this.Disable();
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x000EE835 File Offset: 0x000ECA35
	public virtual void Dispose()
	{
		this._signalID = 0;
		this.Reset();
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x000EE844 File Offset: 0x000ECA44
	~PhotonSignal()
	{
		this.Dispose();
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x000EE870 File Offset: 0x000ECA70
	public static implicit operator PhotonSignal(string s)
	{
		return new PhotonSignal(s);
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x000EE878 File Offset: 0x000ECA78
	public static explicit operator PhotonSignal(int i)
	{
		return new PhotonSignal(i);
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x000EE880 File Offset: 0x000ECA80
	static PhotonSignal()
	{
		Dictionary<ReceiverGroup, RaiseEventOptions> dictionary = new Dictionary<ReceiverGroup, RaiseEventOptions>();
		dictionary[ReceiverGroup.Others] = new RaiseEventOptions
		{
			Receivers = ReceiverGroup.Others
		};
		dictionary[ReceiverGroup.All] = new RaiseEventOptions
		{
			Receivers = ReceiverGroup.All
		};
		dictionary[ReceiverGroup.MasterClient] = new RaiseEventOptions
		{
			Receivers = ReceiverGroup.MasterClient
		};
		PhotonSignal.gGroupToOptions = dictionary;
		PhotonSignal.gSendReliable = SendOptions.SendReliable;
		PhotonSignal.gSendUnreliable = SendOptions.SendUnreliable;
		PhotonSignal.gSendReliable.Encrypt = true;
		PhotonSignal.gSendUnreliable.Encrypt = true;
	}

	// Token: 0x04003540 RID: 13632
	protected int _signalID;

	// Token: 0x04003541 RID: 13633
	protected bool _enabled;

	// Token: 0x04003542 RID: 13634
	[SerializeField]
	protected ReceiverGroup _receivers = ReceiverGroup.All;

	// Token: 0x04003543 RID: 13635
	[SerializeField]
	protected CallLimiter _limiter = new CallLimiter(1, 0.1f, 0.5f);

	// Token: 0x04003544 RID: 13636
	[FormerlySerializedAs("mute")]
	[SerializeField]
	protected bool _mute;

	// Token: 0x04003545 RID: 13637
	[SerializeField]
	protected bool _safeInvoke = true;

	// Token: 0x04003546 RID: 13638
	[SerializeField]
	protected bool _localOnly;

	// Token: 0x04003547 RID: 13639
	[NonSerialized]
	private int _refID;

	// Token: 0x04003548 RID: 13640
	private OnSignalReceived _callbacks;

	// Token: 0x04003549 RID: 13641
	protected static readonly Dictionary<ReceiverGroup, RaiseEventOptions> gGroupToOptions;

	// Token: 0x0400354A RID: 13642
	protected static readonly SendOptions gSendReliable;

	// Token: 0x0400354B RID: 13643
	protected static readonly SendOptions gSendUnreliable;

	// Token: 0x0400354C RID: 13644
	public const byte EVENT_CODE = 177;

	// Token: 0x0400354D RID: 13645
	public const int NULL_SIGNAL = 0;

	// Token: 0x0400354E RID: 13646
	protected const int HEADER_SIZE = 2;

	// Token: 0x020007D1 RID: 2001
	private class RefID
	{
		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x000EE8FA File Offset: 0x000ECAFA
		public static int Count
		{
			get
			{
				return PhotonSignal.RefID.gRefCount;
			}
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x000EE901 File Offset: 0x000ECB01
		public RefID()
		{
			this.intValue = StaticHash.ComputeTriple32(PhotonSignal.RefID.gNextID++);
			PhotonSignal.RefID.gRefCount++;
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x000EE930 File Offset: 0x000ECB30
		~RefID()
		{
			PhotonSignal.RefID.gRefCount--;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x000EE964 File Offset: 0x000ECB64
		public static int Register(PhotonSignal ps)
		{
			if (ps == null)
			{
				return 0;
			}
			PhotonSignal.RefID refID = new PhotonSignal.RefID();
			PhotonSignal.RefID.gRefTable.Add(ps, refID);
			return refID.intValue;
		}

		// Token: 0x0400354F RID: 13647
		public int intValue;

		// Token: 0x04003550 RID: 13648
		private static int gNextID = 1;

		// Token: 0x04003551 RID: 13649
		private static int gRefCount = 0;

		// Token: 0x04003552 RID: 13650
		private static readonly ConditionalWeakTable<PhotonSignal, PhotonSignal.RefID> gRefTable = new ConditionalWeakTable<PhotonSignal, PhotonSignal.RefID>();
	}
}
