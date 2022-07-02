using System.Text;
using Microsoft.Extensions.Primitives;

namespace Eiromplays.IdentityServer.Infrastructure.Common.Extensions;

public class PathExtensions
{
    public static readonly char[] PathSeparators = { '/', '\\' };
    private const string CurrentDirectoryToken = ".";
    private const string ParentDirectoryToken = "..";

    /// <summary>
    /// Combines two path parts.
    /// </summary>
    public static string Combine(string path, string other = null!)
    {
        if (string.IsNullOrWhiteSpace(other))
        {
            return path;
        }

        if (other.StartsWith('/') || other.StartsWith('\\'))
        {
            // "other" is already an app-rooted path. Return it as-is.
            return other;
        }

        int index = path.LastIndexOfAny(PathSeparators);

        if (index != path.Length - 1)
        {
            // If the first ends in a trailing slash e.g. "/Home/", assume it's a directory.
            return path + "/" + other;
        }
        else
        {
            return string.Concat(path.AsSpan(0, index + 1), other);
        }
    }

    /// <summary>
    /// Combines multiple path parts.
    /// </summary>
    public static string Combine(string path, params string[] others)
    {
        return others.Aggregate(path, Combine);
    }

    /// <summary>
    /// Resolves relative segments in a path.
    /// </summary>
    public static string ResolvePath(string path)
    {
        var pathSegment = new StringSegment(path);
        if (path[0] == PathSeparators[0] || path[0] == PathSeparators[1])
        {
            // Leading slashes (e.g. "/Views/Index.cshtml") always generate an empty first token. Ignore these
            // for purposes of resolution.
            pathSegment = pathSegment.Subsegment(1);
        }

        var tokenizer = new StringTokenizer(pathSegment, PathSeparators);
        bool requiresResolution = tokenizer.Any(segment => segment.Length == 0 || segment.Equals(ParentDirectoryToken) || segment.Equals(CurrentDirectoryToken));

        if (!requiresResolution)
        {
            return path;
        }

        var pathSegments = new List<StringSegment>();
        foreach (var segment in tokenizer.Where(segment => segment.Length != 0))
        {
            if (segment.Equals(ParentDirectoryToken))
            {
                if (pathSegments.Count == 0)
                {
                    // Don't resolve the path if we ever escape the file system root. We can't reason about it in a
                    // consistent way.
                    return path;
                }

                pathSegments.RemoveAt(pathSegments.Count - 1);
            }
            else if (!segment.Equals(CurrentDirectoryToken))
            {
                pathSegments.Add(segment);
            }
        }

        var builder = new StringBuilder();
        foreach (var segment in pathSegments)
        {
            builder.Append('/');
            builder.Append(segment.Buffer, segment.Offset, segment.Length);
        }

        return builder.ToString();
    }
}