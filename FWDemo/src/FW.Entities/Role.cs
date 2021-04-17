using FW.Entities.Core;

namespace FW.Entities
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Remark { get; set; }
    }
}
