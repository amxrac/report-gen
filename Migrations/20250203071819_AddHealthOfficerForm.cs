using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rgproj.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthOfficerForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthOfficerForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FacilityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanitationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicHealthRisk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiseaseVectorPresent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterQualityAssessment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WasteDisposalEvaluation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplianceStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnforcementMeasures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicHealthGuidance = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthOfficerForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthOfficerForms_AspNetUsers_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthOfficerForms_SubmittedByUserId",
                table: "HealthOfficerForms",
                column: "SubmittedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthOfficerForms");
        }
    }
}
