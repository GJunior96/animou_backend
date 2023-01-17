using Animou.Business.Models;
using FluentValidation;

namespace Animou.Business.Validations
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("required")
                .Length(2, 50).WithMessage("length");

            RuleFor(u => u.Email!.Address)
                .NotEmpty().WithMessage("required")
                .Length(5, 100).WithMessage("length")
                .Must(ValidateEmail!).WithMessage("invalid");
        }

        protected static bool ValidateEmail(string email) => Email.Validate(email);
    }

    public class CommentValidation : AbstractValidator<Comment>
    {
        public CommentValidation()
        {
            RuleFor(c => c.MediaId)
                .NotEmpty().WithMessage("required");

            RuleFor(c => c.Text)
                .Length(1, 1000).WithMessage("length");

            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty).WithMessage("invalid");
        }
    }

    public class LikeValidation : AbstractValidator<Like>
    {
        public LikeValidation()
        {
            RuleFor(l => l.UserId)
                .NotEqual(Guid.Empty).WithMessage("invalid");

            RuleFor(l => l.CommentId)
                .NotEqual(Guid.Empty).WithMessage("invalid");
        }
    }

    public class DislikeValidation : AbstractValidator<Dislike>
    {
        public DislikeValidation()
        {
            RuleFor(l => l.UserId)
                .NotEqual(Guid.Empty).WithMessage("invalid");

            RuleFor(l => l.CommentId)
                .NotEqual(Guid.Empty).WithMessage("invalid");
        }
    }
}
