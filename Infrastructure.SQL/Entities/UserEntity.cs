namespace Infrastructure.SQL.Entities;

public partial class UserEntity
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int SecurityCode { get; set; }

    public virtual ICollection<RoleEntity> Roles { get; set; } = [];

    public virtual ICollection<ArtistEntity> Artists { get; set; } = [];
}