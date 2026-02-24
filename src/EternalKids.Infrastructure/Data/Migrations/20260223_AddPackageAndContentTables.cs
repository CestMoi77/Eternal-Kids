using Microsoft.EntityFrameworkCore.Migrations;

namespace EternalKids.Infrastructure.Data.Migrations;

public partial class AddPackageAndContentTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EternalKids_PackageDefinition",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                PackageType = table.Column<string>(maxLength: 50, nullable: false),
                Title = table.Column<string>(maxLength: 120, nullable: false),
                Description = table.Column<string>(maxLength: 500, nullable: false),
                FeaturesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsFeatured = table.Column<bool>(nullable: false),
                IsActive = table.Column<bool>(nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_EternalKids_PackageDefinition", x => x.Id); });

        migrationBuilder.CreateTable(
            name: "EternalKids_SiteContentBlock",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                PageKey = table.Column<string>(maxLength: 50, nullable: false),
                SectionKey = table.Column<string>(maxLength: 50, nullable: false),
                Title = table.Column<string>(maxLength: 200, nullable: false),
                Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_EternalKids_SiteContentBlock", x => x.Id); });

        migrationBuilder.CreateIndex(
            name: "IX_EternalKids_PackageDefinition_PackageType",
            table: "EternalKids_PackageDefinition",
            column: "PackageType",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_EternalKids_SiteContentBlock_PageKey_SectionKey",
            table: "EternalKids_SiteContentBlock",
            columns: new[] { "PageKey", "SectionKey" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "EternalKids_PackageDefinition");
        migrationBuilder.DropTable(name: "EternalKids_SiteContentBlock");
    }
}
