using SimpleEnterpriseFramework.Builders.UIBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.Observer
{
    internal class ObservableFormTextField : IObservable
    {
        private readonly FormTextField _textField;
        private readonly List<IObserver> _observers = new List<IObserver>();

        public ObservableFormTextField(FormTextField textField)
        {
            _textField = textField;
            _textField.ControlTextBox.TextChanged += (s, e) => Notify();
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public FormTextField GetFormTextField() => _textField; // Access the original text fieldprivate List<IObserver> observers = new List<IObserver>();
    
}
}
