using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateCrm.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialEventStoreAndReadModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers_read_model",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CpfCnpj = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BirthOrFoundationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StateRegistration = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsStateRegistrationExempt = table.Column<bool>(type: "boolean", nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Neighborhood = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers_read_model", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "event_store_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    AggregateType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EventType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    EventVersion = table.Column<int>(type: "integer", nullable: false),
                    EventData = table.Column<string>(type: "jsonb", nullable: false),
                    Metadata = table.Column<string>(type: "jsonb", nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_store_events", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ux_customers_cpf_cnpj",
                table: "customers_read_model",
                column: "CpfCnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_customers_email",
                table: "customers_read_model",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_event_store_aggregate_id",
                table: "event_store_events",
                column: "AggregateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers_read_model");

            migrationBuilder.DropTable(
                name: "event_store_events");
        }
    }
}
