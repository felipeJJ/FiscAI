using backend.Models;
using Microsoft.EntityFrameworkCore;

public static class InvoicesEndpoints
{
    public static void MapInvoicesEndpoints(this WebApplication app)
    {
        // Endpoint GET para listar todas as notas
        app.MapGet("/invoices", async (AppDbContext db) =>
        {
            var invoices = await db.Invoices.ToListAsync();
            return Results.Ok(invoices);
        });

        // Endpoint POST para adicionar uma nova nota
        app.MapPost("/invoices", async (AppDbContext db, Invoices invoice) =>
        {
            // Gera um novo GUID se o cliente não enviar um
            if (invoice.Id == Guid.Empty)
            {
                invoice.Id = Guid.NewGuid();
            }

            db.Invoices.Add(invoice);
            await db.SaveChangesAsync();
            return Results.Created($"/invoices/{invoice.Id}", invoice);
        });
    }
}