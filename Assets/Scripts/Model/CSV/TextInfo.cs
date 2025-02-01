public class TextInfo : Data
{
    public enum Types { Name, Desc }
    public enum LanguageTypes { Korean, English }

    public string NameKr { get; set; }
    public string DescriptionKr { get; set; }
    public string NameEn { get; set; }
    public string DescriptionEn { get; set; }
}
