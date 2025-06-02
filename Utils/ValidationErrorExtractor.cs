using Microsoft.AspNetCore.Mvc;

namespace Event_Organization_System.Helper;

public class ValidationErrorExtractor
{
    public static List<string> GetValidationErrors(ActionContext context)
    {
        var errors =  context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
            .ToList();
        
        return errors;
        
    }
}