using System;
using System.Collections.Generic;

namespace Observer
{


    interface IObserver
    {
        public void update(float temperature, float humidity, float pressure);
    }

    interface ISubject
    {
        public void registerObserver(IObserver observer);
        public void removeObserver(IObserver observer);
        public void notifyObservers();
    }

    class WeatherData : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();
        private float temperature = 22;
        private float pressure = 1027;
        private float humidity = 71;

        public void registerObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void removeObserver(IObserver observer)
        {
            int index = observers.IndexOf(observer);
            if (index >= 0)
            {
                observers.RemoveAt(index);
            }
        }

        public void notifyObservers()
        {
            foreach (IObserver observer in observers)
            {
                observer.update(getTemperature(), getHumidity(), getPressure());
            }
        }

        float getTemperature()
        {
            return temperature;
        }

        float getHumidity()
        {
            return humidity;
        }

        float getPressure()
        {
            return pressure;
        }

        public void measurementsChanged()
        {
            notifyObservers();
        }
    }



    class CurrentConditionsDisplay : IObserver
    {
        private float temperature;
        private float pressure;
        private float humidity;

        public CurrentConditionsDisplay(ISubject subject)
        {
            subject.registerObserver(this);
        }


        public void update(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            this.display();
        }

        public void display()
        {
            Console.WriteLine("_____Current conditions_____");
            Console.WriteLine("Temperature: " + temperature + " C");
            Console.WriteLine("Humidity: " + humidity + " Pa");
            Console.WriteLine("Pressure: " + pressure + " %");
        }
    }

    class StatisticsDisplay : IObserver
    {
        private float temperature;
        private float pressure;
        private float humidity;

        private float normal_temperature = 20;
        private float normal_pressure = 1020;
        private float normal_humidity = 62;

        public StatisticsDisplay(WeatherData weatherData)
        {
            weatherData.registerObserver(this);
        }

        public void update(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            this.display();
        }
        public void display()
        {
            Console.WriteLine("_____Statistics_____");
            Console.WriteLine("Temperature is different from normal by " + Math.Abs(temperature - normal_temperature) + " C");
            Console.WriteLine("Humidity is different from normal by " + Math.Abs(humidity - normal_humidity) + "%"); 
            Console.WriteLine("Pressure is different from normal by " + Math.Abs(pressure - normal_pressure) + " Pa");
        }
    }

    class ForecastDisplay : IObserver
    {

        private float temperature;
        private float pressure;
        private float humidity;

        public ForecastDisplay(WeatherData weatherData)
        {
            weatherData.registerObserver(this);
        }

        public void update(float temperature, float humidity, float pressure)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            this.display();
        }

        public void display()
        {
            Console.WriteLine("_____Weather forecast_____");
            Console.WriteLine("Temperature will be: " + (temperature + 2) + " C");
            Console.WriteLine("Pressure will be: " + (pressure - 17) + " Pa");
            Console.WriteLine("Humidity will be: " + (humidity - 7) + "%");
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            var weatherData = new WeatherData();

            var currentConditionsDisplay = new CurrentConditionsDisplay(weatherData);
            var statisticsDisplay = new StatisticsDisplay(weatherData);
            var forecastDisplay = new ForecastDisplay(weatherData);
            weatherData.measurementsChanged();
        }
    }
}