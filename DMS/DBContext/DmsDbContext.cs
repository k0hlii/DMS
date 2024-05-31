using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

public class DmsDbContext : DbContext
{
    public DbSet<Developer> Developers { get; init; }
    public DbSet<Project> Projects { get; init;}
    public DbSet<Technologie> Technologies { get; init; }

    public static DmsDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<DmsDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);
    
    public DmsDbContext ()
    {}

    public DmsDbContext(DbContextOptions options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Developer>().ToCollection("Developer");
        modelBuilder.Entity<Project>().ToCollection("Project");
        modelBuilder.Entity<Technologie>().ToCollection("Technologie");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var mongoClient = new MongoClient("mongodb+srv://admin:admin1@dmsdb.2pfyaeb.mongodb.net/?retryWrites=true&w=majority&appName=DMSDB");
        optionsBuilder.UseMongoDB(mongoClient, "DMSDB");
    }
}

public class Project
{
    public ObjectId _id { get; set; }

    public string IdAsString
    {
        get { return _id.ToString(); }
    }

    public string name { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string ProjektmitarbeiterID { get; set; }
    public string status { get; set; }
}

public class Developer
{
    public ObjectId _id { get; set; }

    public string IdAsString
    {
        get { return _id.ToString(); }
    }

    public string firstname { get; set; }
    public string lastname { get; set; }
    public string field { get; set; }
    public string ContactID { get; set; }
}

public class Technologie
{
    public ObjectId _id { get; set; }

    public string IdAsString
    {
        get { return _id.ToString(); }
    }
    
    public string name { get; set; }
    public string description { get; set; }
    public string Usage { get; set; }
}