using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Entities;

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
    //     => optionsBuilder.UseNpgsql("Host=localhost:5432; Database=SahoDB; Username=postgres; Password=postgres", npgsqlOptionsAction: sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 3));

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ArtistId_FK");

            entity.HasMany(d => d.Songs).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "AlbumSong",
                    r => r.HasOne<SongEntity>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("SongId_FK"),
                    l => l.HasOne<AlbumEntity>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("AlbumId_FK"),
                    j =>
                    {
                        j.HasKey("AlbumId", "SongId").HasName("AlbumSongs_PK");
                        j.ToTable("Album songs");
                        j.IndexerProperty<int>("AlbumId").HasColumnName("AlbumId");
                        j.IndexerProperty<int>("SongId").HasColumnName("SongId");
                    });

            entity.HasMany(d => d.LikedByUsers).WithMany(p => p.LikedAlbums)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteAlbum",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("UserId_FK"),
                    l => l.HasOne<AlbumEntity>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("AlbumId_FK"),
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

            entity.HasOne(d => d.Owner).WithMany(p => p.LikedPlaylists)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OwnerId_FK");

            entity.HasMany(d => d.Songs).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistSong",
                    r => r.HasOne<SongEntity>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("SongId_FK"),
                    l => l.HasOne<PlaylistEntity>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("PlaylistId_FK"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "SongId").HasName("PlaylistSongs_PK");
                        j.ToTable("Playlist songs");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("PlaylistId");
                        j.IndexerProperty<int>("SongId").HasColumnName("SongId");
                    });

            entity.HasMany(d => d.LikedByUsers).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoritePlaylist",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("UserId_FK"),
                    l => l.HasOne<PlaylistEntity>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("PlaylistId_FK"),
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
                .ValueGeneratedNever()
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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ArtistId_FK");

            entity.HasMany(d => d.LikedByUsers).WithMany(p => p.LikedSongs)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteSong",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("UserId_FK"),
                    l => l.HasOne<SongEntity>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("SongId_FK"),
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
            entity.Property(e => e.RoleId).HasColumnName("RoleId").IsRequired();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoleId_FK");

            entity.HasMany(d => d.LikedArtists).WithMany(p => p.LikedByUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteArtist",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("ArtistId_FK"),
                    l => l.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("UserId_FK"),
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
