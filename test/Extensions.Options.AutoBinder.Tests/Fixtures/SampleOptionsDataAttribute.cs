namespace Extensions.Options.AutoBinder.Tests.Fixtures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Bogus;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SampleOptionsDataAttribute : DataAttribute
    {
        private readonly int _count;
        private readonly object[] _values;

        public SampleOptionsDataAttribute(int count = 1, params object[] values)
        {
            _count = count;
            _values = values;
        }

        /// <inheritdoc />
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            _ = testMethod ?? throw new ArgumentNullException(nameof(testMethod));
            var faker = new Faker<SampleOptions>()
                .RuleFor(o => o.StringVal, f => f.Random.Word())
                .RuleFor(o => o.IntVal, f => f.Random.Int())
                .RuleFor(o => o.BoolVal, f => f.Random.Bool(0.5f))
                .RuleFor(o => o.DateVal, f => f.Date.Past());

            // https://github.com/bchavez/Bogus/issues/207#issuecomment-464714277
            var data = faker.Generate(_count)
                .Select(dto =>
                    new object[] { dto.StringVal, dto.IntVal, dto.BoolVal, dto.DateVal!.Value.ToString("R") });

            return data;
        }
    }
}