using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingVisitorTracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    IpAddress = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.IpAddress);
                });

            migrationBuilder.CreateTable(
                name: "VisitorHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    QueryString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referrer = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitorIpAddress = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorHistory_Visitors_VisitorIpAddress",
                        column: x => x.VisitorIpAddress,
                        principalTable: "Visitors",
                        principalColumn: "IpAddress",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorHistory_VisitorIpAddress",
                table: "VisitorHistory",
                column: "VisitorIpAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitorHistory");

            migrationBuilder.DropTable(
                name: "Visitors");
        }
    }
}
