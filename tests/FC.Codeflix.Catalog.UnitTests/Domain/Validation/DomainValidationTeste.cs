using FC.Codeflix.Catalog.Domain.Validation;
using Bogus;
using FluentAssertions;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;
public class DomainValidationTeste
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
        Action action = () => DomainValidation.NotNull(value, "Value");
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string value = null;
        string fieldName = Faker.Commerce.ProductName().Replace("", "");
        Action action = () => DomainValidation.NotNull(value, fieldName);
        action.Should().Throw<EntityValidationException>()
                       .WithMessage("Fieldname should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        string fieldName = Faker.Commerce.ProductName().Replace("", "");

        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be null or empty");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        string fieldName = Faker.Commerce.ProductName().Replace("", "");

        var target = Faker.Commerce.ProductName();
        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
        action.Should().NotThrow();
    }
    //tamanho minimo 

    [Theory(DisplayName = nameof(MinLenghtThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValueSmallerThanTheMin), parameters: 10)]
    public void MinLenghtThrowWhenLess(string target, int minLenght)
    {
        string fieldName = Faker.Commerce.ProductName().Replace("", "");
        Action action = () => DomainValidation.MinLength(target, minLenght,fieldName);
        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be less than {minLenght} characters long");
    }



    [Theory(DisplayName = nameof(MaxLenghtThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValueGreaterThanMax), parameters: 10)]
    public void MaxLenghtThrowWhenGreater(string target, int maxLenght)
    {
        string fieldName = Faker.Commerce.ProductName().Replace("", "");
        Action action = () => DomainValidation.MaxLenght(target, maxLenght, fieldName);
        action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be greater than {maxLenght} characters long");
    }

    [Theory(DisplayName = nameof(MaxLenghtOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValueLessThanMax), parameters: 10)]
    public void MaxLenghtOk(string target, int maxLenght)
    {
        string fieldName = Faker.Commerce.ProductName().Replace("", "");

        Action action = () => DomainValidation.MaxLenght(target, maxLenght, fieldName);
        action.Should().NotThrow<EntityValidationException>();
    }
    public static IEnumerable<object[]> GetValueSmallerThanTheMin(int numberOfTests)
    {
        yield return new object[] { "123456", 10 };
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random().Next(1, 20));
            yield return new object[] { minLength, example };
        }
    }

    public static IEnumerable<object[]> GetValueLessThanMax(int numberOfTests)
    {
        yield return new object[] { "123456", 5 };
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + (new Random().Next(1, 5));
            yield return new object[] { example, maxLength };
        }
    } 

    public static IEnumerable<object[]> GetValueGreaterThanMax(int numberOfTests)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - (new Random().Next(1, 5));
            yield return new object[] { example, maxLength  };
        }
    }



}
