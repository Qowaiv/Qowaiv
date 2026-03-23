using Microsoft.EntityFrameworkCore;
using Specs.Models;
using Testcontainers.PostgreSql;

namespace Specs.Builders_specs;

public class SvoProperty
{
    [Test]
    public async Task ModelWithEmail()
    {
        var container = new PostgreSqlBuilder("postgres:15.1").Build();
        await container.StartAsync();

        var cs = container.GetConnectionString();

        var context = new ModelWithEmailContext(o =>
        {
            o.UseNpgsql(cs);
        });

        await context.Database.EnsureCreatedAsync();

        context.Models.Add(new() { Email = EmailAddress.Parse("info@qowaiv.org") });

        await context.SaveChangesAsync();


        await container.DisposeAsync();
    }
}
