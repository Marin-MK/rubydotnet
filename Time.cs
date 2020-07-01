using System;
using System.Collections.Generic;
using System.Text;

namespace rubydotnet
{
    public static partial class Ruby
    {
        public class Time : Object
        {
            public new static string KlassName = "Time";
            public new static Class Class { get => (Class) GetKlass(KlassName); }

            public Time(IntPtr Pointer) : base(Pointer, true) { }
            public Time(double Milliseconds) : this(rb_time_num_new(DBL2NUM(Milliseconds / 1000d), INT2NUM(0))) { }
            public Time(long Milliseconds) : this((double) Milliseconds) { }
            public Time(DateTime time) : this(time.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds) { }
            public Time(int Year, int Month) : this(new DateTime(Year, Month, 1)) { }
            public Time(int Year, int Month, int Day) : this(new DateTime(Year, Month, Day)) { }
            public Time(int Year, int Month, int Day, int Hour) : this(new DateTime(Year, Month, Day, Hour, 0, 0)) { }
            public Time(int Year, int Month, int Day, int Hour, int Minute) : this(new DateTime(Year, Month, Day, Hour, Minute, 0)) { }
            public Time(int Year, int Month, int Day, int Hour, int Minute, int Second) : this(new DateTime(Year, Month, Day, Hour, Minute, Second)) { }
            public Time(int Year, int Month, int Day, int Hour, int Minute, int Second, int Millisecond) : this(new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond)) { }
            public Time() : this(DateTime.Now) { }

            public static Time At(double Milliseconds)
            {
                return new Time(Milliseconds);
            }
            public static Time At(long Milliseconds)
            {
                return new Time((double) Milliseconds);
            }
            public static Time At(DateTime time)
            {
                return new Time(time);
            }
            public static Time At(int Year, int Month)
            {
                return new Time(Year, Month);
            }
            public static Time At(int Year, int Month, int Day)
            {
                return new Time(Year, Month, Day);
            }
            public static Time At(int Year, int Month, int Day, int Hour)
            {
                return new Time(Year, Month, Day, Hour);
            }
            public static Time At(int Year, int Month, int Day, int Hour, int Minute)
            {
                return new Time(Year, Month, Day, Hour, Minute);
            }
            public static Time At(int Year, int Month, int Day, int Hour, int Minute, int Second)
            {
                return new Time(Year, Month, Day, Hour, Minute, Second);
            }
            public static Time At(int Year, int Month, int Day, int Hour, int Minute, int Second, int Millisecond)
            {
                return new Time(Year, Month, Day, Hour, Minute, Second, Millisecond);
            }

            public static Time Now { get => new Time(); }

            public static Time Years(int Years)
            {
                return new Time(Years * 365 * 24 * 60 * 60 * 1000);
            }
            public static Time Months(int Months)
            {
                return new Time(Months * 30 * 24 * 60 * 60 * 1000);
            }
            public static Time Days(int Days)
            {
                return new Time(Days * 24 * 60 * 60 * 1000);
            }
            public static Time Hours(int Hours)
            {
                return new Time(Hours * 60 * 60 * 1000);
            }
            public static Time Minutes(int Minutes)
            {
                return new Time(Minutes * 60 * 1000);
            }
            public static Time Seconds(int Seconds)
            {
                return new Time(Seconds * 1000);
            }
            public static Time Milliseconds(int Milliseconds)
            {
                return new Time(Milliseconds);
            }

            public static Time operator +(Time One, Time Two)
            {
                return new Time(One.ToInt64() + Two.ToInt64());
            }
            public static Time operator +(Time One, long Two)
            {
                return new Time(One.ToInt64() + Two);
            }
            public static Time operator +(long One, Time Two)
            {
                return new Time(One + Two.ToInt64());
            }
            public static Time operator +(DateTime One, Time Two)
            {
                return new Time(One.ToInt64() + Two.ToInt64());
            }
            public static Time operator +(Time One, DateTime Two)
            {
                return new Time(One.ToInt64() + Two.ToInt64());
            }

            public static Time operator -(Time One, Time Two)
            {
                return new Time(One.ToInt64() - Two.ToInt64());
            }
            public static Time operator -(Time One, long Two)
            {
                return new Time(One.ToInt64() - Two);
            }
            public static Time operator -(long One, Time Two)
            {
                return new Time(One - Two.ToInt64());
            }
            public static Time operator -(DateTime One, Time Two)
            {
                return new Time(One.ToInt64() - Two.ToInt64());
            }
            public static Time operator -(Time One, DateTime Two)
            {
                return new Time(One.ToInt64() - Two.ToInt64());
            }

