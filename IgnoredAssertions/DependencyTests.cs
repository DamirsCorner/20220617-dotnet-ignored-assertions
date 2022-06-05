using FluentAssertions;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace IgnoredAssertions;

public class DependencyTests
{
    [Test]
    public void FailsWithFailedMockVerification()
    {
        var mocker = new AutoMocker();
        var service = mocker.CreateInstance<Service>();

        var dependency = mocker.GetMock<IDependency>();
        dependency
            .Setup(d => d.DependencyCall(It.Is<int>(input => input < 5)));

        service.MethodCall(6);
        mocker.VerifyAll();
    }

    [Test]
    public void PassesWithFailedAssertionInCallback()
    {
        var mocker = new AutoMocker();
        var service = mocker.CreateInstance<Service>();

        var dependency = mocker.GetMock<IDependency>();
        dependency
            .Setup(d => d.DependencyCall(It.IsAny<int>()))
            .Callback((int input) =>
            {
                input.Should().BeLessThan(5);
            });

        service.MethodCall(6);
        mocker.VerifyAll();
    }

    [Test]
    public void FailsWithFailedAssertionOutsideCallback()
    {
        var mocker = new AutoMocker();
        var service = mocker.CreateInstance<Service>();

        var dependency = mocker.GetMock<IDependency>();
        int? actualInput = null;
        dependency
            .Setup(d => d.DependencyCall(It.IsAny<int>()))
            .Callback((int input) =>
            {
                actualInput = input;
            });

        service.MethodCall(6);

        actualInput.Should().BeLessThan(5);
        mocker.VerifyAll();
    }
}