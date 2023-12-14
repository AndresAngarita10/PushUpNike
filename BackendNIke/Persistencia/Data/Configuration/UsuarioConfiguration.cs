using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        {
            builder.ToTable("usuario");

            builder.Property(p => p.Id)
            .IsRequired();

            builder.Property(p => p.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(50)
            .IsRequired();

            builder.Property(p => p.Email)
           .HasColumnName("email")
           .IsRequired();

            builder.Property(p => p.Password)
           .HasMaxLength(255)
           .IsRequired();

            builder
           .HasMany(p => p.Rols)
           .WithMany(r => r.Usuarios)
           .UsingEntity<RolUsuario>(

               j => j
               .HasOne(pt => pt.Rol)
               .WithMany(t => t.RolUsuarios)
               .HasForeignKey(ut => ut.IdRolFk),


               j => j
               .HasOne(et => et.Usuario)
               .WithMany(et => et.RolUsuarios)
               .HasForeignKey(el => el.IdUsuarioFk),

               j =>
               {
                   j.ToTable("userRol");
                   j.HasKey(t => new { t.IdUsuarioFk, t.IdRolFk });

               });

            builder.HasMany(p => p.RefreshTokens)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.IdUsuarioFk);


            /* builder.HasOne(p => p.TipoEmail)
            .WithMany(p => p.Emails)
            .HasForeignKey(p => p.TipoEmailIdFk); */

            /* builder.Property(e => e.Stock)
            .HasColumnName("stock")
            .HasColumnType("int")
            .HasMaxLength(3)
            .IsRequired(); */

            /* builder.Property(e => e.Descripcion)
            .HasColumnName("descripcion")
            .HasColumnType("varchar")
            .HasMaxLength(256)
            .IsRequired(); */
        }

    }
}
