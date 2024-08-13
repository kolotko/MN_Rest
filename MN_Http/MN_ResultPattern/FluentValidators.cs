using FluentValidation;

namespace MN_ResultPattern;

public class UserValidation : AbstractValidator<UserDto>
{
    public UserValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithErrorCode("543");
        RuleFor(x => x.SurName).NotEmpty();
        RuleFor(x => x.Age).GreaterThan(0);
        RuleFor(x => x.Profession)
            .SetValidator(new ProfessionValidation())
            .When(x => x.Profession != null);
    }
    
}

public class ProfessionValidation : AbstractValidator<Profession>
{
    public ProfessionValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}