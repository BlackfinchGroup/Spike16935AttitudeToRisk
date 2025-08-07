namespace AttitudeQuestions.AppHost.MongoDBAtlas;

/// <summary>
/// A resource that represents a MongoDB Atlas database. This is a child resource of a <see cref="MongoDBAtlasServerResource"/>.
/// </summary>
public class MongoDBAtlasDatabaseResource : Resource, IResourceWithParent<MongoDBAtlasServerResource>, IResourceWithConnectionString
{
    /// <summary>
    /// A resource that represents a MongoDB Atlas database. This is a child resource of a <see cref="MongoDBAtlasServerResource"/>.
    /// </summary>
    /// <param name="name">The name of the resource.</param>
    /// <param name="databaseName">The database name.</param>
    /// <param name="parent">The MongoDB Atlas server resource associated with this database.</param>
    public MongoDBAtlasDatabaseResource(string name, string databaseName, MongoDBAtlasServerResource parent) : base(name)
    {
        ArgumentNullException.ThrowIfNull(databaseName);
        ArgumentNullException.ThrowIfNull(parent);

        Parent = parent;
        DatabaseName = databaseName;
    }

    /// <summary>
    /// Gets the connection string expression for the MongoDB Atlas database.
    /// </summary>
    public ReferenceExpression ConnectionStringExpression => Parent.BuildConnectionString(DatabaseName);

    /// <summary>
    /// Gets the parent MongoDB container resource.
    /// </summary>
    public MongoDBAtlasServerResource Parent { get; }

    /// <summary>
    /// Gets the database name.
    /// </summary>
    public string DatabaseName { get; }
}
