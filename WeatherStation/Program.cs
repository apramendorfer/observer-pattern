using Observer;

var subject = new WeatherStation();
subject.RegisterMeasurement(new Measurement(20.2));
subject.RegisterMeasurement(new Measurement(22.2));

var observer = new WeatherStationSubscriber("Observer 1");
observer.Subscribe(subject);

subject.RegisterMeasurement(new Measurement(25.2));

observer.Unsubscribe();

subject.RegisterMeasurement(new Measurement(19.8));

observer.Subscribe(subject);
subject.RegisterMeasurement(new Measurement(18.0));

subject.StopMeasuring();
Console.ReadLine();
