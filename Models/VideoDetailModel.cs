namespace Video_Catalogue.Models
{
    public class VideoDetailModel
    {
        public required string FileName { get; set; }

        public double FileSize { get; set; }

        public string FilePath => $"/media/{FileName}";

    }
}
