﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.OperationResults
{
    public class MessageTest
    {
        // Arrange
        private readonly OperationMessageLevel _severity = OperationMessageLevel.Information;

        public class Ctor1 : MessageTest
        {
            [Fact]
            public void Should_set_the_level()
            {
                // Act
                var obj = new Message(_severity);

                // Assert
                Assert.Equal(_severity, obj.Severity);
            }

            [Fact]
            public void Should_create_a_default_details_dictionary()
            {
                // Act
                var obj = new Message(_severity);

                // Assert
                Assert.NotNull(obj.Details);
            }

        }
        public class Ctor2 : MessageTest
        {
            // Arrange
            private readonly IDictionary<string, object> _details = new Dictionary<string, object>();

            [Fact]
            public void Should_set_the_level()
            {
                // Act
                var obj = new Message(_severity, _details);

                // Assert
                Assert.Equal(_severity, obj.Severity);
            }

            [Fact]
            public void Should_set_the_details()
            {
                // Act
                var obj = new Message(_severity, _details);

                // Assert
                Assert.Equal(_details, obj.Details);
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_details_is_null()
            {
                Assert.Throws<ArgumentNullException>("details", () => new Message(_severity, null));
            }
        }

        public abstract class Ctor3 : MessageTest
        {
            protected abstract bool IgnoreNull { get; }

            public class When_ignoreNull_is_true : Ctor3
            {
                protected override bool IgnoreNull => true;
            }

            public class When_ignoreNull_is_false : Ctor3
            {
                protected override bool IgnoreNull => false;
            }

            [Fact]
            public void Should_set_the_level()
            {
                // Act
                var obj = new Message(_severity, new { }, IgnoreNull);

                // Assert
                Assert.Equal(_severity, obj.Severity);
            }

            [Fact]
            public void Should_load_anonymous_object_into_details()
            {
                // Arrange
                var details = new { SomeProp = "Some value", SomeCheck = true };

                // Act
                var obj = new Message(_severity, details, IgnoreNull);

                // Assert
                Assert.Collection(obj.Details,
                    p => AssertDetailsKeyValue(p, "SomeProp", "Some value"),
                    p => AssertDetailsKeyValue(p, "SomeCheck", true)
                );
            }

            [Fact]
            public void Should_load_typed_object_into_details()
            {
                // Arrange
                var details = new SomeClass
                {
                    SomeProp = "Some value",
                    SomeCheck = true
                };

                // Act
                var obj = new Message(_severity, details, IgnoreNull);

                // Assert
                Assert.Collection(obj.Details,
                    p => AssertDetailsKeyValue(p, "SomeProp", "Some value"),
                    p => AssertDetailsKeyValue(p, "SomeCheck", true)
                );
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_details_is_null()
            {
                Assert.Throws<ArgumentNullException>("details", () => new Message(_severity, null, IgnoreNull));
            }

            private class SomeClass
            {
                public string SomeProp { get; set; }
                public bool SomeCheck { get; set; }
            }

            private void AssertDetailsKeyValue(KeyValuePair<string, object> pair, string expectedKey, object expectedValue)
            {
                Assert.Equal(expectedKey, pair.Key);
                Assert.Equal(expectedValue, pair.Value);
            }
        }
    }
}
