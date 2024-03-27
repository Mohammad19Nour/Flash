using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectP.Migrations
{
    /// <inheritdoc />
    public partial class places : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TouristPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArabicName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EnglishName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ArabicDescription = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    EnglishDescription = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouristPlaces_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TouristPlaces_LocationId",
                table: "TouristPlaces",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TouristPlaces");
        }
    }
}
