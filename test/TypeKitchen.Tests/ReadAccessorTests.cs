﻿// Copyright (c) Blowdart, Inc. All rights reserved.

using TypeKitchen.Tests.Fakes;
using Xunit;

namespace TypeKitchen.Tests
{
    public class ReadAccessorTests
    {
        [Fact]
        public void GetTests_AnonymousType()
        {
            var target = GetOutOfMethodTarget();
            var accessor = ReadAccessor.Create(target.GetType());
            var foo = accessor[target, "Foo"];
            var bar = accessor[target, "Bar"];
            Assert.Equal("Bar", foo);
            Assert.Equal("Baz", bar);

            Assert.True(accessor.TryGetValue(target, "Bar", out var value));
            Assert.Equal("Baz", value);

            target = new {Foo = "Fizz", Bar = "Buzz"};
            var other = ReadAccessor.Create(target.GetType());
            Assert.Equal(accessor, other);
            accessor = other;

            foo = accessor[target, "Foo"];
            bar = accessor[target, "Bar"];
            Assert.Equal("Fizz", foo);
            Assert.Equal("Buzz", bar);

            Assert.True(accessor.TryGetValue(target, "Bar", out value));
            Assert.Equal("Buzz", value);
        }

        [Fact]
        public void GetTests_PropertiesAndFields()
        {
            var target = new OnePropertyOneField {Foo = "Bar", Bar = "Baz"};
            var accessor = ReadAccessor.Create(target.GetType());
            var foo = accessor[target, "Foo"];
            var bar = accessor[target, "Bar"];
            Assert.Equal("Bar", foo);
            Assert.Equal("Baz", bar);

            Assert.True(accessor.TryGetValue(target, "Bar", out var value));
            Assert.Equal("Baz", value);

            target = new OnePropertyOneField {Foo = "Fizz", Bar = "Buzz"};
            var other = ReadAccessor.Create(target.GetType());
            Assert.Equal(accessor, other);
            accessor = other;

            foo = accessor[target, "Foo"];
            bar = accessor[target, "Bar"];
            Assert.Equal("Fizz", foo);
            Assert.Equal("Buzz", bar);

            Assert.True(accessor.TryGetValue(target, "Bar", out value));
            Assert.Equal("Buzz", value);

            Assert.Equal(typeof(OnePropertyOneField), accessor.Type);
        }

        public object GetOutOfMethodTarget()
        {
            return new { Foo = "Bar", Bar = "Baz" };
        }
    }
}