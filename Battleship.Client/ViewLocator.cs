using System;

using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Battleship.Client
{
    public class ViewLocator : IDataTemplate
    {
        public IControl Build(object data)
        {
            var type = ResolveType(data) ?? throw new InvalidOperationException();
            return (IControl) Activator.CreateInstance(type)!;
        }

        public bool Match(object data)
        {
            return ResolveType(data) is not null;
        }

        private Type? ResolveType(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            name = data.GetType().FullName!.Replace("Model", "View");
            if (!name.EndsWith("View"))
                name += "View";
            var type = Type.GetType(name);
            return type is not null && type.IsAssignableTo(typeof(IControl))
                ? type
                : null;
        }
    }
}
