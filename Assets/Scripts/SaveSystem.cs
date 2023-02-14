using System.Collections.Generic;
using System.Linq;

public static class SaveSystem
{
    public static IEnumerable<int> GetCollection()
    {
        return Enumerable.Repeat(1, 21);
    }

    public static IEnumerable<Deck> GetDecks()
    {
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
        yield return new Deck("TestDeck", new[] {1, 2, 3});
    }
}