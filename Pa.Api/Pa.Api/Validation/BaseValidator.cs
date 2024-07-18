using FluentValidation;
using Pa.Base.Entity;

namespace Pa.Api.Validation
{
    public class BaseValidator<T> : AbstractValidator<T> where T : BaseEntity
    {
        public BaseValidator()
        {
            // Commented out for now, as it is not required
            //RuleFor(x => x.Id).GreaterThan(0);
            //RuleFor(x => x.IsActive).NotNull();
            //RuleFor(x => x.CreateUserId).GreaterThan(0);
            //RuleFor(x => x.UpdateUserId).GreaterThan(0);
            //RuleFor(x => x.DeleteUserId).GreaterThan(0);
            //RuleFor(x => x.CreateTime).NotNull();
            //RuleFor(x => x.UpdateTime).NotNull();
            //RuleFor(x => x.DeleteTime).NotNull();
        }
    }
}

