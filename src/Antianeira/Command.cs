using Autofac;

namespace Antianeira
{
    public abstract class Command
    {
        protected IContainer GetIoCContainer()
        {
            return GetIoCBuilder().Build();
        }

        protected ContainerBuilder GetIoCBuilder()
        {
            var builder = new ContainerBuilder();
            builder.AddMvcReader();
            builder.AddMvcFormatter();

            return builder;
        }

        public abstract void Execute();
    }
}
