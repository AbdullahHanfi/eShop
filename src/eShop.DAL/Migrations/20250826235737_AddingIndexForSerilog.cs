using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingIndexForSerilog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE UNIQUE INDEX MostUsedColumn_TimeStamp 
            ON Logs (TimeStamp DESC, Id)
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP INDEX MostUsedColumn_TimeStamp ON Logs
        ");
        }
    }
}
