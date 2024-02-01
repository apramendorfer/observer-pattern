namespace Observer
{
    public record Measurement(double Value);

    public class WeatherStation : IObservable<Measurement>
    {
        private readonly List<IObserver<Measurement>> _subscribers;
        private readonly List<Measurement> _measurements;

        public WeatherStation()
        {
            _subscribers = new List<IObserver<Measurement>>();
            _measurements = new List<Measurement>();
            Console.WriteLine($"{this} started");
        }

        public IDisposable Subscribe(IObserver<Measurement> observer)
        {
            if (!_subscribers.Contains(observer))
            {
                _subscribers.Add(observer);

                foreach (var item in _measurements)
                {
                    observer.OnNext(item);
                }
            }

            return new Unsubscriber<Measurement>(_subscribers, observer);
        }

        public void StopMeasuring()
        {
            foreach (IObserver<Measurement> observer in _subscribers)
            {
                observer.OnCompleted();
            }

            _subscribers.Clear();
            Console.WriteLine($"{this} completed");
        }


        public void RegisterMeasurement(Measurement measurement)
        {
            _measurements.Add(measurement);

            foreach (var observer in _subscribers)
            {
                observer.OnNext(measurement);
            }
        }


        internal sealed class Unsubscriber<Measurement> : IDisposable
        {
            private readonly IList<IObserver<Measurement>> _observers;
            private readonly IObserver<Measurement> _observer;

            internal Unsubscriber(
                IList<IObserver<Measurement>> observers,
                IObserver<Measurement> observer) => (_observers, _observer) = (observers, observer);

            public void Dispose() => _observers.Remove(_observer);
        }
    }
    public class WeatherStationSubscriber : IObserver<Measurement>
    {
        private IDisposable _disposable;
        private string _name;

        public WeatherStationSubscriber(string name)
        {
            _name = name;
        }

        public virtual void Subscribe(WeatherStation provider)
        {
            Console.WriteLine($"{_name} subscribed to {provider}");
            _disposable = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            Console.WriteLine($"{_name} unsubscribed from provider");
            _disposable.Dispose();

        }

        public void OnCompleted()
        {
            Console.WriteLine($"{_name} completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Error");
        }

        public void OnNext(Measurement value)
        {
            Console.WriteLine($"[{_name}] Temperature: {value.Value}");
        }
    }
}
