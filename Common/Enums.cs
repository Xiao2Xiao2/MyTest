using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Enums
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        //public enum UserType
        //{
        //    /// <summary>
        //    /// 职员
        //    /// </summary>
        //    [Description("职员")]
        //    Employee = "097936c7-314b-466a-88d6-6a30ce760487",

        //    /// <summary>
        //    /// 专家
        //    /// </summary>
        //    [Description("专家")]
        //    Expert = "28620db7-6e29-419b-9d4a-5d9149d6ed8f",

        //    /// <summary>
        //    /// 未知
        //    /// </summary>
        //    [Description("未知")]
        //    Unknown = "d39f6cc8-b1ec-4599-8760-54d67c5b4e09"
        //}

        /// <summary>
        /// 专家状态
        /// </summary>
        public enum ExpertState
        {
            /// <summary>
            /// 正常
            /// </summary>
            [Description("正常")]
            Normal = 0,

            /// <summary>
            /// 其他
            /// </summary>
            [Description("其他")]
            Other = 1
        }

        /// <summary>
        /// 数据字典类型
        /// </summary>
        public enum DictionaryType
        {
            /// <summary>
            /// 系统字典
            /// </summary>
            [Description("系统字典")]
            SystemDictionary = 0,

            /// <summary>
            /// 用户字典
            /// </summary>
            [Description("用户字典")]
            UserDictionary = 1,
             /// <summary>
            /// 项目字典
            /// </summary>
            [Description("项目字典")]
            ProjectDictionary = 2
        }

        /// <summary>
        /// 标识邮件状态，可按位组合
        /// </summary>
        [Flags]
        public enum MailFlags
        {
            /// <summary>
            /// 新邮件
            /// </summary>
            New = 0,

            /// <summary>
            /// 已读邮件
            /// </summary>
            Read = 1,

            /// <summary>
            /// 已回复邮件
            /// </summary>
            Replied = 2,

            /// <summary>
            /// 已删除邮件
            /// </summary>
            Deleted = 4
        }

        /// <summary>
        /// 邮件优先级
        /// </summary>
        public enum MailPriority
        {
            /// <summary>
            /// 低
            /// </summary>
            [Description("低")]
            Low = 0,

            /// <summary>
            /// 中
            /// </summary>
            [Description("中")]
            Middle = 1,

            /// <summary>
            /// 高
            /// </summary>
            [Description("高")]
            High = 2
        }

        /// <summary>
        /// 文档管理类型
        /// </summary>
        public enum FileManagementType
        {
            /// <summary>
            /// 文件夹
            /// </summary>
            [Description("文件夹")]
            Folder = 0,

            /// <summary>
            /// 文件
            /// </summary>
            [Description("文件")]
            File = 1
        }

        /// <summary>
        /// 项目状态
        /// </summary>
        public enum ProjectState
        {
            /// <summary>
            /// 待编辑
            /// </summary>
            [Description("待编辑")]
            Draft = 0,

            /// <summary>
            /// 待提交
            /// </summary>
            [Description("待提交")]
            Auditing = 1,

            /// <summary>
            /// 待审批
            /// </summary>
            [Description("待审批")]
            Completed = 2,

            /// <summary>
            /// 已完结
            /// </summary>
            [Description("已完结")]
            End = 3,
            /// <summary>
            /// 已停止
            /// </summary>
            [Description("已停止")]
            Stop = 4
        }

        /// <summary>
        /// 项目状态（颜色显示）
        /// </summary>
        public enum ProjectStateColorString
        {
            /// <summary>
            /// 待编辑
            /// </summary>
            [Description("<font color=black>待编辑</font>")]
            Draft = 0,

            /// <summary>
            /// 审批中
            /// </summary>
            [Description("<font color=orange>审批中</font>")]
            Auditing = 1,

            /// <summary>
            /// 已完结
            /// </summary>
            [Description("<font color=green>已完结</font>")]
            Completed = 2,

            /// <summary>
            /// 已停止
            /// </summary>
            [Description("<font color=gray>已停止</font>")]
            Stop = 3
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        public enum AuditState
        {
            /// <summary>
            /// 通过
            /// </summary>
            [Description("通过")]
            Pass = 1,

            /// <summary>
            /// 不通过
            /// </summary>
            [Description("不通过")]
            NoPass = 2,

            /// <summary>
            /// 退回
            /// </summary>
            [Description("退回")]
            Return = 3,

            /// <summary>
            /// 待定
            /// </summary>
            [Description("待定")]
            Undetermined = 0

        }
        
        /// <summary>
        /// 审批状态（颜色显示）
        /// </summary>
        public enum AuditStateColorString
        {
            /// <summary>
            /// 待定
            /// </summary>
            [Description("<font color=orange>待定</font>")]
            Undetermined = 0,

            /// <summary>
            /// 通过
            /// </summary>
            [Description("<font color=green>通过</font>")]
            Pass = 1,

            /// <summary>
            /// 不通过
            /// </summary>
            [Description("<font color=gray>不通过</font>")]
            NoPass = 2,

            /// <summary>
            /// 退回
            /// </summary>
            [Description("<font color=red>退回</font>")]
            Return = 3
        }

        /// <summary>
        /// 付款审批状态
        /// </summary>
        public enum PayAuditState
        {
            /// <summary>
            /// 未审批
            /// </summary>
            [Description("未审批")]
            UnAudit = 0,

            /// <summary>
            /// 提交局办
            /// </summary>
            [Description("提交局办")]
            SubmitJuban = 1,

            /// <summary>
            /// 已审批
            /// </summary>
            [Description("已审批")]
            Pass = 2
        }
    }
}
