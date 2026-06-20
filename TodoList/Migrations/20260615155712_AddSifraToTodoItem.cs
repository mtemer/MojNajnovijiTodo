using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoList.Migrations
{
    /// <inheritdoc />
    public partial class AddSifraToTodoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sifra",
                table: "Todos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sifra",
                table: "Todos");
        }
    }
}