            public static Time operator *(Time One, Time Two)
            {
                return new Time(One.ToInt64() * Two.ToInt64());
            }
            public static Time operator *(Time One, long Two)
            {
                return new Time(One.ToInt64() * Two);
            }
            public static Time operator *(long One, Time Two)
            {
                return new Time(One * Two.ToInt64());
            }
            public static Time operator *(DateTime One, Time Two)
            {
                return new Time(One.ToInt64() * Two.ToInt64());
            }
            public static Time operator *(Time One, DateTime Two)
            {
                return new Time(One.ToInt64() * Two.ToInt64());
            }

            public static Time operator /(Time One, Time Two)
            {
                return new Time(One.ToInt64() / Two.ToInt64());
            }
            public static Time operator /(Time One, long Two)
            {
                return new Time(One.ToInt64() / Two);
            }
            public static Time operator /(long One, Time Two)
            {
                return new Time(One / Two.ToInt64());
            }
            public static Time operator /(DateTime One, Time Two)
            {
                return new Time(One.ToInt64() / Two.ToInt64());
            }
            public static Time operator /(Time One, DateTime Two)
            {
                return new Time(One.ToInt64() / Two.ToInt64());
            }

            public static Time operator %(Time One, Time Two)
            {
                return new Time(One.ToInt64() % Two.ToInt64());
            }
            public static Time operator %(Time One, long Two)
            {
                return new Time(One.ToInt64() % Two);
            }
            public static Time operator %(long One, Time Two)
            {
                return new Time(One % Two.ToInt64());
            }
            public static Time operator %(DateTime One, Time Two)
            {
                return new Time(One.ToInt64() % Two.ToInt64());
            }
            public static Time operator %(Time One, DateTime Two)
            {
                return new Time(One.ToInt64() % Two.ToInt64());
            }

            public static Time operator &(Time One, Time Two)
            {
                return new Time(One.ToInt64() & Two.ToInt64());
            }
            public static Time operator &(Time One, long Two)
            {
                return new Time(One.ToInt64() & Two);
            }
            public static Time operator &(long One, Time Two)
            {
                return new Time(One & Two.ToInt64());
            }
            public static Time operator &(DateTime One, Time Two)
            {
                return new Time(One.ToInt64() & Two.ToInt64());
            }
            public static Time operator &(Time One, DateTime Two)
            {
                return new Time(One.ToInt64() & Two.ToInt64());
            }

            public static Time operator |(Time One, Time Two)
            {
                return new Time(One.ToInt64() | Two.ToInt64());
            }
            public static Time operator |(Time One, long Two)
            {
                return new Time(One.ToInt64() | Two);
            }
            public static Time operator |(long One, Time Two)
            {
                return new Time(One + Two.ToInt64());
            }
            public static Time operator |(DateTime One, Time Two)
            {
                return new Time(One.ToInt64() | Two.ToInt64());
            }
            public static Time operator |(Time One, DateTime Two)
            {
                return new Time(One.ToInt64() | Two.ToInt64());
            }

            public static Time operator ^(Time One, Time Two)
            {
                return new Time(One.ToInt64() ^ Two.ToInt64());
            }
            public static Time operator ^(Time One, long Two)
            {
                return new Time(One.ToInt64() ^ Two);
            }
            public static Time operator ^(long One, Time Two)
            {
                return new Time(One ^ Two.ToInt64());
            }
            public static Time operator ^(DateTime One, Time Two)
            {
                return new Time(One.ToInt64() ^ Two.ToInt64());
            }
            public static Time operator ^(Time One, DateTime Two)
            {
                return new Time(One.ToInt64() ^ Two.ToInt64());
            }

            public static bool operator >(Time One, Time Two)
            {
                return One.ToInt64() > Two.ToInt64();
            }
            public static bool operator <(Time One, Time Two)
            {
                return One.ToInt64() < Two.ToInt64();
            }
            public static bool operator >=(Time One, Time Two)
            {
                return One.ToInt64() >= Two.ToInt64();
            }
            public static bool operator <=(Time One, Time Two)
            {
                return One.ToInt64() <= Two.ToInt64();
            }
            public static bool operator ==(Time One, Time Two)
            {
                return One.ToInt64() == Two.ToInt64();
            }
            public static bool operator !=(Time One, Time Two)
            {
                return One.ToInt64() != Two.ToInt64();
            }

