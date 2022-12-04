
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Finding all movies not accompanied by subtitle files...");

string[] subtitleExts = { ".srt", ".vtt", ".ssa", ".ass", ".smi" };
string[] movieExts = { ".mp4", ".mkv", ".m4v", ".avi", ".wmv", ".mpg", ".mov", ".ts" };

ValueTuple<Int32, Int32> cursorPosition;
int moviesChecked = 0;

List<string> outList = new List<string>();
ScanDirectory(Directory.GetCurrentDirectory());
outList.Sort();
Console.WriteLine("");

foreach (string file in outList)
{
    // Possibly work around a Windows bug.  It's impossible to do completely,
    // but this is the best I can do since the default width is 120.
    if (file.Length == 120)
        Console.WriteLine(file + " ");
    else
        Console.WriteLine(file);
}

Console.WriteLine("Movies checked: " + moviesChecked.ToString());
Console.WriteLine("Movies with subtitles found: " + (moviesChecked - outList.Count).ToString());
Console.WriteLine("Movies without subtitles found: " + outList.Count.ToString());

Console.WriteLine("Press any key to exit");
Console.ReadKey();

void ScanDirectory(string currentDir)
{
    var movies = new List<string>();
    foreach (string fileName in Directory.GetFiles(currentDir))
    {
        if (movieExts.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase) == false)
            continue;
        movies.Add(fileName);
        ++moviesChecked;
        cursorPosition = Console.GetCursorPosition();
        Console.Write(String.Format("Movies checked: {0}       ", moviesChecked));
        Console.SetCursorPosition(cursorPosition.Item1, cursorPosition.Item2);
    }

    HashSet<string> excluded = new HashSet<string>();
    foreach (string fileName in Directory.GetFiles(currentDir))
    {
        if (subtitleExts.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase) == false)
            continue;
        foreach (string movie in movies)
        {
            if (movie is null) 
                continue;  
            string fn = Path.Combine(Path.GetDirectoryName(movie)!, Path.GetFileNameWithoutExtension(movie));
            if (fileName.StartsWith(fn, StringComparison.OrdinalIgnoreCase))
                excluded.Add(movie);
        }
    }

    foreach (string movie in movies)
    {
        if (excluded.Contains(movie)) 
            continue;
        outList.Add(movie);
    }

    foreach (var dir in Directory.GetDirectories(currentDir))
        ScanDirectory(dir);
}

