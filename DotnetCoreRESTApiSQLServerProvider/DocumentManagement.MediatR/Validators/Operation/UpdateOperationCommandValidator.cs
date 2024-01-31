using DocumentManagement.MediatR.Commands;
using FluentValidation;
using System;

namespace DocumentManagement.MediatR.Validators
{
    public class UpdateOperationCommandValidator : AbstractValidator<UpdateOperationCommand>
    {
        public UpdateOperationCommandValidator()
        {
            RuleFor(c => c.Id).Must(NotEmptyGuid).WithMessage("Id is required");
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        }

        private bool NotEmptyGuid(Guid p)
        {
            return p != Guid.Empty;
        }
    }
}
