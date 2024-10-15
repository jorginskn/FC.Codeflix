using DomainEntity = FC.Codeflix.Catalog.Domain;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity;
public class CategoryTest
{

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Entity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;

        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
      }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Entity.Category(validData.Name, validData.Description, isActive);
        var dateTimeAfter = DateTime.Now;
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > dateTimeBefore).Should().BeTrue();
        (category.CreatedAt < dateTimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);
     }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Entity.Category(name!, "Category Name");
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should not be empty or null");
      }



    [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenDescriptionIsEmpty(string? description)
    {
        Action action = () => new DomainEntity.Entity.Category("Category Name"!, null);
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
        Action action = () => new DomainEntity.Entity.Category(invalidName!, "Category Ok Description");
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
     }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreatherThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreatherThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Entity.Category(invalidName, "Category Ok Description");
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
     }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreatherThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    private void InstantiateErrorWhenDescriptionIsGreatherThan10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Entity.Category("Category Name", invalidDescription);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
     }


    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    private void Activate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        var category = new DomainEntity.Entity.Category(validData.Name, validData.Description, false);
        category.Activate();
        category.IsActive.Should().BeTrue();
     }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    private void Deactivate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        var category = new DomainEntity.Entity.Category(validData.Name, validData.Description, true);
        category.Deactivate();
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainEntity.Entity.Category("Category Name", "Category Description");
        var newValues = new
        {
            Name = "New Category Name",
            Description = "New Category Description"
        };
        category.Update(newValues.Name, newValues.Description);

        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
     }


    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new DomainEntity.Entity.Category("Category Name", "Category Description");
        var newValues = new
        {
            Name = "New Category Name",
         
        };
        var currentDescription = category.Description;
        category.Update(newValues.Name, category.Description);
        category.Name.Should().Be(newValues.Name);
      }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new DomainEntity.Entity.Category("Category Name", "Category Description");
        Action action = () => category.Update(name!);
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
        var category = new DomainEntity.Entity.Category("Category Name", "Category Description");
        Action action = () => category.Update(invalidName);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
     }


    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = new DomainEntity.Entity.Category("Category Name", "Category Description");
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => category.Update(invalidName);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
     }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = new DomainEntity.Entity.Category("Category Name", "Category Description");
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => category.Update("New Category name", invalidDescription);
        action.Should().Throw<DomainEntity.Exceptions.EntityValidationException>().WithMessage("Description should be less or equal 10.000 characters long");
     }

}



