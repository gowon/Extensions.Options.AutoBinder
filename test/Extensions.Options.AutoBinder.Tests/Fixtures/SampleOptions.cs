namespace Extensions.Options.AutoBinder.Tests.Fixtures
{
    using System;

    public class SampleOptions
    {
        public string StringVal { get; set; }
        public int? IntVal { get; set; }
        public bool? BoolVal { get; set; }
        public DateTime? DateVal { get; set; }
    }

    [AutoBind("MySettings", "OrangesAndBananas")]
    public class OtherSampleOptions : SampleOptions
    {
    }

    [AutoBind]
    public class OtherStuff : SampleOptions
    {
    }
}