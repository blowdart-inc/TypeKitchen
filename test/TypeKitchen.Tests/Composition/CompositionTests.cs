﻿using System.Diagnostics;
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
            var entity = container.CreateEntity<Velocity, Position2D>();
			entity.Set(new Velocity { Value = 10f }, container);
            container.AddSystem<VelocitySystem>();
            container.AddSystem<ClockSystem>();
			container.Update();

			var c = container.GetComponents(entity).ToArray();

			var c0 = c[0];
			var c1 = c[1];

			var position = c0.GetType().GetProperty("Ref").GetValue(c0);
			var velocity = c1.GetType().GetProperty("Ref").GetValue(c1);

			Assert.Equal(1, ((Position2D) position).X);
			Assert.Equal(1, ((Position2D) position).Y);
			Assert.Equal(10, ((Velocity) velocity).Value);
		}

		public sealed class ClockSystem : ISystem<float>
		{
			public void Update(ref float elapsed)
			{
				
			}
		}

		public sealed class VelocitySystem : ISystem<Velocity, Position2D>, IDependOn<ClockSystem>
		{
			public void Update(ref Velocity velocity, ref Position2D position)
			{
				position.X += (int) (velocity.Value * 0.1f);
				position.Y += (int) (velocity.Value * 0.1f);
			}
		}

		[DebuggerDisplay("{" + nameof(Value) + "}")]
		public struct Velocity
		{
			public float Value;
		}

		[DebuggerDisplay("({" + nameof(X) + "}, {" + nameof(Y) + "})")]
		public struct Position2D
		{
			public int X;
			public int Y;
		}
	}
}
