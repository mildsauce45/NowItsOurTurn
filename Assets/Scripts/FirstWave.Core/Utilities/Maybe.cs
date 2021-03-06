﻿
namespace FirstWave.Core.Utilities
{
	public class Maybe<T>
	{
		public readonly static Maybe<T> Nothing = new Maybe<T>();

		public T Value { get; private set; }

		public bool HasValue { get; private set; }

		public Maybe()
		{
			HasValue = false;
		}

		public Maybe(T value)
		{
			Value = value;
			HasValue = true;
		}

		public static bool operator ==(Maybe<T> m, T value)
		{
			if (value == null)
				return !m.HasValue;

			return true;
		}

		public static bool operator !=(Maybe<T> m, T value)
		{
			return !(m == value);
		}

		public static bool operator ==(Maybe<T> m1, Maybe<T> m2)
		{
			if (m1.HasValue)
				return m2.HasValue && Equals(m1.Value, m2.Value);
			else if (!m1.HasValue)
				return !m2.HasValue;

			return false;
		}

		public static bool operator !=(Maybe<T> m1, Maybe<T> m2)
		{
			return !(m1 == m2);
		}

		public override bool Equals(object obj)
		{
			Maybe<T> o = obj as Maybe<T>;
			if (o == null)
				return false;

			if (o.HasValue)
				return HasValue && Equals(Value, o.Value);
			else if (!o.HasValue)
				return !HasValue;

			return false;
		}

		public override int GetHashCode()
		{
			if (HasValue)
				return 0;

			return Value.GetHashCode();
		}
	}
}
