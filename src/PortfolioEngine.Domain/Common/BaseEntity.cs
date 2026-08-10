using System;

namespace PortfolioEngine.Domain.Common
{
    public interface IAuditableEntity
    {
        DateTime CreatedAtUtc { get; set; }
        string? CreatedBy { get; set; }
        DateTime? UpdatedAtUtc { get; set; }
        string? UpdatedBy { get; set; }
    }
    /// <summary>
    /// Contract for entities that support soft deletion.
    /// </summary>
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAtUtc { get; set; }
        string? DeletedBy { get; set; }
    }

    /// <summary>
    /// Represents a base entity with an identifier of type TKey, audit properties, and soft delete capabilities.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseEntity<TKey> : IAuditableEntity, ISoftDelete
    {
        public TKey Id { get; set; } = default!;

        // IAuditableEntity
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public string? UpdatedBy { get; set; }

        // ISoftDelete
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public string? DeletedBy { get; set; }
    }

    /// <summary>
    /// Represents a base entity with a Guid identifier, audit properties, and soft delete capabilities.
    /// </summary>
    public abstract class BaseEntity : BaseEntity<Guid>
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}