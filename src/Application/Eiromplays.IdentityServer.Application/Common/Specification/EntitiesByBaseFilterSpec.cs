namespace Eiromplays.IdentityServer.Application.Common.Specification;

public class EntitiesByBaseFilterSpec<T, TResult> : Specification<T, TResult>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter) =>

        // ReSharper disable once VirtualMemberCallInConstructor
        Query.SearchBy(filter);
}

public class EntitiesByBaseFilterSpec<T> : Specification<T>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter) =>

        // ReSharper disable once VirtualMemberCallInConstructor
        Query.SearchBy(filter);
}