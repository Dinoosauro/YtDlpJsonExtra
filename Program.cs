
using System.Net;
using System.Text.Json;
using TagLib;

internal class Program
{
    private static void Main(string[] args)
    {
        var jsonGet = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json");
        for (int i = 0; i < jsonGet.Length; i++)
        {
            try {
            convert(jsonGet[i]);
            } catch (Exception ex) {
                Console.WriteLine("Exception while converting file " + jsonGet[i] + "\n" + ex.Message);
            }
        }
    }
    private static void convert(string getPath) {
                    Console.WriteLine("Adding metadata to: " + getPath);
            var getFileName = Directory.GetFiles(Directory.GetCurrentDirectory(), getPath.Substring(getPath.LastIndexOf(Path.DirectorySeparatorChar) + 1).Replace(".info.json", "") + "*");
            var fileName = "";
            for (int y = 0; y < getFileName.Length; y++)
            {
                if (!getFileName[y].Contains(".json"))
                {
                    fileName = getFileName[y];
                    break;
                }
            }

            var tfile = TagLib.File.Create(fileName);
            string json = System.IO.File.ReadAllText(getPath);
            var ciao = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(json);

            String[] artistArray = new String[1];
            artistArray[0] = ciao["channel"].ToString();
            tfile.Tag.Performers = artistArray;
            tfile.Tag.Title = ciao["fulltitle"].ToString();
            string tags = "";
            if (ciao["tags"].ToString() != null)
            {
                tags = ciao["tags"].ToString();
            }
            tfile.Tag.Comment = ciao["fulltitle"].ToString() + " from " + ciao["channel"].ToString() + "(" + ciao["display_id"].ToString() + ")" + "\n\n__________\n" + ciao["description"].ToString() + "\n\n__________\ntag:" + tags + "\n\n__________\nUpload date:" + ciao["upload_date"].ToString();
            tfile.Tag.Year = Convert.ToUInt32(ciao["upload_date"].ToString().Substring(0, 4));
            var ytThumbLink = "https://img.youtube.com/vi/" + ciao["display_id"] + "/maxresdefault.jpg";
            WebRequest request = WebRequest.Create(ytThumbLink);
            try
            {
                request.GetResponse();
            }
            catch
            {
                ytThumbLink = ytThumbLink.Replace("maxresdefault", "sddefault");
            }
            new WebClient().DownloadFile(ytThumbLink, Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "temp.jpg");
            TagLib.IPicture newArt = new Picture(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "temp.jpg");
            tfile.Tag.Pictures = new IPicture[1] { newArt };
            tfile.Save();
            System.IO.File.Delete(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "temp.jpg");
    } 
}