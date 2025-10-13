using FluentValidation;

namespace Expenses.App.API.Dtos.Categories.Validators;

public sealed class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Nama kategori minimal 3 hingga maksimal 100 karakter");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null)
            .WithMessage("Deskripsi tidak boleh melebihi 50 karakter");
    }
}
