using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQL.Database.Entities;

public partial class PostgreDbContext : PostgreDbContext
{
    public PostgreDbContext()
    {
    }

    public PostgreDbContext(DbContextOptions<PostgreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost:5432; Database=SahoDB; Username=postgres; Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Albums_pkey1");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.ArtistId).HasColumnName("ArtistId");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ArtistId");

            entity.HasMany(d => d.Songs).WithMany(p => p.Albums)
                .UsingEntity<Dictionary<string, object>>(
                    "AlbumSong",
                    r => r.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SongId"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("AlbumId"),
                    j =>
                    {
                        j.HasKey("AlbumId", "SongId").HasName("Album songs_pkey");
                        j.ToTable("Album songs");
                        j.IndexerProperty<int>("AlbumId").HasColumnName("AlbumId");
                        j.IndexerProperty<int>("SongId").HasColumnName("SongId");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.AlbumsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteAlbum",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserId"),
                    l => l.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("AlbumId"),
                    j =>
                    {
                        j.HasKey("AlbumId", "UserId").HasName("Favorite albums_pkey");
                        j.ToTable("Favorite albums");
                        j.IndexerProperty<int>("AlbumId").HasColumnName("AlbumId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Playlists_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.OwnerId).HasColumnName("OwnerId");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Owner).WithMany(p => p.PlaylistsNavigation)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OwnerId");

            entity.HasMany(d => d.Songs).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistSong",
                    r => r.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SongId"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PlaylistId"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "SongId").HasName("Playlist songs_pkey");
                        j.ToTable("Playlist songs");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("PlaylistId");
                        j.IndexerProperty<int>("SongId").HasColumnName("SongId");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoritePlaylist",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserId"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PlaylistId"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "UserId").HasName("Favorite playlists_pkey");
                        j.ToTable("Favorite playlists");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("PlaylistId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Songs_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.ArtistId).HasColumnName("ArtistId");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasMany(d => d.Users).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteSong",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserId"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SongId"),
                    j =>
                    {
                        j.HasKey("SongId", "UserId").HasName("Favorite songs_pkey");
                        j.ToTable("Favorite songs");
                        j.IndexerProperty<int>("SongId").HasColumnName("SongId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.Nickname).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoleId");

            entity.HasMany(d => d.Artists).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteArtist",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ArtistId"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserId"),
                    j =>
                    {
                        j.HasKey("ArtistId", "UserId").HasName("Favorite artists_pkey");
                        j.ToTable("Favorite artists");
                        j.IndexerProperty<int>("ArtistId").HasColumnName("ArtistId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Artists)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteArtist",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("UserId"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("ArtistId"),
                    j =>
                    {
                        j.HasKey("ArtistId", "UserId").HasName("Favorite artists_pkey");
                        j.ToTable("Favorite artists");
                        j.IndexerProperty<int>("ArtistId").HasColumnName("ArtistId");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
