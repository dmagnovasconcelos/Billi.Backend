using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Billi.Backend.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class User_V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    password = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    redefine_password = table.Column<bool>(type: "boolean", nullable: false),
                    validation_reset_password = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    person_id = table.Column<Guid>(type: "uuid", nullable: true),
                    supplier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_system_user = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_refresh_token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_refresh_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_refresh_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_is_deleted",
                table: "user",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_user_refresh_token_user_id",
                table: "user_refresh_token",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_refresh_token");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
