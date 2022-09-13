using System.Net;
using System.Text.RegularExpressions;
using System.Web.WebPages;
using HtmlAgilityPack;
using Microsoft.Graph;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

List<string> list = new List<string>();

// Declaring 'x' as a new WebClient() method
WebClient x = new WebClient();
Console.WriteLine("please enter Url(http://www.example.com):");
string address = Console.ReadLine();
WebClient client = new WebClient();
// Setting the URL, then downloading the data from the URL.
string source = x.DownloadString(address);



// Declaring 'document' as new HtmlAgilityPack() method
HtmlDocument document = new HtmlDocument();

// Loading document's source via HtmlAgilityPack
document.LoadHtml(source);

// For every tag in the HTML containing the node img.
var link = document.DocumentNode.Descendants("img")
        .Select(i => i.Attributes["src"]);

var scripts = document.DocumentNode.Descendants("script")
        .Select(i => i.Attributes["src"]);
//Console.WriteLine("choose file type: press(1) for images");
//Console.WriteLine("choose file type: press(2) for script");

//Console.ReadKey();
string LinkPattern = @"<img.*?src=""(?<url>.*?)"".*?>";
Regex rx = new Regex(LinkPattern);
List<string> Link = new List<string>();


foreach (Match m in rx.Matches(source))
{
        var stringm = m.Groups["url"].Value.ToString();
        if(stringm != "")
        {
                if (stringm.StartsWith(".."))
                {
                        var nonzerodigit = stringm.Remove(0, 2);
                        var okstring = address + nonzerodigit;
                        Link.Add(okstring);
                }
                else if (stringm.StartsWith("."))
                {
                        var nonzerodigit = stringm.Remove(0, 1);
                        var okstring = address + nonzerodigit;
                        Link.Add(okstring);
                }
                else if (stringm.StartsWith("/"))
                {
                        Link.Add(address + stringm);
                }
                else
                {
                        Link.Add(m.Groups["url"].Value);
                }
                       
        }
        
}
Console.WriteLine("##########********images********#########");
foreach (var item in Link)
{
        Console.WriteLine(item);

}
Console.WriteLine($"\n###Image count: {Link.Count} \n\n");



string ScripPattern = @"<script.*?src=""(?<url>.*?)"".*?>";
Regex srx = new Regex(ScripPattern);
List<string> Script = new List<string>();
foreach (Match m in srx.Matches(source))
{
        var stringm = m.Groups["url"].Value.ToString();
        if (stringm.StartsWith(".."))
        {
                var nonzerodigit = stringm.Remove(0, 2);
                var okstring = address + nonzerodigit;
                Script.Add(okstring);
        }
        else if (stringm.StartsWith("."))
        {
                var nonzerodigit = stringm.Remove(0, 1);
                var okstring = address + nonzerodigit;
                Script.Add(okstring);
        }
        else if (stringm.StartsWith("/"))
        {
                Script.Add(address + stringm);
        }
        else
                Script.Add(m.Groups["url"].Value);
}
Console.WriteLine("##########********Scripts********#########");
foreach (var item in Script)
{
        Console.WriteLine(item);

}
Console.WriteLine($"###Script count: {Script.Count}");

Console.WriteLine("want to download images?(y,n)");
var imagedownloadflag = Console.ReadLine();
if (imagedownloadflag == "y")
{
        foreach (string item in Link)
        {
                Uri uri = new Uri(item);
                byte[] data = client.DownloadData(uri);

                System.IO.File.WriteAllBytesAsync($"{System.IO.Directory.GetCurrentDirectory()}.{System.IO.Path.GetExtension(item)}", data);
        }
}


