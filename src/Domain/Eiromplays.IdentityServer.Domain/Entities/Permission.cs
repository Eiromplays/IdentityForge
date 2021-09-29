using System;

namespace Eiromplays.IdentityServer.Domain.Entities
{
    public sealed class Permission : Permission<string>
    {
        public Permission()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Permission(string name)
        {
            Name = name;
        }
    }

    public class Permission<TKey> where TKey : IEquatable<TKey>
    {
        public Permission() { }

        public Permission(string name) : this()
        {
            Name = name;
        }

        public virtual TKey? Id { get; set; }


        public string? Name { get; set; }

        public override string? ToString()
            => Name;
    }
}
