using DomainEntity = FC.Codeflix.Catalog.Domain;
using Xunit;
 
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
        Assert.Equal(isActive,category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Entity.Category(name!, "Category Description");
        var exception = Assert.Throws<DomainEntity.Exceptions.EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

}



