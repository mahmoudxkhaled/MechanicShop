using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace MechanicShop.Domain.Common.Results;

public static class Result
{
    public static Success Success => default;
    public static Created Created => default;
    public static Deleted Deleted => default;
    public static Updated Updated => default;

}


public sealed class Result<TValue> : IResult<TValue>
{
    private readonly TValue? _value = default;
    private readonly List<Error>? _errors = null;

    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    public List<Error> Errors => IsError ? _errors! : [];

    public TValue Value => IsSuccess ? _value! : default;

    public Error TopError => (_errors?.Count > 0) ? _errors[0] : default;


    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This constructor is for serialization purposes only. Use the static methods to create a Result instance.", true)]

    public Result(TValue? value, List<Error> errors, bool isSuccess)
    {
        if (isSuccess)
        {
            IsSuccess = true;
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _errors = [];
        }
        else
        {

            if (errors == null || errors.Count == 0)
            {
                throw new ArgumentException("Errors cannot be null or empty. provide at least one error", nameof(errors));

            }
            else
            {
                IsSuccess = false;
                _errors = errors;
                _value = default;

            }
        }
    }


    private Result(Error error)
    {
        _errors = [error];
    }

    private Result(List<Error> errors)
    {
        if (errors is null || errors.Count == 0)
            throw new ArgumentException("Errors cannot be null or empty. provide at least one error", nameof(errors));


        _errors = errors;
        IsSuccess = false;
    }


    private Result(TValue value)
    {

        if (value is null)
            throw new ArgumentNullException(nameof(value));

        _value = value;
        IsSuccess = true;
    }



    public TNextValue Match<TNextValue>(Func<TValue, TNextValue> onValue, Func<List<Error>, TNextValue> onError)
    {
        if (IsSuccess)
        {
            return onValue(Value!);
        }
        else
        {
            return onError(Errors);
        }
    }>

    public static implicit operator Result<TValue>(Error error) => new Result<TValue>(error);
    public static implicit operator Result<TValue>(List<Error> errors) => new Result<TValue>(errors);
    public static implicit operator Result<TValue>(TValue value) => new Result<TValue>(value);
}
public readonly record struct Success;
public readonly record struct Created;
public readonly record struct Deleted;
public readonly record struct Updated;


