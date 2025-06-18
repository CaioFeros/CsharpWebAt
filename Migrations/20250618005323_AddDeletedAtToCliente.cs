using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsharpWebAt.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedAtToCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Clientes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Clientes");
        }
    }
}
