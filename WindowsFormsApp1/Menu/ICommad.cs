using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1.Menu
{
    public interface ICommand
    {
        //
        // 摘要:
        //     当出现影响是否应执行该命令的更改时发生。
        event EventHandler CanExecuteChanged;

        //
        // 摘要:
        //     定义确定此命令是否可在其当前状态下执行的方法。
        //
        // 参数:
        //   parameter:
        //     此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null。
        //
        // 返回结果:
        //     如果可执行此命令，则为 true；否则为 false。
        bool CanExecute(object parameter);
        //
        // 摘要:
        //     定义在调用此命令时要调用的方法。
        //
        // 参数:
        //   parameter:
        //     此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null。
        void Execute(object parameter);
    }
}
