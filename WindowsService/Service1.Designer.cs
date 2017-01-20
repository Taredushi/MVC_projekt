namespace WindowsService
{
    partial class Service1
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            //
            // serviceProcessInstaller1
            //
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            //
            // serviceInstaller1
            //

            //Set the ServiceName of the Windows Service.
            this.serviceInstaller1.ServiceName = "SimpleService";

            //Set its StartType to Automatic.
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;

            //
            // ProjectInstaller
            //
            this.Installer.AddRange(new System.Configuration.Install.Installer[]
            {
                this.serviceProcessInstaller1,
                this.serviceInstaller1
            });
        }

        #endregion
    }
}