            public static bool operator >(Time One, long Two)
            {
                return One.ToInt64() > Two;
            }
            public static bool operator <(Time One, long Two)
            {
                return One.ToInt64() < Two;
            }
            public static bool operator >=(Time One, long Two)
            {
                return One.ToInt64() >= Two;
            }
            public static bool operator <=(Time One, long Two)
            {
                return One.ToInt64() <= Two;
            }
            public static bool operator ==(Time One, long Two)
            {
                return One.ToInt64() == Two;
            }
            public static bool operator !=(Time One, long Two)
            {
                return One.ToInt64() != Two;
            }

            public static bool operator >(Time One, DateTime Two)
            {
                return One.ToInt64() > Two.ToInt64();
            }
            public static bool operator <(Time One, DateTime Two)
            {
                return One.ToInt64() < Two.ToInt64();
            }
            public static bool operator >=(Time One, DateTime Two)
            {
                return One.ToInt64() >= Two.ToInt64();
            }
            public static bool operator <=(Time One, DateTime Two)
            {
                return One.ToInt64() <= Two.ToInt64();
            }
            public static bool operator ==(Time One, DateTime Two)
            {
                return One.ToInt64() == Two.ToInt64();
            }
            public static bool operator !=(Time One, DateTime Two)
            {
                return One.ToInt64() != Two.ToInt64();
            }

            public static bool operator >(long One, Time Two)
            {
                return One > Two.ToInt64();
            }
            public static bool operator <(long One, Time Two)
            {
                return One < Two.ToInt64();
            }
            public static bool operator >=(long One, Time Two)
            {
                return One >= Two.ToInt64();
            }
            public static bool operator <=(long One, Time Two)
            {
                return One <= Two.ToInt64();
            }
            public static bool operator ==(long One, Time Two)
            {
                return One == Two.ToInt64();
            }
            public static bool operator !=(long One, Time Two)
            {
                return One != Two.ToInt64();
            }

            public static bool operator >(DateTime One, Time Two)
            {
                return One.ToInt64() > Two.ToInt64();
            }
            public static bool operator <(DateTime One, Time Two)
            {
                return One.ToInt64() < Two.ToInt64();
            }
            public static bool operator >=(DateTime One, Time Two)
            {
                return One.ToInt64() >= Two.ToInt64();
            }
            public static bool operator <=(DateTime One, Time Two)
            {
                return One.ToInt64() <= Two.ToInt64();
            }
            public static bool operator ==(DateTime One, Time Two)
            {
                return One.ToInt64() == Two.ToInt64();
            }
            public static bool operator !=(DateTime One, Time Two)
            {
                return One.ToInt64() != Two.ToInt64();
            }

            public static implicit operator long(Time t) => t.ToInt64();
            public static implicit operator DateTime(Time t) => new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, t.Millisecond);
            public static implicit operator Time(long l) => new Time(l);
            public static implicit operator Time(double d) => new Time(d);
            public static implicit operator Time(DateTime d) => new Time(d);

            public override bool Equals(object obj)
            {
                if (obj is Time)
                {
                    return this == (Time) obj;
                }
                else if (obj is long)
                {
                    return this == (long) obj;
                }
                else if (obj is DateTime)
                {
                    return this == (DateTime) obj;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return ToInt64().GetHashCode();
            }

            public long ToInt64()
            {
                return Funcall("to_i").Convert<Integer>() * 1000 + Millisecond;
            }

            public bool IsMonday()
            {
                return Funcall("monday?") == True;
            }
            public bool IsTuesday()
            {
                return Funcall("tuesday?") == True;
            }
            public bool IsWednesday()
            {
                return Funcall("wednesday?") == True;
            }
            public bool IsThursday()
            {
                return Funcall("thursday?") == True;
            }
            public bool IsFriday()
            {
                return Funcall("friday?") == True;
            }
            public bool IsSaturday()
            {
                return Funcall("saturday?") == True;
            }
            public bool IsSunday()
            {
                return Funcall("sunday?") == True;
            }
            public Integer Year
            {
                get => Funcall("year").Convert<Integer>();
            }
            public Integer Month
            {
                get => Funcall("month").Convert<Integer>();
            }
            public Integer Day
            {
                get => Funcall("day").Convert<Integer>();
            }
            public Integer Hour
            {
                get => Funcall("hour").Convert<Integer>();
            }
            public Integer Minute
            {
                get => Funcall("min").Convert<Integer>();
            }
            public Integer Second
            {
                get => Funcall("sec").Convert<Integer>();
            }
            public Integer Millisecond
            {
                get => Funcall("nsec").Convert<Integer>() / 1000000;
            }
            public Integer Microsecond
            {
                get => Funcall("nsec").Convert<Integer>() / 1000;
            }
            public Integer Nanosecond
            {
                get => Funcall("nsec").Convert<Integer>();
            }
            public bool DST
            {
                get => Funcall("dst?") == True;
            }
        }
    }
}
