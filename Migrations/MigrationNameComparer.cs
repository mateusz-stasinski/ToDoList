using static System.String;

namespace Migrations
{
  public class MigrationNameComparer : IComparer<string>
  {
    public int Compare(string x, string y)
    {
      if (x == null || y == null)
      {
        return -1;
      }
      if (x == y)
      {
        return 0;
      }
      if (CompareOrdinal(x, y) < 0)
      {
        return -1;
      }
      return 1;
    }
  }
}