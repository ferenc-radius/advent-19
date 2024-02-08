namespace advent_19;

public record Part(int X, int M, int A, int S)
{
    public int Sum()
    {
        return X + M + A + S;
    }
};