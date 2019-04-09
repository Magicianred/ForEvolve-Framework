﻿using System;

namespace ForEvolve.OperationResults
{
    /// <summary>
    /// Represents an operation result containing optional messages, generated by the operation.
    /// Implements the <see cref="ForEvolve.OperationResults.IOperationResult" />
    /// </summary>
    /// <seealso cref="ForEvolve.OperationResults.IOperationResult" />
    public class OperationResult : IOperationResult
    {
        /// <inheritdoc />
        public bool Succeeded => !Messages.HasError();

        /// <inheritdoc />
        public MessageCollection Messages { get; } = new MessageCollection();

        /// <inheritdoc />
        public bool HasMessages()
        {
            return Messages.Count > 0;
        }

        #region OperationResult Factory Methods

        public static IOperationResult Success()
        {
            return new OperationResult();
        }

        public static IOperationResult Failure(params IMessage[] messages)
        {
            if (messages == null || messages.Length == 0) { throw new ArgumentNullException(nameof(messages)); }
            var result = new OperationResult();
            result.Messages.AddRange(messages);
            return result;
        }

        public static IOperationResult Failure(Exception exception)
        {
            var result = new OperationResult();
            result.Messages.Add(new ExceptionMessage(exception));
            return result;
        }

        public static IOperationResult Failure(ProblemDetails problemDetails)
        {
            return Failure(problemDetails, OperationMessageLevel.Error);
        }

        public static IOperationResult Failure(ProblemDetails problemDetails, OperationMessageLevel severity)
        {
            var result = new OperationResult();
            result.Messages.Add(new ProblemDetailsMessage(problemDetails, severity));
            return result;
        }

        #endregion

        #region OperationResult<TValue> Factory Methods

        public static IOperationResult<TValue> Success<TValue>()
        {
            return new OperationResult<TValue>();
        }

        public static IOperationResult<TValue> Success<TValue>(TValue value)
        {
            return new OperationResult<TValue> { Value = value };
        }

        public static IOperationResult<TValue> Failure<TValue>(params IMessage[] messages)
        {
            var result = new OperationResult<TValue>();
            result.Messages.AddRange(messages);
            return result;
        }

        public static IOperationResult<TValue> Failure<TValue>(Exception exception)
        {
            var result = new OperationResult<TValue>();
            result.Messages.Add(new ExceptionMessage(exception));
            return result;
        }

        public static IOperationResult<TValue> Failure<TValue>(ProblemDetails problemDetails)
        {
            return Failure<TValue>(problemDetails, OperationMessageLevel.Error);
        }

        public static IOperationResult<TValue> Failure<TValue>(ProblemDetails problemDetails, OperationMessageLevel severity)
        {
            var result = new OperationResult<TValue>();
            result.Messages.Add(new ProblemDetailsMessage(problemDetails, severity));
            return result;
        }

        #endregion
    }

    /// <summary>
    /// Represents an operation result containing optional messages, generated by the operation, and an optional resulting object.
    /// Implements the <see cref="ForEvolve.OperationResults.OperationResult" />
    /// Implements the <see cref="ForEvolve.OperationResults.IOperationResult{TValue}" />
    /// </summary>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    /// <seealso cref="ForEvolve.OperationResults.OperationResult" />
    /// <seealso cref="ForEvolve.OperationResults.IOperationResult{TValue}" />
    public class OperationResult<TValue> : OperationResult, IOperationResult<TValue>
    {
        /// <inheritdoc />
        public TValue Value { get; set; }

        /// <inheritdoc />
        public bool HasValue()
        {
            return Value != null;
        }
    }

    public static class OperationResultExtensions
    {
        #region Conversion operators

        public static TOperationResult ConvertTo<TOperationResult>(
            this IOperationResult operationResult)
            where TOperationResult : IOperationResult
        {
            TOperationResult result;
            var type = typeof(TOperationResult);
            if (type.IsGenericType && type.Name.Equals("IOperationResult`1"))
            {
                var genericOperationResultType = typeof(OperationResult<>);
                var genericArgs = type.GetGenericArguments();
                var finalType = genericOperationResultType.MakeGenericType(genericArgs);
                result = (TOperationResult)Activator.CreateInstance(finalType);
            }
            else
            {
                var nonGenericResult = new OperationResult();
                nonGenericResult.Messages.AddRange(operationResult.Messages);
                result = (TOperationResult)(IOperationResult)nonGenericResult;
            }
            result.Messages.AddRange(operationResult.Messages);
            return result;
        }

        public static IOperationResult<TValue> ConvertTo<TOperationResult, TValue>(
            this IOperationResult operationResult)
            where TOperationResult : IOperationResult<TValue>
        {
            var genericResult = new OperationResult<TValue>();
            genericResult.Messages.AddRange(operationResult.Messages);
            return genericResult;
        }

        #endregion

        public static TOperationResult On<TOperationResult>(this TOperationResult operationResult, 
            Action<TOperationResult> success = null, 
            Action<TOperationResult> failure = null
            )
            where TOperationResult : IOperationResult
        {
            var result = operationResult;
            if (success != null)
            {
                result = result.OnSuccess(success);
            }
            if (failure != null)
            {
                result = result.OnFailure(failure);
            }
            return result;
        }

        public static TOperationResult OnSuccess<TOperationResult>(this TOperationResult operationResult, Action<TOperationResult> successDelegate)
            where TOperationResult : IOperationResult
        {
            if (operationResult == null) { throw new ArgumentNullException(nameof(operationResult)); }
            if (operationResult.Succeeded)
            {
                successDelegate(operationResult);
            }
            return operationResult;
        }

        public static TOperationResult OnFailure<TOperationResult>(this TOperationResult operationResult, Action<TOperationResult> failureDelegate)
            where TOperationResult : IOperationResult
        {
            if (operationResult == null) { throw new ArgumentNullException(nameof(operationResult)); }
            if (!operationResult.Succeeded)
            {
                failureDelegate(operationResult);
            }
            return operationResult;
        }
    }

    /// <summary>
    /// DELETE ME
    /// </summary>
    class MyClass
    {
        public IOperationResult Operation()
        {
            return OperationResult.Success();
        }

        public void Consumer()
        {
            Operation()
                .OnSuccess(r => Console.WriteLine("Success"))
                .OnFailure(r => Console.WriteLine("Failure!"));
        }

        public void Consumer2()
        {
            Operation().On(
                success: r => Console.WriteLine("Success"),
                failure: r => Console.WriteLine("Failure!")
            );
        }
    }
}
