using FluentAssertions;
using PetFamily.Discussions.Domain.AggregateRoot;
using PetFamily.Discussions.Domain.Entities;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.SharedKernel.IDs;

namespace PetFamily.Discussions.Domain.UnitTests;

public class DiscussionsTests
{
    [Fact]
    public void Create_Discussion_Should_Return_Success_Result()
    {
        // arrange
        var relationId = Guid.NewGuid();
        List<Guid> users = [Guid.NewGuid(), Guid.NewGuid()];

        // act
        var discussionResult = Discussion.Create(relationId, users);

        // assert
        discussionResult.IsSuccess.Should().BeTrue();
        discussionResult.Value.Users.Should().BeEquivalentTo(users);
        discussionResult.Value.RelationId.Should().Be(relationId);
        discussionResult.Value.IsActive.Should().BeTrue();
        discussionResult.Value.Messages.Should().BeEmpty();
    }

    [Fact]
    public void Create_Discussion_Should_Return_Error_Result_If_Large_Number_Of_Users()
    {
        // arrange
        var relationId = Guid.NewGuid();
        List<Guid> users = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        // act
        var discussionResult = Discussion.Create(relationId, users);

        // assert
        discussionResult.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Add_Comment_Should_Return_Success_Result()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();
        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first user's message").Value;
        var message = Message.Create(firstUser, text).Value;

        // act
        var result = discussion.AddComment(firstUser, message);

        // assert
        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().BeEquivalentTo(new[] { message });
    }

    [Fact]
    public void Add_Comment_Should_Return_Error_Result_If_User_Not_In_Discussion()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();
        var anotherUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first user's message").Value;
        var message = Message.Create(anotherUser, text).Value;

        // act
        var result = discussion.AddComment(anotherUser, message);

        // assert
        result.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().BeEmpty();
    }

    [Fact]
    public void Delete_Comment_Should_Return_Success_Result()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first user's message").Value;
        var message = Message.Create(firstUser, text).Value;
        discussion.AddComment(firstUser, message);

        // act
        var messageId = message.Id;
        var result = discussion.DeleteComment(firstUser, messageId);

        // assert
        result.IsSuccess.Should().BeTrue();
        discussion.Messages.Should().BeEmpty();
    }

    [Fact]
    public void Delete_Comment_Should_Return_Error_Result_If_User_Is_Not_Own_This_Comment()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first user's message").Value;
        var message = Message.Create(firstUser, text).Value;
        discussion.AddComment(firstUser, message);

        // act
        var messageId = message.Id;
        var result = discussion.DeleteComment(secondUser, messageId);

        // assert
        result.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().BeEquivalentTo(new[] { message });
    }
    
    [Fact]
    public void Delete_Comment_Should_Return_Error_Result_If_Comment_Not_Found()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first user's message").Value;
        var message = Message.Create(firstUser, text).Value;
        discussion.AddComment(firstUser, message);

        // act
        var messageId = MessageId.NewMessageId();
        var result = discussion.DeleteComment(secondUser, messageId);

        // assert
        result.IsSuccess.Should().BeFalse();
        discussion.Messages.Should().BeEquivalentTo(new[] { message });
    }
    
    [Fact]
    public void Update_Comment_Should_Return_Success_Result()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first message").Value;
        var message = Message.Create(firstUser, text).Value;
        discussion.AddComment(firstUser, message);
        
        var anotherText = Text.Create("second message").Value;

        // act
        var messageId = message.Id;
        var result = discussion.UpdateComment(messageId, firstUser, anotherText);

        // assert
        result.IsSuccess.Should().BeTrue();
        message.Text.Should().Be(anotherText);
        message.IsEdited.Should().BeTrue();
        discussion.Messages.Should().BeEquivalentTo(new[] { message });
    }
    
    [Fact]
    public void Update_Comment_Should_Return_Error_Result_If_Comment_Not_Found()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first message").Value;
        var message = Message.Create(firstUser, text).Value;
        discussion.AddComment(firstUser, message);
        
        var anotherText = Text.Create("second message").Value;

        // act
        var messageId = MessageId.NewMessageId();
        var result = discussion.UpdateComment(messageId, firstUser, anotherText);

        // assert
        result.IsSuccess.Should().BeFalse();
        message.Text.Should().Be(text);
        message.IsEdited.Should().BeFalse();
        discussion.Messages.Should().BeEquivalentTo(new[] { message });
    }
    
    [Fact]
    public void Update_Comment_Should_Return_Error_Result_If_User_Is_Not_Own_This_Comment()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first message").Value;
        var message = Message.Create(firstUser, text).Value;
        discussion.AddComment(firstUser, message);
        
        var anotherText = Text.Create("second message").Value;

        // act
        var messageId = MessageId.NewMessageId();
        var result = discussion.UpdateComment(messageId, secondUser, anotherText);

        // assert
        result.IsSuccess.Should().BeFalse();
        message.Text.Should().Be(text);
        message.IsEdited.Should().BeFalse();
        discussion.Messages.Should().BeEquivalentTo(new[] { message });
    }
    
    [Fact]
    public void Update_Comment_Should_Return_Error_Result_If_User_Not_In_Discussion()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();
        var anotherUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;
        var text = Text.Create("first message").Value;
        var message = Message.Create(firstUser, text).Value;
        discussion.AddComment(firstUser, message);
        
        var anotherText = Text.Create("second message").Value;

        // act
        var messageId = MessageId.NewMessageId();
        var result = discussion.UpdateComment(messageId, anotherUser, anotherText);

        // assert
        result.IsSuccess.Should().BeFalse();
        message.Text.Should().Be(text);
        message.IsEdited.Should().BeFalse();
        discussion.Messages.Should().BeEquivalentTo(new[] { message });
    }
    
    [Fact]
    public void Close_Discussion_Should_Return_Success_Result()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;

        // act
        var closeResult = discussion.CloseDiscussion(firstUser);

        // assert
        closeResult.IsSuccess.Should().BeTrue();
        discussion.IsActive.Should().BeFalse();
    }
    
        
    [Fact]
    public void Close_Discussion_Should_Return_Error_Result_If_User_Not_In_Discussion()
    {
        // arrange
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();
        var anotherUser = Guid.NewGuid();

        var relationId = Guid.NewGuid();

        List<Guid> users = [firstUser, secondUser];
        var discussion = Discussion.Create(relationId, users).Value;

        // act
        var closeResult = discussion.CloseDiscussion(anotherUser);

        // assert
        closeResult.IsSuccess.Should().BeFalse();
        discussion.IsActive.Should().BeTrue();
    }
}