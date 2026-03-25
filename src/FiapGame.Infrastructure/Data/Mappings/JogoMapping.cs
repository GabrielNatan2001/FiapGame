using FiapGame.Domain.Jogo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapGame.Infrastructure.Data.Mappings;

public class JogoMapping : IEntityTypeConfiguration<JogoEntity>
{
    public void Configure(EntityTypeBuilder<JogoEntity> builder)
    {
        builder.ToTable("jogos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        builder.Property(x => x.Titulo)
            .HasColumnName("titulo")
            .HasColumnType("varchar(120)")
            .IsRequired();

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasColumnType("varchar(1000)")
            .IsRequired();

        builder.Property(x => x.Preco)
            .HasColumnName("preco")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        builder.Property(x => x.DtCadastro)
            .HasColumnName("dt_cadastro")
            .IsRequired();

        builder.Property(x => x.DtAtualizacao)
            .HasColumnName("dt_atualizacao");
    }
}
