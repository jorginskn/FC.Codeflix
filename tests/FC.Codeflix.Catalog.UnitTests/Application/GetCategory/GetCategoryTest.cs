using FluentAssertions;
using Moq;
using Xunit;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;


[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;
    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - Use cases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetValidCategory();
        repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCategory);
        var input = new UseCase.GetCategoryInput(exampleCategory.Id);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);
        var output = await useCase.Handle(input, CancellationToken.None);
        repositoryMock.Verify(x => x.GetById(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Id.Should().Be(exampleCategory.Id);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesnttExits))]
    [Trait("Application", "GetCategory - Use cases")]
    public async Task NotFoundExceptionWhenCategoryDoesnttExits()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();
        repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()
            )).ThrowsAsync(new NotFoundException(
                $"Category '{exampleGuid}' not found"
            ));
        var input = new UseCase.GetCategoryInput(exampleGuid);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);
        var task = () => useCase.Handle(input, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.GetById(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
}