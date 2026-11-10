using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exposure.Migrations
{
    /// <inheritdoc />
    public partial class InitialExposure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "credit_limits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_limits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "credit_exposures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditOperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditLimitId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OutstandingAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_exposures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_credit_exposures_credit_limits_CreditLimitId",
                        column: x => x.CreditLimitId,
                        principalTable: "credit_limits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exposure_movements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditExposureId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PreviousBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exposure_movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exposure_movements_credit_exposures_CreditExposureId",
                        column: x => x.CreditExposureId,
                        principalTable: "credit_exposures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_credit_exposures_CreditLimitId",
                table: "credit_exposures",
                column: "CreditLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_exposures_CreditOperationId",
                table: "credit_exposures",
                column: "CreditOperationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_credit_exposures_Status",
                table: "credit_exposures",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_credit_limits_IsActive",
                table: "credit_limits",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_credit_limits_Name",
                table: "credit_limits",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_exposure_movements_CreditExposureId_CreatedAt",
                table: "exposure_movements",
                columns: new[] { "CreditExposureId", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exposure_movements");

            migrationBuilder.DropTable(
                name: "credit_exposures");

            migrationBuilder.DropTable(
                name: "credit_limits");
        }
    }
}
