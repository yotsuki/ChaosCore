using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosCore.ModelBase.Model
{
    public class FunctionTreeNode :Function
    {
        public FunctionTreeNode() { }
        public FunctionTreeNode(Function func) {
            this.ID = func.ID;
            this.IsMenu = func.IsMenu;
            this.Icon = func.Icon;
            this.FuncCode = func.FuncCode;
            this.FuncName = func.FuncName;
            this.OrderNo = func.OrderNo;
            this.ParentFuncID = func.ParentFuncID;
            this.Url = func.Url;
        }
        public FunctionTreeNode Parent { get; set; }
        public IEnumerable<FunctionTreeNode> Children { get; set; }
    }
}
