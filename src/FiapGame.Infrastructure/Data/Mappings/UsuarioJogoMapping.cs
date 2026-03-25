using FiapGame.Domain.Jogo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapGame.Infrastructure.Data.Mappings;

public class UsuarioJogoMapping : IEntityTypeConfiguration<UsuarioJogoEntity>
{
    public void Configure(EntityTypeBuilder<UsuarioJogoEntity> builder)
    {
        builder.ToTable("usuario_jogos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.JogoId)
            .HasColumnName("jogo_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.HasIndex(x => new { x.UsuarioId, x.JogoId }).IsUnique();

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId);

        builder.HasOne(x => x.Jogo)
            .WithMany()
            .HasForeignKey(x => x.JogoId);

        builder.Property(x => x.DtCadastro)
            .HasColumnName("dt_cadastro")
            .IsRequired();

        builder.Property(x => x.DtAtualizacao)
            .HasColumnName("dt_atualizacao");
    }
}
