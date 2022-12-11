// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
class Tree
{
    public int row;
    public int col;
    public int height;
    public int scenicScore;

    public Tree(int _row, int _col, int _height)
    {
        row = _row;
        col = _col;
        height = _height;
    }
    static void Main(string[] args)
    {
        string inputText = System.IO.File.ReadAllText("C:\\Users\\Nicole\\source\\repos\\AoC Day 8\\input.txt");
        Tree[] trees = new Tree[] { };

        int rIndex = 0;
        int cIndex = 0;
        foreach (string row in inputText.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries))
        {
            cIndex = 0;
            foreach (int col in Array.ConvertAll(row.ToCharArray(),c => (int)Char.GetNumericValue(c)))
            {
                Tree newTree = new Tree(rIndex, cIndex, col);
                trees = trees.Append(newTree).ToArray();
                cIndex++;
            }
            rIndex++;
        }

        int counter = 0;
        int max = 0;
        foreach (Tree t in trees)
        {
            if (t.col==0 || t.row==0 || t.col==cIndex || t.row==rIndex)
            {
                trees[counter].scenicScore = 0;
            }
            else
            {
                int leftCount = 0;
                foreach (Tree l in (from tree in trees where (tree.col < t.col && tree.row == t.row) orderby tree.col descending select tree))
                {
                    if (l.height!=0)
                    {
                        leftCount++;
                    }
                    if (l.height >= t.height)
                    {
                        break;
                    }
                }
                int rightCount = 0;
                foreach (Tree l in (from tree in trees where (tree.col > t.col && tree.row == t.row) orderby tree.col ascending select tree))
                {
                    if (l.height != 0)
                    {
                        rightCount++;
                    }
                    if (l.height >= t.height)
                    {
                        break;
                    }
                }
                int upCount = 0;
                foreach (Tree l in (from tree in trees where (tree.col == t.col && tree.row < t.row) orderby tree.row descending select tree))
                {
                    if (l.height != 0)
                    {
                        upCount++;
                    }
                    if (l.height >= t.height)
                    {
                        break;
                    }
                }
                int downCount = 0;
                foreach (Tree l in (from tree in trees where (tree.col == t.col && tree.row > t.row) orderby tree.row ascending select tree))
                {
                    if (l.height != 0)
                    {
                        downCount++;
                    }
                    if (l.height >= t.height)
                    {
                        break;
                    }
                }
                trees[counter].scenicScore = leftCount * rightCount * upCount * downCount;
                //Debug.WriteLine(String.Format("{0} x {1} = {2}", t.row, t.col, trees[counter].scenicScore));
            }
            counter++;
        }
        Debug.WriteLine(trees.Max(tree => tree.scenicScore));
    }
}