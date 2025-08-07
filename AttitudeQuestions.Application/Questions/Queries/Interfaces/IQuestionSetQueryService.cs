using AttitudeQuestions.Domain;

namespace AttitudeQuestions.Application.Questions.Queries.Interfaces;

public interface IQuestionSetQueryService
{
    Task<QuestionSet?> Get(Guid questionSetId, CancellationToken cancellationToken);
}
