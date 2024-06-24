using FluentValidation;

namespace OsanWebsite.Core.Dtos;

public class BookingDto
{
    public string? Id { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public DateTime? BookingDate { get; set; } = default!;
    public string? Service { get; set; } = default!;
    public string? TableType { get; set; } = default!;
}

public class BookingDtoValidator : AbstractValidator<BookingDto>
{
    public BookingDtoValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Le nom est requis");
        RuleFor(x => x.CustomerPhone)
            .NotEmpty().WithMessage("Le numéro de téléphone est requis")
            .Matches(@"^\d+$").WithMessage("Uniquement des chiffres");
        RuleFor(x => x.CustomerEmail)
            .NotEmpty().WithMessage("L'adresse email est requise")
            .EmailAddress().WithMessage("Adresse email invalide");
        RuleFor(x => x.TableType).NotEmpty().WithMessage("Veuillez choisir une table");
        RuleFor(x => x.Service).NotEmpty().WithMessage("Veuillez choisir un service");
        RuleFor(x => x.BookingDate).NotEmpty().WithMessage("Veuillez choisir une date");
    }
}
