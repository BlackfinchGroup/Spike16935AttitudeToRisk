using AttitudeQuestions.Domain;
using AttitudeQuestions.Infrastructure.Shared.Persistence.Options;
using AttitudeQuestions.Infrastructure.Shared.Persistence.Options.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace AttitudeQuestions.Infrastructure.Shared.Persistence;

public class QuestionDbContext(IMongoClient mongoClient, IOptions<RepositoryOptions> repositoryOptions, IHostEnvironment environment)
    : BaseDbContext<QuestionDbContext>(mongoClient, repositoryOptions, environment)
{
    public DbSet<QuestionSetSession> QuestionSetSessions { get; init; }
    public DbSet<QuestionSet> QuestionSets { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<QuestionSetSession>().ToCollection("question-set-sessions").AddAuditProperties();
        modelBuilder.Entity<QuestionSet>().ToCollection("question-sets").AddAuditProperties();
    }
}
