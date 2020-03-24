﻿// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using TypeKitchen.Internal;
using TypeKitchen.Reflection;

namespace TypeKitchen.Composition.Internal
{
	internal static class TypeExtensions
	{
		public static Value128 Archetype(this IEnumerable<Type> componentTypes, Value128 seed = default)
		{
			Value128 archetype = default;
			foreach (var component in componentTypes.StableOrder(x => x.FullName))
			{
				var componentId = Hashing.MurmurHash3(component.FullName, seed);
				archetype = componentId ^ archetype;
			}
			return archetype;
		}
	}
}