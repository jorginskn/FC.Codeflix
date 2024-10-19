using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Validation;
public class DomainValidation
{
    public static void NotNull(object target, string fieldname)
    {
        if (target is null)
        {
            throw new EntityValidationException($"{fieldname} should not be null");
        }       
    }
    public static void NotNullOrEmpty(string? target, string fieldname)
    {
        if (String.IsNullOrWhiteSpace(target))
        {
            throw new EntityValidationException($"{fieldname} should not be null or empty");
        }
    }
}
