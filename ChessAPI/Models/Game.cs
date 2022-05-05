namespace ChessAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The data model class.
    /// </summary>
    public partial class Game
    {
        /// <summary>
        /// The game ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The FEN length.
        /// </summary>
        [StringLength(255)]
        public string Fen { get; set; }

        /// <summary>
        /// The game status length.
        /// </summary>
        [StringLength(4)]
        public string Status { get; set; }

        [StringLength(25)]
        public string White { get; set; }

        [StringLength(25)]
        public string Black { get; set; }

        [StringLength(5)]
        public string LastMove { get; set; }

        [StringLength(5)]
        public string YourColor { get; set; }

        public bool IsYourMove { get; set; }

        [StringLength(25)]
        public string OfferDraw { get; set; }

        [StringLength(25)]
        public string Winner { get; set; }
    }
}
