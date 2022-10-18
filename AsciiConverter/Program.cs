using AsciiConverter;
using System.Diagnostics;
using System.Drawing;
using System.Text;

const uint dim_limit = 10000;
const uint dim_default = 500;

string destname = @"res.txt";
string buff = "";

try
{
    Console.WriteLine("Insert the path of the file (absolute or relative) with the extension");
    buff = Console.ReadLine().Normalize();

    Bitmap bmp = HelpConvert.FileToBitmap(buff);
    
    uint height, width;
    Console.WriteLine("Insert the width and height of the resulting text (max 10000 x 10000)");
    buff = Console.ReadLine();

    var w_and_h = buff.Split(' ');
    if (!UInt32.TryParse(w_and_h[0], out width))
    {
        throw new Exception("The width of the image is malformed");
    }
    if (width < 1 || width > dim_limit)
    {
        Console.WriteLine("The width of the image is out of range, the default value of " + dim_default.ToString() +" was used");
        width = dim_default;
    }
    if (!UInt32.TryParse(w_and_h[1], out height))
    {
        throw new Exception("The height of the image is malformed");
    }
    if (height < 1 || height > dim_limit)
    {
        Console.WriteLine("The height of the image is out of range, the default value of " + dim_default.ToString() + " was used");
        height = dim_default;
    }

    string res = HelpConvert.BitmapToAscii(bmp, height, width);

    // Check if file already exists. If yes, delete it.     
    if (File.Exists(destname))
    {
        File.Delete(destname);
    }

    // Create a new file     
    using (FileStream fs = File.Create(destname))
    {
        // Add some text to file    
        byte[] content = new UTF8Encoding(true).GetBytes(res);
        fs.Write(content, 0, content.Length);
        fs.Close();
    }

    Console.WriteLine("Conversion successful!");
    var proc = Process.Start("notepad.exe", destname);
}
catch (Exception Ex)
{
    Console.WriteLine(Ex.ToString());
}