namespace Eiromplays.IdentityServer.Application.Common.Specification;

public class EntitiesByPaginationFilterSpec<T, TResult> : EntitiesByBaseFilterSpec<T, TResult>
{
    public EntitiesByPaginationFilterSpec(PaginationFilter filter)
        : base(filter) =>

        // ReSharper disable once VirtualMemberCallInConstructor
        Query.PaginateBy(filter);
}

public class EntitiesByPaginationFilterSpec<T> : EntitiesByBaseFilterSpec<T>
{
    public EntitiesByPaginationFilterSpec(PaginationFilter filter)
        : base(filter) =>

        // ReSharper disable once VirtualMemberCallInConstructor
        Query.PaginateBy(filter);
}