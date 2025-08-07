using System.ComponentModel.DataAnnotations;

namespace AttitudeQuestions.Infrastructure.Shared.Persistence.Options;

public class RepositoryOptions()
{
    [Required(AllowEmptyStrings = false)]
    public required string PrimaryDbName { get; set; }
}
