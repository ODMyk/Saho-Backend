using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Roles_PK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ProfilePicture = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_PK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ArtistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Albums_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Album_ArtistId_FK",
                        column: x => x.ArtistId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorite artists",
                schema: "public",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FavoriteArtists_PK", x => new { x.ArtistId, x.UserId });
                    table.ForeignKey(
                        name: "LikedArtists_ArtistId_FK",
                        column: x => x.ArtistId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "LikedArtists_UserId_FK",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Playlists_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Playlist_OwnerId_FK",
                        column: x => x.OwnerId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ArtistId = table.Column<int>(type: "integer", nullable: false),
                    TimesPlayed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Songs_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Song_ArtistId_FK",
                        column: x => x.ArtistId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User roles",
                schema: "public",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserRoles_PK", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "UserRoles_RoleId_FK",
                        column: x => x.RoleId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "UserRoles_UserId_FK",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorite albums",
                schema: "public",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FavoriteAlbums_PK", x => new { x.AlbumId, x.UserId });
                    table.ForeignKey(
                        name: "LikedAlbums_AlbumId_FK",
                        column: x => x.AlbumId,
                        principalSchema: "public",
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "LikedAlbums_UserId_FK",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorite playlists",
                schema: "public",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FavoritePlaylists_PK", x => new { x.PlaylistId, x.UserId });
                    table.ForeignKey(
                        name: "LikedAlbums_PlaylistId_FK",
                        column: x => x.PlaylistId,
                        principalSchema: "public",
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "LikedAlbums_UserId_FK",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Album songs",
                schema: "public",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "integer", nullable: false),
                    SongId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AlbumSongs_PK", x => new { x.AlbumId, x.SongId });
                    table.ForeignKey(
                        name: "AlbumSong_AlbumId_FK",
                        column: x => x.AlbumId,
                        principalSchema: "public",
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "AlbumSong_SongId_FK",
                        column: x => x.SongId,
                        principalSchema: "public",
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorite songs",
                schema: "public",
                columns: table => new
                {
                    SongId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FavoriteSongs_PK", x => new { x.SongId, x.UserId });
                    table.ForeignKey(
                        name: "LikedSongs_SongId_FK",
                        column: x => x.SongId,
                        principalSchema: "public",
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "LikedSongs_UserId_FK",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlist songs",
                schema: "public",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "integer", nullable: false),
                    SongId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlaylistSongs_PK", x => new { x.PlaylistId, x.SongId });
                    table.ForeignKey(
                        name: "PlaylistSong_PlaylistId_FK",
                        column: x => x.PlaylistId,
                        principalSchema: "public",
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PlaylistSong_SongId_FK",
                        column: x => x.SongId,
                        principalSchema: "public",
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album songs_SongId",
                schema: "public",
                table: "Album songs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistId",
                schema: "public",
                table: "Albums",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite albums_UserId",
                schema: "public",
                table: "Favorite albums",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite artists_UserId",
                schema: "public",
                table: "Favorite artists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite playlists_UserId",
                schema: "public",
                table: "Favorite playlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite songs_UserId",
                schema: "public",
                table: "Favorite songs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist songs_SongId",
                schema: "public",
                table: "Playlist songs",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_OwnerId",
                schema: "public",
                table: "Playlists",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ArtistId",
                schema: "public",
                table: "Songs",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_User roles_RoleId",
                schema: "public",
                table: "User roles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Album songs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Favorite albums",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Favorite artists",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Favorite playlists",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Favorite songs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Playlist songs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "User roles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Albums",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Playlists",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Songs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "public");
        }
    }
}
