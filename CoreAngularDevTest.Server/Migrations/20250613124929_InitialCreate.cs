using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreAngularDevTest.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    lat = table.Column<double>(type: "REAL", nullable: false),
                    lng = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "locationGeoInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locationGeoInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Northeast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    lat = table.Column<double>(type: "REAL", nullable: false),
                    lng = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Northeast", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Southwest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    lat = table.Column<double>(type: "REAL", nullable: false),
                    lng = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Southwest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    northeastId = table.Column<int>(type: "INTEGER", nullable: false),
                    southwestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bounds_Northeast_northeastId",
                        column: x => x.northeastId,
                        principalTable: "Northeast",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bounds_Southwest_southwestId",
                        column: x => x.southwestId,
                        principalTable: "Southwest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Viewport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    northeastId = table.Column<int>(type: "INTEGER", nullable: false),
                    southwestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viewport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Viewport_Northeast_northeastId",
                        column: x => x.northeastId,
                        principalTable: "Northeast",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Viewport_Southwest_southwestId",
                        column: x => x.southwestId,
                        principalTable: "Southwest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Geometry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BoundsId = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Location_type = table.Column<string>(type: "TEXT", nullable: true),
                    ViewportId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geometry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Geometry_Bounds_BoundsId",
                        column: x => x.BoundsId,
                        principalTable: "Bounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Geometry_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Geometry_Viewport_ViewportId",
                        column: x => x.ViewportId,
                        principalTable: "Viewport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    formatted_address = table.Column<string>(type: "TEXT", nullable: true),
                    geometryId = table.Column<int>(type: "INTEGER", nullable: false),
                    place_id = table.Column<string>(type: "TEXT", nullable: true),
                    types = table.Column<string>(type: "TEXT", nullable: false),
                    LocationGeoInfoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Result_Geometry_geometryId",
                        column: x => x.geometryId,
                        principalTable: "Geometry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Result_locationGeoInfo_LocationGeoInfoId",
                        column: x => x.LocationGeoInfoId,
                        principalTable: "locationGeoInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AddressComponent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Long_name = table.Column<string>(type: "TEXT", nullable: true),
                    Short_name = table.Column<string>(type: "TEXT", nullable: true),
                    Types = table.Column<string>(type: "TEXT", nullable: false),
                    ResultId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressComponent_Result_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Result",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressComponent_ResultId",
                table: "AddressComponent",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Bounds_northeastId",
                table: "Bounds",
                column: "northeastId");

            migrationBuilder.CreateIndex(
                name: "IX_Bounds_southwestId",
                table: "Bounds",
                column: "southwestId");

            migrationBuilder.CreateIndex(
                name: "IX_Geometry_BoundsId",
                table: "Geometry",
                column: "BoundsId");

            migrationBuilder.CreateIndex(
                name: "IX_Geometry_LocationId",
                table: "Geometry",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Geometry_ViewportId",
                table: "Geometry",
                column: "ViewportId");

            migrationBuilder.CreateIndex(
                name: "IX_Result_geometryId",
                table: "Result",
                column: "geometryId");

            migrationBuilder.CreateIndex(
                name: "IX_Result_LocationGeoInfoId",
                table: "Result",
                column: "LocationGeoInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Viewport_northeastId",
                table: "Viewport",
                column: "northeastId");

            migrationBuilder.CreateIndex(
                name: "IX_Viewport_southwestId",
                table: "Viewport",
                column: "southwestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressComponent");

            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropTable(
                name: "Geometry");

            migrationBuilder.DropTable(
                name: "locationGeoInfo");

            migrationBuilder.DropTable(
                name: "Bounds");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Viewport");

            migrationBuilder.DropTable(
                name: "Northeast");

            migrationBuilder.DropTable(
                name: "Southwest");
        }
    }
}
