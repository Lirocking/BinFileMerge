//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BACK_ERROR_INFO_Back201510161018
    {
        public int AUTO_ID { get; set; }
        public string ORDER_ID { get; set; }
        public Nullable<int> USER_ID { get; set; }
        public string DEPART_ID { get; set; }
        public Nullable<int> ERROR_ID { get; set; }
        public Nullable<int> METER_ID { get; set; }
        public string ADDR { get; set; }
        public string ADDR_IN { get; set; }
        public string SOLUTION { get; set; }
        public string REPAIR_TYPE { get; set; }
        public Nullable<int> CREATE_ID { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<int> UPDATE_ID { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string Fail_Cau { get; set; }
        public Nullable<int> FaultReason { get; set; }
        public string Production_Line { get; set; }
        public string Returned_Depart { get; set; }
        public string Responsibility_Depart { get; set; }
    }
}