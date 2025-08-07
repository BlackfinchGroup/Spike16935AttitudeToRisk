using AttitudeQuestions.Application.Questions.Queries.Interfaces;
using AttitudeQuestions.Domain;
using AttitudeQuestions.Infrastructure.Shared.Persistence;

namespace AttitudeQuestions.Infrastructure.Questions;

internal class QuestionSetQueryService(QuestionDbContext dbContext) :
    BaseQueryRepository<QuestionSet>(dbContext),
    IQuestionSetQueryService
{
}
