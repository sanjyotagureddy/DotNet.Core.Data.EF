using Shouldly;
using Xbehave;

namespace DotNet.Core.Data.EF.Test.Tests
{
  public class StubTest
  {
    [Scenario]
    public void Stub()
    {
      "Stub test"
          .x(() => true.ShouldBe(true));
    }
  }
}
