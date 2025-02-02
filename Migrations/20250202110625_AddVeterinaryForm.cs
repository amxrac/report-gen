using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rgproj.Migrations
{
    /// <inheritdoc />
    public partial class AddVeterinaryForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateGenerated",
                table: "EnvironmentalistForms",
                newName: "DateSubmitted");

            migrationBuilder.CreateTable(
                name: "VeterinaryForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AnimalSpecies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaccinationDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClinicalSymptoms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreliminaryDiagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PotentialZoonoticRisk = table.Column<bool>(type: "bit", nullable: false),
                    SuspectedDisease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuarantineRecommended = table.Column<bool>(type: "bit", nullable: false),
                    FollowUpProtocol = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeterinaryForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeterinaryForms_AspNetUsers_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VeterinaryForms_SubmittedByUserId",
                table: "VeterinaryForms",
                column: "SubmittedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VeterinaryForms");

            migrationBuilder.RenameColumn(
                name: "DateSubmitted",
                table: "EnvironmentalistForms",
                newName: "DateGenerated");
        }
    }
}
