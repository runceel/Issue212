using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using Xunit;

namespace Issue212
{
    public class UnitTest1
    {
        [Fact]
        public void Board_Has_Observable()
        {
            var rt = new ReactiveThing();
            rt.Model.Update();
            rt.Model.Update();

            Assert.Equal(2, rt.RpInt.Value);
        }
    }

    class SomeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _someInt;
        public int SomeInt => _someInt;

        public void Update()
        {
            _someInt++;
            PropertyChanged?.Invoke(this, new(nameof(SomeInt)));
        }
    }

    class ReactiveThing
    {
        // ReadOnlyReactiveProperty is prefer in this case.
        public ReadOnlyReactiveProperty<int> RpInt { get; }
        public SomeModel Model { get; }
        public ReactiveThing()
        {
            Model = new SomeModel();
            RpInt = Model.ObserveProperty(x => x.SomeInt)
                .ToReadOnlyReactiveProperty();
        }
    }
}
