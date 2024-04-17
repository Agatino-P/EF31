using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF31;

public class UnitTest1
{
    [Fact]
    public async Task Test1Async()
    {
        MyDbContext dbContext = new MyDbContext();
        Message m1=new Message() { Id=Guid.NewGuid(), MessageId=$"MessageId {Guid.NewGuid()}"};
        await dbContext.Message.AddAsync(m1);
        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            Console.WriteLine("...");
        }

    }
}

public class MyDbContext : DbContext
{
    public DbSet<Message> Message { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=myDataBase=OSM.PaymentOrder;User Id=OSM_SA_Replica;Password=Corner123;";
        const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=OSM.PaymentOrder;Integrated Security=true;";
        optionsBuilder.UseSqlServer(ConnectionString);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }));

        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
        .ToTable("Message", schema: "InstantPayment");
        base.OnModelCreating(modelBuilder);
    }
}

public class Message
{
    public Guid Id { get; set; }
    public string MessageId { get; set; }
}