using System.Collections.ObjectModel;

var input = File.ReadAllLines("input.txt");
ObservableCollection<(long value, int start)> coordinates = new ObservableCollection<(long, int)>();
int length = input.Count();
int key = 811589153;

for (int i = 0; i < length; i++)
{
    coordinates.Add((long.Parse(input[i]) * key, i));
}

int index;

for (int k = 0; k < 10; k++)
{
    for (int i = 0; i < length; i++)
    {
        
        var current = coordinates.Where(c => c.start == i).First();
        var moves = current.value;
        if (Math.Abs(current.value) > length)
            moves = (moves % (length - 1));
        if (current.value < 0)
        {
            index = coordinates.IndexOf(current);
            var end = index + moves;
            while (end < 0)
                end += (length - 1);
            if (end == 0)
                coordinates.Move(index, length - 1);
            else if (end == length - 1)
                coordinates.Move(index, length - 2);
            else
                coordinates.Move(index, (int)end);
        }
        if (current.value > 0)
        {
            index = coordinates.IndexOf(current);
            var end = index + moves;
            while (end >= length)
                end -= (length - 1);
            coordinates.Move(index, (int)end);
        }
    }
}

index = coordinates.IndexOf(coordinates.Where(c => c.value == 0).First());
var first = index + (1000 % length);
if (first >= length) first -= length;
var second = index + (2000 % length);
if (second >= length) second -= length;
var third = index + (3000 % length);
if (third >= length) third -= length;

Console.WriteLine((coordinates[first].value + coordinates[second].value + coordinates[third].value));