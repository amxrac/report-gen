using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rgproj.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialistForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialistForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CaseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeverityLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedDemographic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransmissionPattern = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExposureHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaboratoryFindings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntibioticResistanceObserved = table.Column<bool>(type: "bit", nullable: false),
                    RequiresNCDCNotification = table.Column<bool>(type: "bit", nullable: false),
                    ContainmentMeasures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialistComments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialistForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialistForms_AspNetUsers_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialistForms_SubmittedByUserId",
                table: "SpecialistForms",
                column: "SubmittedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialistForms");
        }
    }
}
