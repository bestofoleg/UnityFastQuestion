using System;

[Serializable]
public class Language
{

    public int language_id { get; set; }

    public string language_name { get; set; }

    public Language()
    {}

    public Language(int languageId, string languageName)
    {
        language_id = languageId;
        language_name = languageName;
    }

    public override string ToString()
    {
        return "Language = {id = "+language_id+", name = "+language_name+"}";
    }
}
