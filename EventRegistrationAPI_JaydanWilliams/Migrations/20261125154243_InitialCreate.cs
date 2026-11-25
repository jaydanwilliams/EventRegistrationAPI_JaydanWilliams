using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventRegistrationAPI_JaydanWilliams.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.RegistrationId);
                    table.ForeignKey(
                        name: "FK_Registrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "Capacity", "EventDate", "Location", "Title" },
                values: new object[,]
                {
                    { 1, 200, new DateTime(2025, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), "Trafalgar Campus, Main Hall", "Fall Orientation 2025" },
                    { 2, 30, new DateTime(2027, 2, 15, 14, 0, 0, 0, DateTimeKind.Unspecified), "Davis Campus, Room B210", "Intro to Cloud Computing Workshop" },
                    { 3, 2, new DateTime(2027, 3, 10, 13, 0, 0, 0, DateTimeKind.Unspecified), "HMC Campus, Career Centre", "Resume Review Session" },
                    { 4, 100, new DateTime(2027, 4, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "Trafalgar Campus, Library Commons", "Spring Hackathon" }
                });

            migrationBuilder.InsertData(
                table: "Registrations",
                columns: new[] { "RegistrationId", "Email", "EventId", "RegisteredOn", "StudentName" },
                values: new object[,]
                {
                    { 1, "priya.sharma@sheridancollege.ca", 2, new DateTime(2026, 11, 1, 9, 30, 0, 0, DateTimeKind.Unspecified), "Priya Sharma" },
                    { 2, "marcus.lee@sheridancollege.ca", 3, new DateTime(2026, 11, 2, 11, 0, 0, 0, DateTimeKind.Unspecified), "Marcus Lee" },
                    { 3, "sofia.alvarez@sheridancollege.ca", 3, new DateTime(2026, 11, 3, 15, 45, 0, 0, DateTimeKind.Unspecified), "Sofia Alvarez" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EventId",
                table: "Registrations",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
