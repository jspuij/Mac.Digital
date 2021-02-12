// <copyright file="BoilerException.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An exception that occurs when the boiler does not function.
    /// </summary>
    public class BoilerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerException"/> class.
        /// </summary>
        public BoilerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerException"/> class.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public BoilerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerException"/> class.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public BoilerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerException"/> class.
        /// </summary>
        /// <param name="info">The serialization info class.</param>
        /// <param name="context">The streaming context.</param>
        protected BoilerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
