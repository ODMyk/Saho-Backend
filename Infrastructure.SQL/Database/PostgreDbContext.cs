using Microsoft.EntityFrameworkCore;
using Infrastructure.SQL.Entities;

namespace Infrastructure.SQL.Database;

public partial class PostgreDbContext : DbContext
{
    public PostgreDbContext()
    {
    }

    public PostgreDbContext(DbContextOptions<PostgreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AlbumEntity> Albums { get; set; }

    public virtual DbSet<PlaylistEntity> Playlists { get; set; }

    public virtual DbSet<RoleEntity> Roles { get; set; }

    public virtual DbSet<SongEntity> Songs { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseNpgsql(Configuration.SahoConfig.PostgreConnectionString, npgsqlOptionsAction: sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 3));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlbumEntity>(entity =>
        {
            entity.ToTable("Albums", "public");
            entity.HasKey(e => e.Id).HasName("Albums_PK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            entity.Property(e => e.ArtistId).HasColumnName("ArtistId").IsRequired();
            entity.Property(e => e.Title).HasMaxLength(50).IsRequired();

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Album_ArtistId_FK");

            entity.HasMany(d => d.LikedByUsers).WithMany(p => p.LikedAlbums)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteAlbum",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LikedAlbums_UserId_FK"),
                    l => l.HasOne<AlbumEntity>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LikedAlbums_AlbumId_FK"),
                    j =>
                    {
                        j.HasKey("AlbumId", "UserId").HasName("FavoriteAlbums_PK");
                        j.ToTable("Favorite albums");
                        j.IndexerProperty<int>("AlbumId").HasColumnName("AlbumId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });
        });

        modelBuilder.Entity<PlaylistEntity>(entity =>
        {
            entity.ToTable("Playlists", "public");
            entity.HasKey(e => e.Id).HasName("Playlists_PK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            entity.Property(e => e.OwnerId).HasColumnName("OwnerId").IsRequired();
            entity.Property(e => e.Title).HasMaxLength(50).IsRequired();

            entity.HasOne(d => d.Owner).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Playlist_OwnerId_FK");

            entity.HasMany(d => d.Songs).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistSong",
                    r => r.HasOne<SongEntity>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("PlaylistSong_SongId_FK"),
                    l => l.HasOne<PlaylistEntity>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("PlaylistSong_PlaylistId_FK"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "SongId").HasName("PlaylistSongs_PK");
                        j.ToTable("Playlist songs");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("PlaylistId");
                        j.IndexerProperty<int>("SongId").HasColumnName("SongId");
                    });

            entity.HasMany(d => d.LikedByUsers).WithMany(p => p.LikedPlaylists)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoritePlaylist",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LikedAlbums_UserId_FK"),
                    l => l.HasOne<PlaylistEntity>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LikedAlbums_PlaylistId_FK"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "UserId").HasName("FavoritePlaylists_PK");
                        j.ToTable("Favorite playlists");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("PlaylistId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.ToTable("Roles", "public");
            entity.HasKey(e => e.Id).HasName("Roles_PK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            entity.Property(e => e.Title).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<SongEntity>(entity =>
        {
            entity.ToTable("Songs", "public");
            entity.HasKey(e => e.Id).HasName("Songs_PK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            entity.Property(e => e.ArtistId).HasColumnName("ArtistId").IsRequired();
            entity.Property(e => e.Title).HasMaxLength(50).IsRequired();

            entity.HasOne(d => d.Artist).WithMany(p => p.Songs)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Song_ArtistId_FK");

            entity.HasOne(s => s.Album).WithMany(a => a.Songs)
                .HasConstraintName("AlbumID_ForeignKey")
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(d => d.LikedByUsers).WithMany(p => p.LikedSongs)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteSong",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LikedSongs_UserId_FK"),
                    l => l.HasOne<SongEntity>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LikedSongs_SongId_FK"),
                    j =>
                    {
                        j.HasKey("SongId", "UserId").HasName("FavoriteSongs_PK");
                        j.ToTable("Favorite songs");
                        j.IndexerProperty<int>("SongId").HasColumnName("SongId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("Users", "public");
            entity.HasKey(e => e.Id).HasName("Users_PK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");
            entity.Property(e => e.Nickname).HasMaxLength(50).IsRequired();

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    r => r.HasOne<RoleEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("UserRoles_UserId_FK"),
                    l => l.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("UserRoles_RoleId_FK"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("UserRoles_PK");
                        j.ToTable("User roles");
                        j.IndexerProperty<int>("RoleId").HasColumnName("RoleId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    }
            );

            entity.HasMany(d => d.LikedArtists).WithMany(p => p.LikedByUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteArtist",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LikedArtists_ArtistId_FK"),
                    l => l.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("LikedArtists_UserId_FK"),
                    j =>
                    {
                        j.HasKey("ArtistId", "UserId").HasName("FavoriteArtists_PK");
                        j.ToTable("Favorite artists");
                        j.IndexerProperty<int>("ArtistId").HasColumnName("ArtistId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
