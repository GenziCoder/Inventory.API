namespace Inventory.API.Common
{
    public abstract class BaseEntity
    {
        // Primary Key
        public int Id { get; set; }

        // Record Creation Date
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Record Last Updated Date
        public DateTime? UpdatedDate { get; set; }

        // Soft Delete
        public bool IsDeleted { get; set; } = false;
    }
}
