﻿namespace XtbLibrairie.codes
{
    using System;

    /// <summary>
    ///     Base class for all xAPI codes.
    /// </summary>
    public class BaseCode
    {
        /// <summary>
        ///     Creates new base code object.
        /// </summary>
        /// <param name="code">Code represented as long value.</param>
        public BaseCode(long code)
        {
            Code = code;
        }

        /// <summary>
        ///     Raw code received from the API.
        /// </summary>
        public long Code { get; set; }

        [Obsolete("Use Code instead")]
        public long LongValue
        {
            get => Code;
            set => Code = value;
        }

        public static bool operator ==(BaseCode baseCode1, BaseCode baseCode2)
        {
            if (ReferenceEquals(baseCode1, baseCode2))
                return true;

            if ((object)baseCode1 == null || (object)baseCode2 == null)
                return false;

            return baseCode1.Code == baseCode2.Code;
        }

        public static bool operator !=(BaseCode baseCode1, BaseCode baseCode2)
        {
            return !(baseCode1 == baseCode2);
        }

        public override bool Equals(object target)
        {
            if (target == null)
                return false;

            var baseCode = target as BaseCode;
            if ((object)baseCode == null)
                return false;

            return Code == baseCode.Code;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [Obsolete("Use Code instead")]
        public long longValue()
        {
            return Code;
        }
    }
}