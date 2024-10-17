using DomainEntity = FC.Codeflix.Catalog.Domain;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    public readonly CategoryTestFixture _fixture;

    public CategoryTest(CategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _fixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Entity.Category(validCategory.Name, validCategory.Description);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _fixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Entity.Category(validCategory.Name, validCategory.Description, isActive);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt >=   dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _fixture.GetValidCategory();
        Action action = () => new DomainEntity.Entity.Category(name!, validCategory.Name);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenDescriptionIsEmpty(string? description)
    {
        var validCategory = _fixture.GetValidCategory();
        Action action = () => new DomainEntity.Entity.Category(validCategory.Name!, null);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Description should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string? invalidName)
    {
        var validCategory = _fixture.GetValidCategory();
        Action action = () => new DomainEntity.Entity.Category(invalidName!, validCategory.Description);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreatherThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreatherThan255Characters()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Entity.Category(invalidName, validCategory.Description);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreatherThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    private void InstantiateErrorWhenDescriptionIsGreatherThan10_000Characters()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Entity.Category(validCategory.Name, invalidDescription);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    private void Activate()
    {
        var validCategory = _fixture.GetValidCategory();
        var category = new DomainEntity.Entity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    private void Deactivate()
    {
        var validCategory = _fixture.GetValidCategory();
        var category = new DomainEntity.Entity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var validCategory = _fixture.GetValidCategory();
        var categoryWithNewValues = _fixture.GetValidCategory();
        
        validCategory.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        validCategory.Name.Should().Be(categoryWithNewValues.Name);
        validCategory.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var validCategory = _fixture.GetValidCategory();
        var newName = _fixture.GetValidCategoryName();

        var currentDescription = validCategory.Description;
        validCategory.Update(newName, validCategory.Description);
        validCategory.Name.Should().Be(newName);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _fixture.GetValidCategory();
        Action action = () => validCategory.Update(name!);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string? invalidName)
    {
        var validCategory = _fixture.GetValidCategory();
        Action action = () => validCategory.Update(invalidName);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidName = _fixture.Faker.Lorem.Letter(256);
        Action action = () => validCategory.Update(invalidName);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidDescription = _fixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
        {
            invalidDescription = $"{invalidDescription} {_fixture.Faker.Commerce.ProductDescription()}";
        }
        Action action = () => validCategory.Update(validCategory.Name, invalidDescription);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
    }

}



