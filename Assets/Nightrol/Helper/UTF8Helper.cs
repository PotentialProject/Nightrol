using System.Text;

public static class UTF8Helper
{
    public static byte[] StringToUTF8Bytes(string input)
        => Encoding.UTF8.GetBytes(input);

    public static string UTF8BytesToString(byte[] bytes)
        => Encoding.UTF8.GetString(bytes);
}