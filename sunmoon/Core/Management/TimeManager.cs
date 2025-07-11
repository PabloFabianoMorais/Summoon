using System;
using Microsoft.Xna.Framework;

namespace sunmoon.Core.Management
{
    public enum TimeOfDay
    {
        Dawn,
        Day,
        Dusk,
        Night
    }
    public class TimeManager
    {
        public float DayDurationInSeconds { get; set; } = 60f;
        public float CurrentTime { get; private set; } = 0f;
        public int DayCount { get; private set; } = 1;
        public TimeOfDay CurrentTimeOfDay { get; private set; }

        public event Action<TimeOfDay> OnTimeOfDayChanged;

        public TimeManager()
        {
            CurrentTime = 0.25f;
            UpdateState(0f);
        }

        public void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float timeIncrement = elapsedSeconds / DayDurationInSeconds;

            UpdateState(timeIncrement);
        }

        private void UpdateState(float timeIncrement)
        {
            CurrentTime += timeIncrement;

            if (CurrentTime >= 1.0f)
            {
                CurrentTime -= 1.0f;
                DayCount += 1;
            }

            var previousTimeOfDay = CurrentTimeOfDay;

            if (CurrentTime >= 0.25f && CurrentTime < 0.75f)
                CurrentTimeOfDay = TimeOfDay.Day;
            else if (CurrentTime >= 0.75 && CurrentTime < 0.85)
                CurrentTimeOfDay = TimeOfDay.Dusk;
            else if (CurrentTime >= 0.85 || CurrentTime < 0.15f)
                CurrentTimeOfDay = TimeOfDay.Night;
            else
                CurrentTimeOfDay = TimeOfDay.Dawn;

            if (previousTimeOfDay != CurrentTimeOfDay)
            {
                OnTimeOfDayChanged?.Invoke(CurrentTimeOfDay);
                Console.WriteLine($"A new day has begun: It's {CurrentTimeOfDay}");
            }

        }
    }
}