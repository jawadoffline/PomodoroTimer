using PomodoroTimer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

namespace PomodoroTimer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer;
        private int _secondsLeft;
        private bool _isRunning;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public MainViewModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += TimerTick;
            Reset();
        }

        public string TimeLeft =>
            TimeSpan.FromSeconds(_secondsLeft).ToString(@"mm\:ss");

        public bool IsRunning
        {
            get => _isRunning;
            set { _isRunning = value; OnPropertyChanged(nameof(IsRunning)); }
        }

        public ICommand StartPauseCommand =>
            new RelayCommand(_ =>
            {
                if (IsRunning) Pause();
                else Start();
            });

        public ICommand ResetCommand =>
            new RelayCommand(_ => Reset());

        private void Start()
        {
            IsRunning = true;
            _timer.Start();
        }

        private void Pause()
        {
            IsRunning = false;
            _timer.Stop();
        }

        private void Reset()
        {
            Pause();
            _secondsLeft = TimerSettings.WorkMinutes * 60;
            OnPropertyChanged(nameof(TimeLeft));
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (_secondsLeft == 0)
            {
                System.Media.SystemSounds.Beep.Play();
                Reset();
                return;
            }

            _secondsLeft--;
            OnPropertyChanged(nameof(TimeLeft));
        }
    }
}
