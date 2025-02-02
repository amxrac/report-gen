using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rgproj.Migrations
{
    /// <inheritdoc />
    public partial class AddEnvironmentalistForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnvironmentalistForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateGenerated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WitnessDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionsTaken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReported = table.Column<bool>(type: "bit", nullable: false),
                    ReportedDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowUpAction = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentalistForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvironmentalistForms_AspNetUsers_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentalistForms_SubmittedByUserId",
                table: "EnvironmentalistForms",
                column: "SubmittedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnvironmentalistForms");
        }
    }
}
