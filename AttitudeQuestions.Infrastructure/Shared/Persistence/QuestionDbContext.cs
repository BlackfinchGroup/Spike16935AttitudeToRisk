using AttitudeQuestions.Infrastructure.Shared.Persistence.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AttitudeQuestions.Infrastructure.Shared.Persistence;

public class QuestionDbContext(IMongoClient mongoClient, IOptions<RepositoryOptions> repositoryOptions, IHostEnvironment environment)
    : BaseDbContext<QuestionDbContext>(mongoClient, repositoryOptions, environment)
{

}
