using AttitudeQuestions.Domain.Shared;

namespace AttitudeQuestions.Domain;

public class QuestionSetSession(Guid id) : AggregateRoot<Guid>(id)
{
    public List<QuestionAnswer> Answers { get; private set; } = new List<QuestionAnswer>();
}

public class QuestionAnswer(Guid questionId, int weighting)
{
    public Guid QuestionId { get; private set; } = questionId;
    public int Weighting { get; private set; } = weighting;
}