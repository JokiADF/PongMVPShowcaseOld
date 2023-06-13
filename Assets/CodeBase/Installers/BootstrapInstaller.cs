using Zenject;

namespace CodeBase.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // This additional code is included to ensure that the installation of Project Context takes place before the Loading scene is initiated. 
        }
    }
}
