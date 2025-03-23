using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BoardService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.type_id);
                });

            migrationBuilder.CreateTable(
                name: "boards",
                columns: table => new
                {
                    board_id = table.Column<string>(type: "text", nullable: false),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boards", x => x.board_id);
                    table.ForeignKey(
                        name: "FK_boards_Types_type_id",
                        column: x => x.type_id,
                        principalTable: "Types",
                        principalColumn: "type_id");
                });

            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    sprint_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    board_id = table.Column<string>(type: "text", nullable: false),
                    date_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.sprint_id);
                    table.ForeignKey(
                        name: "FK_Sprints_boards_board_id",
                        column: x => x.board_id,
                        principalTable: "boards",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    stage_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    board_id = table.Column<string>(type: "text", nullable: false),
                    max_tasks_count = table.Column<int>(type: "integer", nullable: true),
                    Queue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.stage_id);
                    table.ForeignKey(
                        name: "FK_Stages_boards_board_id",
                        column: x => x.board_id,
                        principalTable: "boards",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    task_id = table.Column<string>(type: "text", nullable: false),
                    stage_id = table.Column<int>(type: "integer", nullable: true),
                    board_id = table.Column<string>(type: "text", nullable: false),
                    sprint_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.task_id);
                    table.ForeignKey(
                        name: "FK_Tasks_Sprints_sprint_id",
                        column: x => x.sprint_id,
                        principalTable: "Sprints",
                        principalColumn: "sprint_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tasks_Stages_stage_id",
                        column: x => x.stage_id,
                        principalTable: "Stages",
                        principalColumn: "stage_id");
                    table.ForeignKey(
                        name: "FK_Tasks_boards_board_id",
                        column: x => x.board_id,
                        principalTable: "boards",
                        principalColumn: "board_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_boards_type_id",
                table: "boards",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_board_id",
                table: "Sprints",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "IX_Stages_board_id",
                table: "Stages",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_board_id",
                table: "Tasks",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_sprint_id",
                table: "Tasks",
                column: "sprint_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_stage_id",
                table: "Tasks",
                column: "stage_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Sprints");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "boards");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
