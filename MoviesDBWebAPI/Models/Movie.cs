using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesDBWebAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        [DisplayName("Original Title")]
        public string Original_Title { get; set; }

        [DisplayName("Release Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Release_Date { get; set; }
        public bool adult { get; set; }
        public string Backdrop_Path { get; set; }
        [DisplayName("Original Language")]
        public string Original_Language { get; set; }
        public string Popularity { get; set; }
        public string Poster_Path { get; set; }
        [DisplayName("Vote Average")]
        public float Vote_Average { get; set; }
        public bool Video { get; set; }
        [DisplayName("Vote Count")]
        public int Vote_Count { get; set; }
       
    }
}
