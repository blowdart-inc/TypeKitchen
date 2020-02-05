﻿using System;
using TypeKitchen.Serialization;

namespace TypeKitchen.ValueHash
{
	public static class ValueHash
	{
		private static readonly ulong DefaultSeed;
		
		static ValueHash() => DefaultSeed = BitConverter.ToUInt64(new[] {(byte) 'd', (byte) 'e', (byte) 'a', (byte) 'd', (byte) 'b', (byte) 'e', (byte) 'e', (byte) 'f' }, 0);
		
		public static ulong ComputeHash(object instance, IObjectSerializer serializer = null, ITypeResolver typeResolver = null, IValueHashProvider valueHashProvider = null, ulong? seed = null) => ComputeHash((serializer ?? Defaults.ObjectSerializer).ToBuffer(instance, typeResolver ?? Defaults.TypeResolver), valueHashProvider, seed);

		public static ulong ComputeHash(ReadOnlySpan<byte> buffer, IValueHashProvider valueHashProvider = null, ulong? seed = null) => buffer == null ? default : (valueHashProvider ?? Defaults.ValueHashProvider).ComputeHash64(buffer, seed ?? DefaultSeed);
	}
}