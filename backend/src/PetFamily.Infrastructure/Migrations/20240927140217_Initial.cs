using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    requisites = table.Column<string>(type: "jsonb", nullable: false),
                    social_networks = table.Column<string>(type: "jsonb", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    experience = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    patronymic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "breeds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_breeds_species_species_id",
                        column: x => x.species_id,
                        principalTable: "species",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    help_status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    requisites = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    address_city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address_country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address_flat = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address_house = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address_street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    birthday_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    health_info = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                    height = table.Column<double>(type: "double precision", nullable: false),
                    is_castrated = table.Column<bool>(type: "boolean", nullable: false),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pet_photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pet_photos", x => x.id);
                    table.ForeignKey(
                        name: "fk_pet_photos_pets_pet_id",
                        column: x => x.pet_id,
                        principalTable: "pets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_breeds_species_id",
                table: "breeds",
                column: "species_id");

            migrationBuilder.CreateIndex(
                name: "ix_pet_photos_pet_id",
                table: "pet_photos",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id",
                table: "pets",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breeds");

            migrationBuilder.DropTable(
                name: "pet_photos");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "volunteers");
        }
    }
}
