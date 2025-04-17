using System.Data;
using Dapper;

namespace Flashcards.Controllers;

public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value.Date;
    }

    public override DateTime Parse(object value)
    {
        return DateTime.Parse(value.ToString()!, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
    }
}