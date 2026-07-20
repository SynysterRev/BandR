using BandR.Entities;
using BandR.Extensions;
using FluentAssertions;

namespace BandR.Tests.Unit.Extensions;

public class ConversationExtensionsTests
{
    [Fact]
    public void ToDto_ShouldExposeActiveState()
    {
        var conversation = new Conversation
        {
            IsActive = false
        };

        var result = conversation.ToDto();

        result.IsActive.Should().BeFalse();
    }
}
