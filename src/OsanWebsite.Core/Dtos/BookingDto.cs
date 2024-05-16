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
        RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Vous n'avez pas fourni votre nom");
        RuleFor(x => x.CustomerPhone)
            .NotEmpty().WithMessage("Vous n'avez pas fourni votre numéro de téléphone")
            .Matches(@"^\d+$").WithMessage("Vous ne pouvez entrer que des chiffres");
        RuleFor(x => x.CustomerEmail)
            .NotEmpty().WithMessage("Vous n'avez pas fourni votre adresse email")
            .EmailAddress().WithMessage("Vous n'avez pas fourni une adresse email valide");
        RuleFor(x => x.TableType).NotEmpty().WithMessage("Veuillez choisir une table");
        RuleFor(x => x.Service).NotEmpty().WithMessage("Veuillez choisir un service");
        RuleFor(x => x.BookingDate).NotEmpty().WithMessage("Veuillez choisir une date");
    }
}
