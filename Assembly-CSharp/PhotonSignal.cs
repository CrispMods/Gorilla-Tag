using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020007E7 RID: 2023
[Serializable]
public class PhotonSignal
{
	// Token: 0x060031EA RID: 12778 RVA: 0x001365C8 File Offset: 0x001347C8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, info);
		}
	}

	// Token: 0x060031EB RID: 12779 RVA: 0x001365F8 File Offset: 0x001347F8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, info);
		}
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x00136624 File Offset: 0x00134824
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, info);
		}
	}

	// Token: 0x060031ED RID: 12781 RVA: 0x00136650 File Offset: 0x00134850
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8, T9> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, info);
		}
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x00136678 File Offset: 0x00134878
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7, T8> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, info);
		}
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x001366A0 File Offset: 0x001348A0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6, T7>(OnSignalReceived<T1, T2, T3, T4, T5, T6, T7> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, arg7, info);
		}
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x00051030 File Offset: 0x0004F230
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5, T6>(OnSignalReceived<T1, T2, T3, T4, T5, T6> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, arg6, info);
		}
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x00051046 File Offset: 0x0004F246
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4, T5>(OnSignalReceived<T1, T2, T3, T4, T5> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, arg5, info);
		}
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x0005105A File Offset: 0x0004F25A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3, T4>(OnSignalReceived<T1, T2, T3, T4> _event, T1 arg1, T2 arg2, T3 arg3, T4 arg4, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, arg4, info);
		}
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x0005106C File Offset: 0x0004F26C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2, T3>(OnSignalReceived<T1, T2, T3> _event, T1 arg1, T2 arg2, T3 arg3, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, arg3, info);
		}
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x0005107C File Offset: 0x0004F27C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1, T2>(OnSignalReceived<T1, T2> _event, T1 arg1, T2 arg2, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, arg2, info);
		}
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x0005108A File Offset: 0x0004F28A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke<T1>(OnSignalReceived<T1> _event, T1 arg1, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(arg1, info);
		}
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x00051097 File Offset: 0x0004F297
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void _Invoke(OnSignalReceived _event, PhotonSignalInfo info)
	{
		if (_event != null)
		{
			_event(info);
		}
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x001366C4 File Offset: 0x001348C4
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

	// Token: 0x060031F8 RID: 12792 RVA: 0x00136724 File Offset: 0x00134924
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

	// Token: 0x060031F9 RID: 12793 RVA: 0x00136784 File Offset: 0x00134984
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

	// Token: 0x060031FA RID: 12794 RVA: 0x001367E0 File Offset: 0x001349E0
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

	// Token: 0x060031FB RID: 12795 RVA: 0x0013683C File Offset: 0x00134A3C
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

	// Token: 0x060031FC RID: 12796 RVA: 0x00136894 File Offset: 0x00134A94
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

	// Token: 0x060031FD RID: 12797 RVA: 0x001368EC File Offset: 0x00134AEC
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

	// Token: 0x060031FE RID: 12798 RVA: 0x00136940 File Offset: 0x00134B40
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

	// Token: 0x060031FF RID: 12799 RVA: 0x00136994 File Offset: 0x00134B94
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

	// Token: 0x06003200 RID: 12800 RVA: 0x001369E4 File Offset: 0x00134BE4
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

	// Token: 0x06003201 RID: 12801 RVA: 0x00136A34 File Offset: 0x00134C34
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

	// Token: 0x06003202 RID: 12802 RVA: 0x00136A80 File Offset: 0x00134C80
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

	// Token: 0x06003203 RID: 12803 RVA: 0x00136ACC File Offset: 0x00134CCC
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

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06003204 RID: 12804 RVA: 0x000510A3 File Offset: 0x0004F2A3
	public bool enabled
	{
		get
		{
			return this._enabled;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06003205 RID: 12805 RVA: 0x00030498 File Offset: 0x0002E698
	public virtual int argCount
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06003206 RID: 12806 RVA: 0x000510AB File Offset: 0x0004F2AB
	// (remove) Token: 0x06003207 RID: 12807 RVA: 0x000510DF File Offset: 0x0004F2DF
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

	// Token: 0x06003208 RID: 12808 RVA: 0x000510FC File Offset: 0x0004F2FC
	protected PhotonSignal()
	{
		this._refID = PhotonSignal.RefID.Register(this);
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x00051134 File Offset: 0x0004F334
	public PhotonSignal(string signalID) : this()
	{
		signalID = ((signalID != null) ? signalID.Trim() : null);
		if (string.IsNullOrWhiteSpace(signalID))
		{
			throw new ArgumentNullException("signalID");
		}
		this._signalID = XXHash32.Compute(signalID, 0U);
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x0005116A File Offset: 0x0004F36A
	public PhotonSignal(int signalID) : this()
	{
		this._signalID = signalID;
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x00051179 File Offset: 0x0004F379
	public void Raise()
	{
		this.Raise(this._receivers);
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x00136B18 File Offset: 0x00134D18
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

	// Token: 0x0600320D RID: 12813 RVA: 0x00051187 File Offset: 0x0004F387
	public void Enable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += this._EventHandle;
		this._enabled = true;
	}

	// Token: 0x0600320E RID: 12814 RVA: 0x000511A6 File Offset: 0x0004F3A6
	public void Disable()
	{
		this._enabled = false;
		PhotonNetwork.NetworkingClient.EventReceived -= this._EventHandle;
	}

	// Token: 0x0600320F RID: 12815 RVA: 0x00136BA8 File Offset: 0x00134DA8
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

	// Token: 0x06003210 RID: 12816 RVA: 0x000511C5 File Offset: 0x0004F3C5
	protected virtual void _Relay(object[] args, PhotonSignalInfo info)
	{
		if (!this._safeInvoke)
		{
			PhotonSignal._Invoke(this._callbacks, info);
			return;
		}
		PhotonSignal._SafeInvoke(this._callbacks, info);
	}

	// Token: 0x06003211 RID: 12817 RVA: 0x000511E8 File Offset: 0x0004F3E8
	public virtual void ClearListeners()
	{
		this._callbacks = null;
	}

	// Token: 0x06003212 RID: 12818 RVA: 0x000511F1 File Offset: 0x0004F3F1
	public virtual void Reset()
	{
		this.ClearListeners();
		this.Disable();
	}

	// Token: 0x06003213 RID: 12819 RVA: 0x000511FF File Offset: 0x0004F3FF
	public virtual void Dispose()
	{
		this._signalID = 0;
		this.Reset();
	}

	// Token: 0x06003214 RID: 12820 RVA: 0x00136C68 File Offset: 0x00134E68
	~PhotonSignal()
	{
		this.Dispose();
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x0005120E File Offset: 0x0004F40E
	public static implicit operator PhotonSignal(string s)
	{
		return new PhotonSignal(s);
	}

	// Token: 0x06003216 RID: 12822 RVA: 0x00051216 File Offset: 0x0004F416
	public static explicit operator PhotonSignal(int i)
	{
		return new PhotonSignal(i);
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x00136C94 File Offset: 0x00134E94
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

	// Token: 0x040035E4 RID: 13796
	protected int _signalID;

	// Token: 0x040035E5 RID: 13797
	protected bool _enabled;

	// Token: 0x040035E6 RID: 13798
	[SerializeField]
	protected ReceiverGroup _receivers = ReceiverGroup.All;

	// Token: 0x040035E7 RID: 13799
	[SerializeField]
	protected CallLimiter _limiter = new CallLimiter(1, 0.1f, 0.5f);

	// Token: 0x040035E8 RID: 13800
	[FormerlySerializedAs("mute")]
	[SerializeField]
	protected bool _mute;

	// Token: 0x040035E9 RID: 13801
	[SerializeField]
	protected bool _safeInvoke = true;

	// Token: 0x040035EA RID: 13802
	[SerializeField]
	protected bool _localOnly;

	// Token: 0x040035EB RID: 13803
	[NonSerialized]
	private int _refID;

	// Token: 0x040035EC RID: 13804
	private OnSignalReceived _callbacks;

	// Token: 0x040035ED RID: 13805
	protected static readonly Dictionary<ReceiverGroup, RaiseEventOptions> gGroupToOptions;

	// Token: 0x040035EE RID: 13806
	protected static readonly SendOptions gSendReliable;

	// Token: 0x040035EF RID: 13807
	protected static readonly SendOptions gSendUnreliable;

	// Token: 0x040035F0 RID: 13808
	public const byte EVENT_CODE = 177;

	// Token: 0x040035F1 RID: 13809
	public const int NULL_SIGNAL = 0;

	// Token: 0x040035F2 RID: 13810
	protected const int HEADER_SIZE = 2;

	// Token: 0x020007E8 RID: 2024
	private class RefID
	{
		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x0005121E File Offset: 0x0004F41E
		public static int Count
		{
			get
			{
				return PhotonSignal.RefID.gRefCount;
			}
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x00051225 File Offset: 0x0004F425
		public RefID()
		{
			this.intValue = StaticHash.ComputeTriple32(PhotonSignal.RefID.gNextID++);
			PhotonSignal.RefID.gRefCount++;
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x00136D10 File Offset: 0x00134F10
		~RefID()
		{
			PhotonSignal.RefID.gRefCount--;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x00136D44 File Offset: 0x00134F44
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

		// Token: 0x040035F3 RID: 13811
		public int intValue;

		// Token: 0x040035F4 RID: 13812
		private static int gNextID = 1;

		// Token: 0x040035F5 RID: 13813
		private static int gRefCount = 0;

		// Token: 0x040035F6 RID: 13814
		private static readonly ConditionalWeakTable<PhotonSignal, PhotonSignal.RefID> gRefTable = new ConditionalWeakTable<PhotonSignal, PhotonSignal.RefID>();
	}
}
