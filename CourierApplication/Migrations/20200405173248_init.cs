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
                    { 17, 51.057948m, 17.058763m, "Brzozowa 10a" },
                    { 16, 51.080764m, 16.997120m, "Raclawicka 61/3" },
                    { 15, 51.158208m, 17.032974m, "Lekarska 44/12" },
                    { 14, 51.106747m, 17.086136m, "Edwarda Dembowskiego 13/4" },
                    { 13, 51.078977m, 17.066367m, "Gazowa 50/11" },
                    { 12, 51.1259106m, 16.9693086m, "Drzewieckiego 24/29" },
                    { 11, 51.091435m, 17.008427m, "Pretficza 27/19" },
                    { 10, 51.135035m, 16.973569m, "Kolista 32/5" },
                    { 8, 51.124014m, 16.960451m, "Bajana 82/16" },
                    { 7, 51.119083m, 16.978771m, "Bystrzycka 64/3" },
                    { 6, 51.065627m, 16.957833m, "Targowa 87/11" },
                    { 5, 51.098380m, 16.936670m, "Rakietowa 20/9" },
                    { 4, 51.123066m, 17.049608m, "Nowowiejska 31/10" },
                    { 3, 51.114899m, 17.001978m, "Legnicka 42/18" },
                    { 2, 51.114836m, 17.068532m, "Grunwaldzka 71/4" },
                    { 9, 51.069190m, 17.040798m, "Terenowa 8/2" }
                });

            migrationBuilder.InsertData(
                table: "Couriers",
                columns: new[] { "CourierId", "FirstName", "LastName", "isFree" },
                values: new object[,]
                {
                    { 2, "Tomasz", "Nowak", true },
                    { 1, "Marcin", "Kowalski", true },
                    { 3, "Tomasz", "Gajda", false }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "AdressId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 12, 12 },
                    { 10, 10 },
                    { 17, 17 },
                    { 9, 9 },
                    { 13, 13 },
                    { 8, 8 },
                    { 7, 7 },
                    { 6, 6 },
                    { 14, 14 },
                    { 5, 5 },
                    { 15, 15 },
                    { 4, 4 },
                    { 16, 16 },
                    { 3, 3 },
                    { 2, 2 },
                    { 11, 11 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "AdressId", "isCompleted" },
                values: new object[,]
                {
                    { 19, 11, false },
                    { 15, 13, false },
                    { 8, 15, false },
                    { 11, 12, false },
                    { 17, 16, false },
                    { 5, 14, false },
                    { 16, 9, false },
                    { 6, 10, false },
                    { 2, 10, false },
                    { 20, 8, false },
                    { 4, 8, false },
                    { 10, 7, false },
                    { 12, 6, false },
                    { 3, 5, false },
                    { 18, 4, false },
                    { 7, 4, false },
                    { 13, 3, false },
                    { 1, 2, false },
                    { 9, 11, false },
                    { 14, 17, false }
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
