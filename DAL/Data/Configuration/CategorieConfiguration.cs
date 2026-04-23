using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class CategorieConfiguration : IEntityTypeConfiguration<Categorie>
    {
        public void Configure(EntityTypeBuilder<Categorie> builder)
        {
            builder.HasKey(x => x.Id);

            //relation with Product
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Categorie)
                .HasForeignKey(p => p.CategorieId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
