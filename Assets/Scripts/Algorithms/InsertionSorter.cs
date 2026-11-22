using System.Collections.Generic;

public class SiloResult
{
    public int silo;
    public int torch;
}

public static class InsertionSorter
{
    public static void SortBrightestFirst(List<SiloResult> results)
    {
        for (int i = 1; i < results.Count; i++)
        {
            SiloResult current = results[i];
            int j = i - 1;

            // Strict < so Silos with the same torch keep their order.
            while (j >= 0 && results[j].torch < current.torch)
            {
                results[j + 1] = results[j];
                j--;
            }

            results[j + 1] = current;
        }
    }
}
