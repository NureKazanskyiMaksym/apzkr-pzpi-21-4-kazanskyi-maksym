using EquipmentWatcher.Models;

namespace EquipmentWatcher.Responses
{
    public class PermissionResponse : BaseResponse
    {
        public PermissionResponse(Permission permission) : base(true, null, string.Empty)
        {
            Result = new View(permission);
        }

        public PermissionResponse(IEnumerable<Permission> permission) : base(true, null, string.Empty)
        {
            Result = permission.Select(p => new View(p));
        }

        public class View
        {
            public int ID { get; set; }
            public string Value { get; set; }

            public View(Permission permission)
            {
                this.ID = permission.PermissionID;
                this.Value = permission.Value;
            }
        }
    }
}
