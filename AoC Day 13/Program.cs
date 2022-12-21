using System.Diagnostics;
using System.Text.Json;

IEnumerable<string> input = File.ReadAllLines("input.txt");
Part2.Calc(input.GetEnumerator());

public class Part
{
    public static int Compare(string s1, string s2) =>
        Compare(JsonSerializer.Deserialize<JsonElement>(s1), JsonSerializer.Deserialize<JsonElement>(s2));
    public static int Compare(JsonElement j1, JsonElement j2) =>
        (j1.ValueKind, j2.ValueKind) switch
        {
            (JsonValueKind.Number, JsonValueKind.Number) =>
                j1.GetInt32() - j2.GetInt32(),
            (JsonValueKind.Number, _) =>
                DoCompare(JsonSerializer.Deserialize<JsonElement>($"[{j1.GetInt32()}]"), j2),
            (_, JsonValueKind.Number) =>
                DoCompare(j1, JsonSerializer.Deserialize<JsonElement>($"[{j2.GetInt32()}]")),
            _ => DoCompare(j1, j2),
        };
    public static int DoCompare(JsonElement j1, JsonElement j2)
    {
        int res;
        JsonElement.ArrayEnumerator e1 = j1.EnumerateArray();
        JsonElement.ArrayEnumerator e2 = j2.EnumerateArray();
        while (e1.MoveNext() && e2.MoveNext())
            if ((res = Compare(e1.Current, e2.Current)) != 0)
                return res;
        return j1.GetArrayLength() - j2.GetArrayLength();
    }
}
public class Part1
{
    public int Calc(List<string> input)
    {
        var left = new List<string>();
        var right = new List<string>();
        int sum = 0;
        for (int i = 0; i < input.Count; i++)
        {
            if (i % 2 == 0)
                left.Add(input[i]);
            else
                right.Add(input[i]);
        }
        for (int i = 0; i < left.Count; i++)
        {
            var res = Part.Compare(left[i], right[i]);
            if (res < 0)
                sum += (i + 1);
        }
        Debug.WriteLine(sum);
        return sum;
    }
}
public class Part2 : Part
{
    public static int Calc(IEnumerator<string> input)
    {
        int decoder = 0;
        string[] dividers = new string[3] { "[[]]", "[[2]]", "[[6]]" };
        List<string> items = new(dividers);
        while (input.MoveNext())
            if (input.Current.Length > 0)
                items.Add(input.Current);
        items.Sort(Compare);
        decoder = items.IndexOf(dividers[1]) * items.IndexOf(dividers[2]);
        Debug.WriteLine(decoder);
        return decoder;
    }
}