using Microsoft.EntityFrameworkCore.Migrations;

namespace CourierApplication.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adresses",
                columns: table => new
                {
                    AdressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresses", x => x.AdressId);
                });

            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    CourierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    isFree = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.CourierId);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Clients_Adresses_AdressId",
                        column: x => x.AdressId,
                        principalTable: "Adresses",
                        principalColumn: "AdressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdressId = table.Column<int>(nullable: false),
                    isCompleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Adresses_AdressId",
                        column: x => x.AdressId,
                        principalTable: "Adresses",
                        principalColumn: "AdressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Adresses",
                columns: new[] { "AdressId", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 1, 51.117193m, 17.044819m, "Sienkiewicza 13/4" },
                    { 2, 51.114836m, 17.068532m, "Grunwaldzka 71/4" },
                    { 3, 51.114899m, 17.001978m, "Legnicka 42/18" },
                    { 4, 51.123066m, 17.049608m, "Nowowiejska 31/10" },
                    { 5, 51.098380m, 16.936670m, "Rakietowa 20/9" },
                    { 6, 51.065627m, 16.957833m, "Targowa 87/11" },
                    { 7, 51.119083m, 16.978771m, "Bystrzycka 64/3" },
                    { 8, 51.124014m, 16.960451m, "Bajana 82/16" },
                    { 9, 51.069190m, 17.040798m, "Terenowa 8/2" },
                    { 10, 51.135035m, 16.973569m, "Kolista 32/5" },
                    { 11, 51.091435m, 17.008427m, "Pretficza 27/19" },
                    { 12, 51.1259106m, 16.9693086m, "Drzewieckiego 24/29" }
                });

            migrationBuilder.InsertData(
                table: "Couriers",
                columns: new[] { "CourierId", "FirstName", "LastName", "isFree" },
                values: new object[,]
                {
                    { 1, "Marcin", "Kowalski", true },
                    { 2, "Tomasz", "Nowak", true },
                    { 3, "Tomasz", "Gajda", false }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "AdressId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 11, 11 },
                    { 10, 10 },
                    { 9, 9 },
                    { 8, 8 },
                    { 12, 12 },
                    { 6, 6 },
                    { 5, 5 },
                    { 7, 7 },
                    { 3, 3 },
                    { 4, 4 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "AdressId", "isCompleted" },
                values: new object[,]
                {
                    { 9, 11, false },
                    { 7, 10, false },
                    { 6, 10, false },
                    { 2, 10, false },
                    { 1, 2, false },
                    { 5, 9, false },
                    { 3, 5, false },
                    { 4, 8, false },
                    { 8, 3, false },
                    { 13, 7, false },
                    { 14, 3, false },
                    { 12, 6, false },
                    { 15, 5, false },
                    { 10, 4, false },
                    { 11, 12, false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AdressId",
                table: "Clients",
                column: "AdressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AdressId",
                table: "Orders",
                column: "AdressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Couriers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Adresses");
        }
    }
}
