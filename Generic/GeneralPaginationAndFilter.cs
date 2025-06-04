namespace Event_Organization_System.Generic;

public class GeneralPaginationAndFilter
{
    public static List<T> ApplyPagination<T>(List <T> data, int pageNumber, int pageSize)
    {
        if (data == null)
        {
            throw new ArgumentException("Data cannot be null", nameof(data));
        }

        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number cannot be less than 1", nameof(pageNumber));
        }

        if (pageSize < 1)
        {
            throw new ArgumentException("Page size cannot be less than 1", nameof(pageSize));
        }
        
        var iEnumerableData = data.AsEnumerable();

        return iEnumerableData
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}