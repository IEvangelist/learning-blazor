// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Web.Client.Tests;

public class ChatMessageComponentTests
{
    public static IEnumerable<object[]> ChatMessageInput
    {
        get
        {
            yield return new object[]
            {
                Guid.Parse("f08b0096-5301-4f4d-8e19-6cb1514991ea"),
                "Test message... does this work?",
                "David Pine"
            };
            yield return new object[]
            {
                Guid.Parse("379b3861-0c04-49e9-8287-e5de3a40dcb3"),
                "...",
                "Fake"
            };
            yield return new object[]
            {
                Guid.Parse("f68386bb-e4d9-4fed-86b3-0fe539640b60"),
                "If a tree falls in the forest, does it make a sound?",
                null!
            };
            yield return new object[]
            {
                Guid.Parse("b19ab8b4-7819-438e-a281-56246cd3cda7"),
                null!,
                "User" };
            yield return new object[]
            {
                Guid.Parse("26ae3eae-b763-4ff1-8160-11aaad0cf078"),
                null!,
                null!
            };
        }
    }

    [
        Theory,
        MemberData(nameof(ChatMessageInput))
    ]
    public void ChatMessageComponentRendersUserAndText(
        Guid guid, string text, string user)
    {
        using var ctx = new TestContext();

        // Component parameters
        var message = new ActorMessage(
            Id: guid,
            Text: text,
            UserName: user);

        var cut =
            ctx.RenderComponent<ChatMessageComponent>(
                parameters => parameters
                    .Add(c => c.Message, message)
                    .Add(c => c.IsEditable, true)
                    .Add(c => c.EditMessage, OnEditMessage));

        cut.MarkupMatches(
            $@"<a id=""{guid}"" class=""panel-block is-size-5"">
    <span>{user}</span>
    <span class=""panel-icon px-4"">
        <i class=""fas fa-chevron-right"" aria-hidden=""true""></i>
    </span>
    <span class=""pl-2"">
        <span>{text}</span>
    </span>
</a>");
    }

    void OnEditMessage(ActorMessage _) { }
}
