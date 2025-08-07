using AttitudeQuestions.Domain;
using AttitudeQuestions.Infrastructure.Shared.Persistence;

namespace AttitudeQuestions.Infrastructure.QuestionSetSessions;

public class QuestionSetSessionRepository(QuestionDbContext dBContext) : BaseCrudRepository<QuestionSetSession>(dBContext)
{
}
