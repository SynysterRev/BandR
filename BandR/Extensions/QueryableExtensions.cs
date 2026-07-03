using System.Reflection;
using System.Linq.Dynamic.Core;
using BandR.Common;
using BandR.Entities;

namespace BandR.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
    
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string? sortBy) where T : class
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        var allowedProperties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) 
                        || p.PropertyType.IsValueType)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var sortExpressions = new List<string>();

        foreach (var part in sortBy.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var tokens = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0 || !allowedProperties.Contains(tokens[0]))
                continue;

            var direction = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "descending"
                : "ascending";

            sortExpressions.Add($"{tokens[0]} {direction}");
        }

        return sortExpressions.Count > 0
            ? query.OrderBy(string.Join(", ", sortExpressions))
            : query;
    }
    
    public static IQueryable<Announcement> ApplyFilters(this IQueryable<Announcement> query, AnnouncementQueryFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var searchLower = filter.Search.ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(searchLower) 
                                     || a.Description.ToLower().Contains(searchLower));
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var cityLower = filter.City.ToLower();
            query = query.Where(a => a.Location.City.ToLower() == cityLower);
        }

        if (filter.Instruments?.Count > 0)
        {
            var targetInstruments = filter.Instruments.Select(i => i.ToLower()).ToList();
            query = query.Where(a => a.AnnouncementInstruments
                .Any(ai => targetInstruments.Contains(ai.Instrument.Name.ToLower())));
        }

        if (filter.Styles?.Count > 0)
        {
            var targetStyles = filter.Styles.Select(s => s.ToLower()).ToList();
            query = query.Where(a => a.Styles
                .Any(s => targetStyles.Contains(s.Name.ToLower())));
        }
        
        if (filter.Tags?.Count > 0)
        {
            var targetTags = filter.Tags.Select(t => t.ToLower()).ToList();
            query = query.Where(a => a.Tags
                .Any(t => targetTags.Contains(t.Name.ToLower())));
        }

        return query;
    }
}