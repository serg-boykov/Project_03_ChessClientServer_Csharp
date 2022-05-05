using System.Data.Entity;

namespace ChessAPI.Models
{
    public partial class ModelChessDB : DbContext
    {
        public ModelChessDB()
            : base("name=ModelChessDB")
        {
        }

        public virtual DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(e => e.Fen)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.White)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Black)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.LastMove)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.YourColor)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.IsYourMove);

            modelBuilder.Entity<Game>()
                .Property(e => e.OfferDraw)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Winner)
                .IsUnicode(false);
        }
    }
}
