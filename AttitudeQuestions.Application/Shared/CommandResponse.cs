namespace AttitudeQuestions.Application.Shared;

public record struct CommandResponse(Guid Id)
{
    private static readonly CommandResponse _default = new(Guid.Empty);

    public static ref readonly CommandResponse Default => ref _default;

    public static CommandResponse From(Guid id) => new(id);
}
