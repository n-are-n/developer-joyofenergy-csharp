namespace JOIEnergy.Validation;
public interface IValidationRule<T>
{
    void Validate(T entity);
}