using System.Linq.Expressions;

namespace EventOrganizationSystem.Generic;

public class CollectionUtils
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

    public static List<T> ApplySorting<T>(List<T> data, string sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
        {
            return data;
        }
        
        // var iEnumerableData = data.AsEnumerable();

        var sortExpression =  sortBy.Split('-');
        var descending = "ascending";
        if (sortExpression.Length > 1)
        {
            descending = sortExpression[1].Contains("desc") ? "descending" : "ascending";
        }
        var propertyName = sortExpression[0];
        var parameter = Expression.Parameter(typeof(T) , "x");
        var property = Expression.PropertyOrField(parameter, propertyName);
        var lambda = Expression.Lambda<Func<T, object>>(
            Expression.Convert(property, typeof(object)),
            parameter
            );

        if (descending.Equals("descending"))
        {
            data = data.OrderByDescending(lambda.Compile()).ToList();
        }
        else 
        {
           data = data.OrderBy(lambda.Compile()).ToList();
        }
        
        return data;
    }
}