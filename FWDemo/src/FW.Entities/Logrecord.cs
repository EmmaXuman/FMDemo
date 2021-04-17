using FW.Entities.Core;
using System;

namespace FW.Entities
{
    /// <summary>
    /// 日志表
    /// </summary>
    public class Logrecord : IEntity
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string LogLevel { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string MachineName { get; set; }
        public string MachineIp { get; set; }
        public string NetRequestMethod { get; set; }
        public string NetRequestUrl { get; set; }
        public string NetUserIsauthenticated { get; set; }
        public string NetUserAuthtype { get; set; }
        public string NetUserIdentity { get; set; }
    }
}
