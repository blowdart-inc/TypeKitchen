﻿// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using TypeKitchen.Composition;
using Xunit;

namespace TypeKitchen.Tests.Composition
{
	public class CompositionTests
	{
		[Fact]
		public void BasicTests_compose_simple_system()
		{
            var container = Container.Create();
            container.AddSystem<VelocitySystem>();
            container.AddSystem<ClockSystem>();

			var entity = container.CreateEntity(new Velocity { Value = 10f }, new Position2D());

			var c = container.GetComponents(entity).ToArray();
			var velocity = c[1].QuackLike<Velocity>();
			Assert.Equal(10f, velocity.Value);

			//var snapshot = container.Snapshot();

			AssertSimulation(container, entity);

			//container.Restore(snapshot);

			//AssertSimulation(container, entity);
		}

		private static void AssertSimulation(Container container, uint entity)
		{
			container.Update(TimeSpan.FromSeconds(0.1));

			var c = container.GetComponents(entity).ToArray();
			var position = c[0].QuackLike<Position2D>();
			var velocity = c[1].QuackLike<Velocity>();

			Assert.Equal(1, position.X);
			Assert.Equal(1, position.Y);
			Assert.Equal(10f, velocity.Value);
		}

		public sealed class ClockSystem : ISystem<float>
		{
			public bool Update(UpdateContext updateContext, ref float elapsed)
			{
				return false;
			}
		}

		public sealed class VelocitySystem : ISystemWithState<TimeSpan, Velocity, Position2D>, IDependOn<ClockSystem>
		{
			public bool Update(UpdateContext updateContext, TimeSpan elapsedTime, ref Velocity velocity, ref Position2D position)
			{
				var delta = elapsedTime.Milliseconds * 0.001;
				position.X += (int) (velocity.Value * delta);
				position.Y += (int) (velocity.Value * delta);
				return true;
			}
		}

		[DebuggerDisplay("{" + nameof(Value) + "}")]
		public struct Velocity
		{
			public float Value { get; set; }
		}

		[DebuggerDisplay("({" + nameof(X) + "}, {" + nameof(Y) + "})")]
		public struct Position2D
		{
			public int X { get; set; }
			public int Y { get; set; }
		}
	}
}
