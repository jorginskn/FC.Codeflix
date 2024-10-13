using DomainEntity = FC.Codeflix.Catalog.Domain;
using Xunit;
using FC.Codeflix.Catalog.Domain.Entity;
using System.Xml.Linq;

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
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
    }

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
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Entity.Category(name!, "Category Name");
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }



    [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenDescriptionIsEmpty(string? description)
    {
        Action action = () => new DomainEntity.Entity.Category("Category Name"!, null);
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Description should not be empty or null", exception.Message);
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
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreatherThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreatherThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Entity.Category(invalidName, "Category Ok Description");
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreatherThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    private void InstantiateErrorWhenDescriptionIsGreatherThan10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Entity.Category("Category Name", invalidDescription);
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
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
        Assert.True(category.IsActive);
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
        Assert.False(category.IsActive);
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

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
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

        Assert.Equal(newValues.Name, category.Name);
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
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
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
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }


    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = new DomainEntity.Entity.Category("Category Name", "Category Description");
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        Action action = () => category.Update(invalidName);
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }
    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = new DomainEntity.Entity.Category("Category Name", "Category Description");
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => category.Update("New Category name", invalidDescription);
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }

}



