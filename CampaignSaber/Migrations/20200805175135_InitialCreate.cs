using System;
using CampaignSaber.Models;
using CampaignSaber.Models.Discord;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CampaignSaber.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    role = table.Column<int>(nullable: false),
                    profile = table.Column<DiscordUser>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "campaigns",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    hash = table.Column<string>(nullable: false),
                    title = table.Column<string>(nullable: false),
                    cover_url = table.Column<string>(nullable: true),
                    uploaded = table.Column<DateTime>(nullable: false),
                    uploader_id = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    download_url = table.Column<string>(nullable: false),
                    stats = table.Column<CampaignStats>(type: "jsonb", nullable: false),
                    direct_download_url = table.Column<string>(nullable: false),
                    metadata = table.Column<CampaignMetadata>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campaigns", x => x.id);
                    table.ForeignKey(
                        name: "fk_campaigns_users_uploader_id",
                        column: x => x.uploader_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_campaigns_uploader_id",
                table: "campaigns",
                column: "uploader_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "campaigns");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
