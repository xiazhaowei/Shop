using System.ServiceProcess;

namespace Shop.TimerTask
{
    partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            Bootstrap.Initialize();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            Bootstrap.Start();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            Bootstrap.Stop();
        }
    }
}
